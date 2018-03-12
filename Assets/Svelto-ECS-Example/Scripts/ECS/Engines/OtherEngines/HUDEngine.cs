using Svelto.Tasks.Enumerators;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Svelto.ECS.Example.Survive.Player.Gun;
using System.Linq;

namespace Svelto.ECS.Example.Survive.HUD
{
    /// <summary>
    /// 
    /// You may wonder why I use QueryEntityViews instead to hold a reference
    /// of the only HudEntityView existing in the game or just using QueryEntityView.
    /// This is for learning purposes. An engine shouldn't really have the concept
    /// of how many entities are created. Using the QueryEntityView could be awkward
    /// because the entity ID is needed.
    /// Therefore using the Add/Remove callbacks is not wrong, but I try to not
    /// promote their use. 
    /// </summary>
    public class HUDEngine : SingleEntityViewEngine<GunEntityView>, IQueryingEntityViewEngine, IStep<DamageInfo>, IStep<PickupInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public HUDEngine(ITime time)
        {
            _time = time;
        }

        protected override void Add(GunEntityView entityView)
        {
            _playerGunEntityView = entityView;
            entityView.gunHitTargetComponent.targetHit.NotifyOnValueSet(OnFireEvent);
        }

        protected override void Remove(GunEntityView entityView)
        {
            _playerGunEntityView = null;
        }

        void OnFireEvent(int ID, bool targetHasBeenHit)
        {
            UpdateAmmo();
        }

        public void Ready()
        {
            Tick().Run();
        }

        IEnumerator Tick()
        {
            while (true)
            {
                var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
                for (int i = 0; i < hudEntityViews.Count; i++)
                {
                    var damageComponent = hudEntityViews[i].damageImageComponent;

                    damageComponent.imageColor = Color.Lerp(damageComponent.imageColor, Color.clear,
                        damageComponent.speed * _time.deltaTime);
                }
                
                yield return null;
            }
        }

        void OnDamageEvent(DamageInfo damaged)
        {
            UpdateSlider(damaged);
        }

        void OnDeadEvent()
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
                hudEntityViews[i].healthSliderComponent.value = 0;

            RestartLevelAfterFewSeconds().Run();
        }

        void UpdateSlider(DamageInfo damaged)
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
            {
                var guiEntityView = hudEntityViews[i];
                var damageComponent = guiEntityView.damageImageComponent;

                damageComponent.imageColor = damageComponent.flashColor;

                var hudDamageEntityView =
                    entityViewsDB.QueryEntityView<HUDDamageEntityView>(damaged.entityDamagedID);

                guiEntityView.healthSliderComponent.value = hudDamageEntityView.healthComponent.currentHealth;
            }
        }

        void UpdateSlider(PickupInfo pickup)
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
            {
                var guiEntityView = hudEntityViews[i];
                var damageComponent = guiEntityView.damageImageComponent;

                damageComponent.imageColor = damageComponent.flashColor;

                var hudDamageEntityView =
                    entityViewsDB.QueryEntityView<HUDDamageEntityView>(pickup.triggerEntityID);

                guiEntityView.healthSliderComponent.value = hudDamageEntityView.healthComponent.currentHealth;
            }
        }

        void UpdateAmmo()
        {
            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
            {
                var guiEntityView = hudEntityViews[i];

                if (_playerGunEntityView != null)
                {
                    guiEntityView.ammoComponent.ammo = _playerGunEntityView.gunComponent.ammo;
                }
            }
        }

        IEnumerator RestartLevelAfterFewSeconds()
        {
            _waitForSeconds.Reset(5);
            yield return _waitForSeconds;

            var hudEntityViews = entityViewsDB.QueryEntityViews<HUDEntityView>();
            for (int i = 0; i < hudEntityViews.Count; i++)
                hudEntityViews[i].HUDAnimator.trigger = "GameOver";

            _waitForSeconds.Reset(2);
            yield return _waitForSeconds;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Step(ref DamageInfo token, int condition)
        {
            if (condition == DamageCondition.Damage)
                OnDamageEvent(token);
            else
            if (condition == DamageCondition.Dead)
                OnDeadEvent();
        }

        public void Step(ref PickupInfo token, int type)
        {
            if (type == (int)PickupType.Health)
            {
                UpdateSlider(token);
            } else if (type == (int)PickupType.Ammo)
            {
                UpdateAmmo();
            }
        }

        readonly WaitForSecondsEnumerator  _waitForSeconds = new WaitForSecondsEnumerator(5);
        readonly ITime                     _time;
        GunEntityView                      _playerGunEntityView;
    }
}

