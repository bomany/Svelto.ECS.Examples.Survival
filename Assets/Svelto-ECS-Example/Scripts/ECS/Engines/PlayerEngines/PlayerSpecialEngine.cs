using UnityEngine;
using System;
using System.Collections;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.Player.Special
{
    public class PlayerSpecialEngine : MultiEntityViewsEngine<SpecialEntityView, PlayerEntityView>, IQueryingEntityViewEngine
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {
            _taskRoutine.Start();
        }

        public PlayerSpecialEngine(ITime time, IPhysics physics)
        {
            _time        = time;
            _physics     = physics;

            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(Tick())
                                               .SetScheduler(StandardSchedulers.physicScheduler);
        }

        protected override void Add(SpecialEntityView entityView)
        {
            _specialEntityView = entityView;
        }

        protected override void Remove(SpecialEntityView entityView)
        {
            _taskRoutine.Stop();
            _specialEntityView = null;
        }

        protected override void Add(PlayerEntityView entityView)
        {
            _playerEntityView = entityView;
        }

        protected override void Remove(PlayerEntityView entityView)
        {
            _taskRoutine.Stop();
            _playerEntityView = null;
        }

        IEnumerator Tick()
        {
            while (_playerEntityView == null || _specialEntityView == null) yield return null;

            while (true)
            {
                var playerSpecialComponent = _specialEntityView.specialComponent;

                playerSpecialComponent.timer += _time.deltaTime;

                if (_playerEntityView.inputComponent.special &&
                    playerSpecialComponent.timer >= playerSpecialComponent.cooldown)
                    Trigger(_specialEntityView);

                yield return null;
            }
        }

        void Trigger(SpecialEntityView specialEntityView)
        {
            var playerSpecialComponent = specialEntityView.specialComponent;
            var position = _playerEntityView.positionComponent.position;
            playerSpecialComponent.timer = 0;

            int[] ids = _physics.OverlapSphere(position, playerSpecialComponent.range, ENEMY_MASK);

            for (var i = 0; i <= ids.Length-1; i++)
            {
                PlayerTargetEntityView targetEntityView;
                if (entityViewsDB.TryQueryEntityView(ids[i], out targetEntityView))
                {
                    var targetPosition = targetEntityView.positionComponent.position;
                    var direction = (targetPosition - position).normalized;
                    var force = Math.Abs(Vector3.Distance(position, targetPosition) - playerSpecialComponent.range)
                        / playerSpecialComponent.range
                        * playerSpecialComponent.maxForce;

                    targetEntityView.rigidBodyComponent.velocity = direction * force;
                }
            }

        }

        PlayerEntityView        _playerEntityView;
        SpecialEntityView       _specialEntityView;

        readonly ITaskRoutine   _taskRoutine;
        readonly IPhysics       _physics;
        readonly ITime          _time;

        static readonly int ENEMY_MASK = LayerMask.GetMask("Enemies");
        static readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemies");
    }
}

