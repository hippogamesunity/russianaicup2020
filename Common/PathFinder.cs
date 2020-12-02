using System.Collections.Generic;
using System.Linq;
using Aicup2020.Model;

namespace aicup2020.Common
{
    public static class PathFinder
    {
        private static bool[,] _graph;

        public static void Initialize()
        {
            if (_graph == null)
            {
                _graph = new bool[PlayerView.Instance.MapSize, PlayerView.Instance.MapSize];
            }

            for (var x = 0; x < PlayerView.Instance.MapSize; x++)
            {
                for (var y = 0; y < PlayerView.Instance.MapSize; y++)
                {
                    _graph[x, y] = true;
                }
            }

            foreach (var entity in PlayerView.Instance.Entities)
            {
                for (var x = entity.Position.X; x < entity.Position.X + entity.Properties.Size; x++)
                {
                    for (var y = entity.Position.Y; y < entity.Position.Y + entity.Properties.Size; y++)
                    {
                        _graph[x, y] = false;
                    }
                }
            }
        }

        public static List<Vec2Int> FindRouteByWavePath(Entity from, Entity to)
        {
            return WavePath.FindRoute(_graph, new Position(from.Position), new Position(to.Position), diagonal: false)?.Select(i => i.ToVec2Int()).ToList();
        }

        public static List<Vec2Int> FindRouteByDijkstraMap(Entity from, Entity to)
        {
            return DijkstraMap.FindRoute(_graph, new Position(from.Position), new Position(to.Position), speed: 99)?.Select(i => i.ToVec2Int()).ToList();
        }
    }
}