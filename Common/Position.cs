using System;
using Aicup2020.Model;

namespace aicup2020.Common
{
    [Serializable]
    public class Position
    {
        public int X;
        public int Y;

        public Position()
        {
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position(Vec2Int position)
        {
            X = position.X;
            Y = position.Y;
        }

        public Vec2Int ToVec2Int()
        {
            return new Vec2Int(X, Y);
        }

        public bool Equals(Position other)
        {
            if (ReferenceEquals(null, other)) return false;

            return X == other.X && Y == other.Y;
        }

        public override string ToString()
        {
            return X + ":" + Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Position)obj);
        }

        public override int GetHashCode()
        {
            return X * 357 + Y;
        }

        public static bool operator ==(Position a, Position b)
        {
            if (ReferenceEquals(null, a)) return ReferenceEquals(null, b);

            return a.Equals(b);
        }

        public static bool operator !=(Position a, Position b)
        {
            if (ReferenceEquals(null, a)) return !ReferenceEquals(null, b);

            return !a.Equals(b);
        }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        public Position Copy()
        {
            return new Position(X, Y);
        }

        public static double Distance(Position a, Position b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public static int Steps(Position a, Position b)
        {
            return Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y);
        }
    }
}