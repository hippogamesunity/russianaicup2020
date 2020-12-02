using System.Collections.Generic;
using System.Linq;
using Aicup2020.Model;

namespace aicup2020.Common
{
    public static class EntityHelper
    {
        public static bool FindBestBuilderSpawnPosition(Entity builderBase, out Vec2Int spawn)
        {
            spawn = new Vec2Int();

            var size = PlayerView.Instance.EntityProperties[builderBase.EntityType].Size;
            var position = builderBase.Position;
            var spawns = new List<Vec2Int>();

            for (var x = position.X - 1; x <= position.X + size; x++)
            {
                for (var y = position.Y - 1; y <= position.Y + size; y++)
                {
                    var p = new Vec2Int(x, y);

                    if (PlayerView.Instance.Entities.Any(i => i.Position.Equals(p))) continue; // TODO: Size not checked.

                    if ((x == position.X - 1 || x == position.X + size) && y != position.Y - 1 && y != position.Y + size)
                    {
                        spawns.Add(p);
                    }
                    else if ((y == position.Y - 1 || y == position.Y + size) && x != position.X - 1 && x != position.X + size)
                    {
                        spawns.Add(p);
                    }
                }
            }

            if (spawns.Count == 0) return false;

            var resources = PlayerView.Instance.Entities.Where(i => i.EntityType == EntityType.Resource && i.Health == PlayerView.Instance.EntityProperties[i.EntityType].MaxHealth).OrderBy(i => i.Distance(builderBase)).ToList();

            if (resources.Count == 0) return false;

            spawn = spawns.OrderBy(i => Extensions.Distance(i, 1, resources[0].Position, PlayerView.Instance.EntityProperties[resources[0].EntityType].Size)).First();

            return true;
        }
    }
}