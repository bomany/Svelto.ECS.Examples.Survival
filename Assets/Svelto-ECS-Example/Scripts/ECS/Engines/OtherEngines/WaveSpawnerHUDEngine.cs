using Svelto.ECS.Example.Survive.Player.Special;
using UnityEngine;
using System;
using System.Collections;
using Svelto.Tasks;

namespace Svelto.ECS.Example.Survive.HUD
{
    public class WaveSpawnerHUDEngine : SingleEntityViewEngine<HUDEntityView>,
                                        IQueryingEntityViewEngine,
                                        IStep<WaveInfo>,
                                        IStep<DamageInfo>
    {

        public IEntityViewsDB entityViewsDB { set; private get; }

        public WaveSpawnerHUDEngine(ITime time)
        {
            _time = time;
            _enemyCount = 0;
            _currentWave = 0;
        }

        protected override void Add(HUDEntityView entityView)
        {
            _hudEntityView = entityView;
        }

        protected override void Remove(HUDEntityView entityView)
        {
            _hudEntityView = null;
        }

        public void Ready()
        {
            Tick().Run();
        }

        void UpdateEnemyCount()
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
            {
                hudEntityViews[i].enemyCountComponent.total = _enemyCount;
            }
        }

        void FlashWaveWarning()
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
            {
                var waveComponent = hudEntityViews[i].waveWarningComponent;
                waveComponent.textColor = waveComponent.flashColor;
                waveComponent.wave = _currentWave;
            }
        }

        IEnumerator Tick()
        {
            while (true)
            {
                var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
                for (int i = 0; i < hudEntityViews.Count; i++)
                {
                    var waveComponent = hudEntityViews[i].waveWarningComponent;

                    waveComponent.textColor = Color.Lerp(waveComponent.textColor, Color.clear,
                        waveComponent.flashSpeed * _time.deltaTime);
                }

                yield return null;
            }
        }

        public void Step(ref WaveInfo wave, int state)
        {
            _enemyCount += wave.enemyCount;
            _currentWave = wave.wave;
            UpdateEnemyCount();
            FlashWaveWarning();
        }

        public void Step(ref DamageInfo damage, int type)
        {
            if (type == DamageCondition.Dead)
            {
                _enemyCount--;
                UpdateEnemyCount();
            }
        }

        HUDEntityView       _hudEntityView;
        readonly ITime      _time;
        int                 _enemyCount;
        int                 _currentWave;
    }
}
