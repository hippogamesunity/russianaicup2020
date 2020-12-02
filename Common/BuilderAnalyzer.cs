using System.Collections.Generic;
using System.Linq;
using Aicup2020.Model;

namespace aicup2020.Common
{
    public static class BuilderAnalyzer
    {
        public static Dictionary<int, int> Statistics = new Dictionary<int, int>();
        public static bool[,] ResourceMap;
        public const int PeriodTicks = 100;

        public static void Refresh()
        {
            if (ResourceMap == null)
            {
                ResourceMap = new bool[PlayerView.Instance.MapSize, PlayerView.Instance.MapSize];
            }

            for (var x = 0; x < PlayerView.Instance.MapSize; x++)
            {
                for (var y = 0; y < PlayerView.Instance.MapSize; y++)
                {
                    ResourceMap[x, y] = false;
                }
            }

            foreach (var resource in PlayerView.Instance.Entities.Where(i => i.EntityType == EntityType.Resource))
            {
                ResourceMap[resource.Position.X, resource.Position.Y] = true;
            }

            foreach (var builder in PlayerView.Instance.Entities.Where(i => i.EntityType == EntityType.BuilderUnit && i.PlayerId == PlayerView.Instance.MyId))
            {
                if (!Statistics.ContainsKey(builder.Id)) Statistics.Add(builder.Id, PeriodTicks);

                var nearPositions = new[]
                {
                    new Vec2Int(builder.Position.X - 1, builder.Position.Y),
                    new Vec2Int(builder.Position.X + 1, builder.Position.Y),
                    new Vec2Int(builder.Position.X, builder.Position.Y - 1),
                    new Vec2Int(builder.Position.X, builder.Position.Y + 1)
                };

                if (nearPositions.Any(p => p.IsInsideMap() && ResourceMap[p.X, p.Y]))
                {
                    Statistics[builder.Id]++;

                    if (Statistics[builder.Id] > PeriodTicks) Statistics[builder.Id] = PeriodTicks;
                }
                else
                {
                    Statistics[builder.Id]--;

                    if (Statistics[builder.Id] < 0) Statistics[builder.Id] = 0;
                }
            }
        }
    }
}