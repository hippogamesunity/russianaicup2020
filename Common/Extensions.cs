using System;
using Aicup2020.Model;

namespace aicup2020.Common
{
    public static class Extensions
    {
        public static double Distance(this Entity from, Entity to)
        {
            return Distance(from.Position, PlayerView.Instance.EntityProperties[from.EntityType].Size, to.Position, PlayerView.Instance.EntityProperties[from.EntityType].Size);
        }

        private static double Distance(Vec2Int a, int aSize, Vec2Int b, int bSize)
        {
            return Math.Sqrt(Math.Pow(a.X + aSize / 2d - b.X - bSize / 2d, 2) + Math.Pow(a.Y + aSize / 2d - b.Y - bSize / 2d, 2));
        }
    }
}