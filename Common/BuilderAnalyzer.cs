using System.Collections.Generic;
using System.Linq;
using Aicup2020.Model;

namespace aicup2020.Common
{
    public static class BuilderAnalyzer
    {
        public static Dictionary<int, int> Statistics;
        public const int PeriodTicks = 100;

        public static void Refresh()
        {
            var resourceMap = new bool[PlayerView.Instance.MapSize, PlayerView.Instance.MapSize];

            foreach (var resource in PlayerView.Instance.Entities.Where(i => i.EntityType == EntityType.Resource))
            {
                resourceMap[resource.Position.X, resource.Position.Y] = true;
            }

            foreach (var builder in PlayerView.Instance.Entities.Where(i => i.EntityType == EntityType.BuilderBase))
            {
                if (!Statistics.ContainsKey(builder.Id)) Statistics.Add(builder.Id, 0);

                foreach (var position in new[] { new Vec2Int(builder.Position.X - 1, builder.Position.Y), new Vec2Int(builder.Position.X + 1, builder.Position.Y), new Vec2Int(builder.Position.X, builder.Position.Y - 1), new Vec2Int(builder.Position.X, builder.Position.Y + 1) })
                {
                    if (!position.IsInsideMap()) continue;

                    if (resourceMap[position.X, position.Y])
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
}