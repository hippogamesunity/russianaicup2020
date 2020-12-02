using System;
using System.Collections.Generic;
using System.Linq;

namespace aicup2020.Common
{
    public static class WavePath
    {
        private static readonly List<Position> Offsets4 = new List<Position> { new Position(-1, 0), new Position(0, 1), new Position(1, 0), new Position(0, -1) };
        private static readonly List<Position> Offsets8 = new List<Position> { new Position(-1, 0), new Position(-1, 1), new Position(0, 1), new Position(1, 1), new Position(1, 0), new Position(1, -1), new Position(0, -1), new Position(-1, -1) };
        private static List<Position> _offsets;
        private static int[,] _graph;

        public static List<Position> FindRoute(bool[,] graph, Position from, Position to, bool diagonal)
        {
            return FindRoute(graph, from, new List<Position> { to }, diagonal);
        }

        public static List<Position> FindRoute(bool[,] graph, Position from, List<Position> to, bool diagonal)
        {
            //if (to.Any(i => !graph[i.X, i.Y])) return null;
            if (to.Any(i => i == from)) return new List<Position>();

            InitializeGraph(graph, diagonal);

            if (OutOfBounds(from) || to.Any(OutOfBounds)) return null;

            var finish = CreateWaves(from, to);

            return finish == null ? null : RestoreRoute(finish);
        }

        public static List<Position> GetRange(bool[,] graph, Position from, int steps, bool diagonal)
        {
            InitializeGraph(graph, diagonal);

            if (OutOfBounds(from)) return null;

            return CreateWaves(from, steps);
        }

        private static void InitializeGraph(bool[,] graph, bool diagonal)
        {
            _offsets = diagonal ? Offsets8 : Offsets4;
            _graph = new int[graph.GetLength(0), graph.GetLength(1)];

            for (var x = 0; x < graph.GetLength(0); x++)
            {
                for (var y = 0; y < graph.GetLength(1); y++)
                {
                    _graph[x, y] = graph[x, y] ? 0 : -1;
                }
            }
        }

        private static Position CreateWaves(Position from, List<Position> to)
        {
            var wave = new List<Position> { from };
            var count = 1000;

            _graph[from.X, from.Y] = 0;

            while (wave.Count > 0 && count > 0)
            {
                count--;
                var increment = new List<Position>();

                foreach (var position in wave)
                {
                    foreach (var offset in _offsets)
                    {
                        var nextPosition = position + offset;

                        if (OutOfBounds(nextPosition)) continue;

                        if (to.Any(i => i == nextPosition))
                        {
                            _graph[nextPosition.X, nextPosition.Y] = _graph[position.X, position.Y] + 1;

                            return nextPosition;
                        }

                        if (_graph[nextPosition.X, nextPosition.Y] == 0 && nextPosition != from)
                        {
                            _graph[nextPosition.X, nextPosition.Y] = _graph[position.X, position.Y] + 1;
                            increment.Add(nextPosition);
                        }
                    }
                }

                wave = increment;
            }

            if (count == 0) throw new Exception("Loop!");

            return null;
        }

        private static List<Position> CreateWaves(Position from, int steps)
        {
            var wave = new List<Position> { from };
            var result = new List<Position>();

            _graph[from.X, from.Y] = 0;

            while (wave.Count > 0 && steps > 0)
            {
                steps--;
                var increment = new List<Position>();

                foreach (var position in wave)
                {
                    foreach (var offset in _offsets)
                    {
                        var nextPosition = position + offset;

                        if (OutOfBounds(nextPosition)) continue;

                        if (_graph[nextPosition.X, nextPosition.Y] == 0 && nextPosition != from)
                        {
                            _graph[nextPosition.X, nextPosition.Y] = _graph[position.X, position.Y] + 1;
                            increment.Add(nextPosition);
                        }
                    }
                }

                wave = increment;
                result.AddRange(increment);
            }

            return result;
        }

        private static List<Position> RestoreRoute(Position finish)
        {
            var route = new List<Position> { finish };
            var position = new Position(finish.X, finish.Y);
            var count = 1000;

            while (count > 0)
            {
                count--;

                foreach (var offset in _offsets)
                {
                    var nextPosition = position + offset;

                    if (OutOfBounds(nextPosition)) continue;

                    if (_graph[nextPosition.X, nextPosition.Y] == _graph[position.X, position.Y] - 1)
                    {
                        route.Add(nextPosition);

                        if (_graph[nextPosition.X, nextPosition.Y] == 0)
                        {
                            route.Reverse();
                            return route;
                        }

                        position = nextPosition;
                        break;
                    }
                }
            }

            throw new Exception("Loop!");
        }

        private static bool OutOfBounds(Position position)
        {
            return position.X < 0 || position.X >= _graph.GetLength(0) || position.Y < 0 || position.Y >= _graph.GetLength(1);
        }
    }
}