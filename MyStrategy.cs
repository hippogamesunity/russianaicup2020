using System;
using System.Collections.Generic;
using System.Linq;
using Aicup2020.Model;
using Action = Aicup2020.Model.Action;

namespace Aicup2020
{
    public class MyStrategy
    {
        public Action GetAction(PlayerView playerView, DebugInterface debugInterface)
        {
            PlayerView.Instance = playerView;

            var actions = new Dictionary<int, EntityAction>();

            foreach (var builder in playerView.Entities.Where(i => i.EntityType == EntityType.BuilderUnit && i.PlayerId == playerView.MyId))
            {
                var resource = playerView.Entities.Where(i => i.EntityType == EntityType.Resource).OrderBy(i => Distance(i, builder)).First();

                actions.Add(builder.Id, new EntityAction(new MoveAction(resource.Position, true, true), null, new AttackAction(resource.Id, new AutoAttack(999, new[] { EntityType.Resource })), null));
            }
            
            var builderBase = playerView.Entities.First(i => i.EntityType == EntityType.BuilderBase && i.PlayerId == playerView.MyId);

            if (FindBestBuilderSpawnPosition(builderBase, out var spawn))
            {
                actions.Add(builderBase.Id, new EntityAction(null, new BuildAction(EntityType.BuilderUnit, spawn), null, null));
            }

            return new Action(actions);
        }

        public void DebugUpdate(PlayerView playerView, DebugInterface debugInterface) 
        {
            debugInterface.Send(new DebugCommand.Clear());
            debugInterface.GetState();
        }

        private double Distance(Entity a, Entity b)
        {
            return Distance(a.Position, PlayerView.Instance.EntityProperties[a.EntityType].Size, b.Position, PlayerView.Instance.EntityProperties[b.EntityType].Size);
        }

        private static double Distance(Vec2Int a, int aSize, Vec2Int b, int bSize)
        {
            return Math.Sqrt(Math.Pow(a.X + aSize / 2d - b.X - bSize / 2d, 2) + Math.Pow(a.Y + aSize / 2d - b.Y - bSize / 2d, 2));
        }

        private bool FindBestBuilderSpawnPosition(Entity builderBase, out Vec2Int spawn)
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

            var resources = PlayerView.Instance.Entities.Where(i => i.EntityType == EntityType.Resource && i.Health == PlayerView.Instance.EntityProperties[i.EntityType].MaxHealth).OrderBy(i => Distance(i, builderBase)).ToList();

            if (resources.Count == 0) return false;

            spawn = spawns.OrderBy(i => Distance(i, 1, resources[0].Position, PlayerView.Instance.EntityProperties[resources[0].EntityType].Size)).First();

            return true;
        }
    }
}