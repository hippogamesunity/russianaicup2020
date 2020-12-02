using System;
using System.Collections.Generic;
using Aicup2020.Model;

namespace aicup2020.Common
{
    public static class Extensions
    {
        public static double Distance(this Entity from, Entity to)
        {
            return Distance(from.Position, PlayerView.Instance.EntityProperties[from.EntityType].Size, to.Position, PlayerView.Instance.EntityProperties[from.EntityType].Size);
        }

        public static List<Vec2Int> RouteTo(this Entity from, Entity to)
        {
            return PathFinder.FindRouteByWavePath(from, to);
        }

        public static double Distance(Vec2Int a, int aSize, Vec2Int b, int bSize)
        {
            return Math.Sqrt(Math.Pow(a.X + aSize / 2d - b.X - bSize / 2d, 2) + Math.Pow(a.Y + aSize / 2d - b.Y - bSize / 2d, 2));
        }

        public static bool IsInsideMap(this Vec2Int position)
        {
            return position.X >= 0 && position.X < PlayerView.Instance.MapSize && position.Y >= 0 && position.Y < PlayerView.Instance.MapSize;
        }
    }
}