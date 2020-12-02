using System.Collections.Generic;
using System.Linq;
using aicup2020.Common;
using Aicup2020.Model;
using Action = Aicup2020.Model.Action;

namespace Aicup2020
{
    public class MyStrategy
    {
        public Action GetAction(PlayerView playerView, DebugInterface debugInterface)
        {
            PlayerView.Instance = playerView;
            PathFinder.Refresh();
            BuilderAnalyzer.Refresh();

            var actions = new Dictionary<int, EntityAction>();

            foreach (var builder in playerView.Entities.Where(i => i.EntityType == EntityType.BuilderUnit && i.PlayerId == playerView.MyId))
            {
                var resource = playerView.Entities.Where(i => i.EntityType == EntityType.Resource).OrderBy(i => i.Distance(builder)).First();
                var path = builder.RouteTo(resource);

                if (path != null && path.Count > 1)
                {
                    actions.Add(builder.Id, new EntityAction(new MoveAction(path[1], true, true), null, new AttackAction(resource.Id, new AutoAttack(1, new[] { EntityType.Resource })), null));
                }
                else
                {
                    actions.Add(builder.Id, new EntityAction(new MoveAction(resource.Position, true, true), null, new AttackAction(resource.Id, new AutoAttack(1, new[] { EntityType.Resource })), null));
                }
            }
            
            var builderBase = playerView.Entities.First(i => i.EntityType == EntityType.BuilderBase && i.PlayerId == playerView.MyId);

            if (EntityHelper.FindBestBuilderSpawnPosition(builderBase, out var spawn))
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
    }
}