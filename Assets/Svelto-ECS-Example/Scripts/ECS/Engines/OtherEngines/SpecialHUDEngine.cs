using Svelto.ECS.Example.Survive.Player.Special;
using UnityEngine;
using System;
using System.Collections;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.HUD
{
    public class SpecialHUDEngine : MultiEntityViewsEngine<SpecialEntityView, HUDEntityView>
    {

        public SpecialHUDEngine()
        {
            _taskRoutine = TaskRunner.Instance.AllocateNewTaskRoutine().SetEnumerator(UpdateHud())
                                               .SetScheduler(StandardSchedulers.lateScheduler);
            _taskRoutine.Start();
        }
        protected override void Add(SpecialEntityView entityView)
        {
            _specialEntityView = entityView;       
            //_taskRoutine.Start();
        }

        protected override void Remove(SpecialEntityView entityView)
        {
            _specialEntityView = null;
            _taskRoutine.Stop();
        }

        protected override void Add(HUDEntityView entityView)
        {
            _hudEntityView = entityView;
        }

        protected override void Remove(HUDEntityView entityView)
        {
            _hudEntityView = null;
        }

        IEnumerator UpdateHud()
        {
            while (_hudEntityView == null || _specialEntityView == null) yield return null;

            while (true)
            {
                var playerSpecialComponent = _specialEntityView.specialComponent;
                var specialHudComponent = _hudEntityView.specialComponent;
                var timer = playerSpecialComponent.cooldown - playerSpecialComponent.timer;

                if (timer > 0)
                {
                    specialHudComponent.showTimer = true;
                    specialHudComponent.timer = timer;
                }
                else
                {
                    specialHudComponent.showTimer = false;
                }
                    
                             
                 
                yield return null;
            }

        }

        SpecialEntityView _specialEntityView;
        HUDEntityView _hudEntityView;
        readonly ITaskRoutine _taskRoutine;
    }
}
