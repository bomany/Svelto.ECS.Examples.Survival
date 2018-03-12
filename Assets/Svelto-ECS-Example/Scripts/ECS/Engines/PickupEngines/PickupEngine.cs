using UnityEngine;
using Svelto.Tasks;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Player.Pickup
{
    public class PickupEngine : SingleEntityViewEngine<PlayerEntityView>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
        }

        public PickupEngine(ISequencer pickupSequence)
        {
            _pickupSequence = pickupSequence;
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(CheckIfTouchingPlayer()).SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(PlayerEntityView entity)
        {
            _taskRoutine.Start();
        }

        protected override void Remove(PlayerEntityView obj)
        {
            _taskRoutine.Stop();
        }

        IEnumerator CheckIfTouchingPlayer()
        {
            while (true)
            {
                var targetEntitiesView = entityViewsDB.QueryEntityViews<PlayerEntityView>();
                var pickupList = entityViewsDB.QueryEntityViews<PickupEntityView>();

                for (int pickupIndex = 0; pickupIndex < pickupList.Count; pickupIndex++)
                {
                    var pickupEntityView = pickupList[pickupIndex];
                    var pickupCollisionData = pickupEntityView.targetTriggerComponent.entityInRange;

                    for (int targetIndex = 0; targetIndex < targetEntitiesView.Count; targetIndex++)
                    {
                        var targetEntityView = targetEntitiesView[targetIndex];

                        if (pickupCollisionData.collides == true &&
                            !pickupEntityView.pickupComponent.empty &&
                            pickupCollisionData.otherEntityID == targetEntityView.ID)
                        {
                            pickupEntityView.pickupComponent.empty = true;
                            var pickupInfo = new PickupInfo(pickupEntityView.pickupComponent.amount, pickupCollisionData.otherEntityID, pickupEntityView.ID);
                            _pickupSequence.Next(this, ref pickupInfo, (int)pickupEntityView.pickupComponent.pickupType);
                        }
                    }
                }

                yield return null;
            }
        }

        ISequencer _pickupSequence;
        ITaskRoutine _taskRoutine;
    }
}
