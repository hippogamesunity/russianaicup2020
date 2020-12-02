using System;
using System.Collections.Generic;
using System.Linq;

namespace aicup2020.Common
{
    public class DijkstraVertex
    {
        public Position Position;
        public double SpeedLeft;
        public Position Previous;
    }

    public static class DijkstraMap
    {
        public static bool DiagonalOffset;
        public static double DiagonalSpeedCost = Math.Sqrt(2);

        private static readonly List<DijkstraVertex> DijkstraVertices = new List<DijkstraVertex>();
        private static readonly List<Position> Offsets4 = new List<Position> { new Position(-1, 0), new Position(1, 0), new Position(0, -1), new Position(0, 1) };
        private static readonly List<Position> Offsets8 = new List<Position> { new Position(-1, 0), new Position(1, 0), new Position(0, -1), new Position(0, 1), new Position(-1, -1), new Position(-1, 1), new Position(1, -1), new Position(1, 1) };

        public static List<DijkstraVertex> GetDijkstraVertices(bool[,] graph, Position from, double speed)
        {
            var start = new DijkstraVertex { Position = from, SpeedLeft = speed, Previous = from };

            DijkstraVertices.Clear();
            FindDijkstraVertices(graph, start);

            return DijkstraVertices;
        }

        public static List<Position> FindRoute(bool[,] graph, Position from, Position to, double speed)
        {
            var vertices = GetDijkstraVertices(graph, from, speed);

            if (vertices.Count <= 0) return null;

            var route = new List<Position>();
            var last = vertices.SingleOrDefault(i => i.Position == to);

            if (last == null)
            {
                return null;
            }

            while (last.Previous != @from)
            {
                route.Add(last.Position);
                last = vertices.Single(i => i.Position == last.Previous);
            }

            route.Add(last.Position);
            route.Add(from);
            route.Reverse();

            return route;
        }

        private static void FindDijkstraVertices(bool[,] graph, DijkstraVertex start)
        {
            foreach (var offset in DiagonalOffset ? Offsets8 : Offsets4)
            {
                var currentPosition = start.Position;
                var nextPosition = start.Position + offset;

                if (nextPosition.X < 0 || nextPosition.X >= graph.GetLength(0) || nextPosition.Y < 0 || nextPosition.Y >= graph.GetLength(1)) continue;
                if (!graph[currentPosition.X, currentPosition.Y] || !graph[nextPosition.X, nextPosition.Y]) continue;

                var speedLeft = start.SpeedLeft - (DiagonalOffset ? DiagonalSpeedCost : 1);

                if (speedLeft < 0) continue;

                var vertex = DijkstraVertices.FirstOrDefault(i => i.Position == nextPosition);

                if (vertex == null)
                {
                    vertex = new DijkstraVertex
                    {
                        Position = nextPosition.Copy(),
                        SpeedLeft = speedLeft,
                        Previous = currentPosition.Copy(),
                    };

                    DijkstraVertices.Add(vertex);
                    FindDijkstraVertices(graph, vertex);
                }
                else if (vertex.SpeedLeft < speedLeft)
                {
                    vertex.SpeedLeft = speedLeft;
                    vertex.Previous = currentPosition.Copy();
                    FindDijkstraVertices(graph, vertex);
                }
            }
        }
    }
}