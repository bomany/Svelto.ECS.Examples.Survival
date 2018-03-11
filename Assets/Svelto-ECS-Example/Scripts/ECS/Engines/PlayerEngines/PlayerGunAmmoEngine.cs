using UnityEngine;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Player.Gun
{
    public class PlayerGunAmmoEngine : SingleEntityViewEngine<GunEntityView>
    {
        protected override void Add(GunEntityView entityView)
        {
            _playerGunEntityView = entityView;
            entityView.gunHitTargetComponent.targetHit.NotifyOnValueSet(DecrementAmmo);
        }

        protected override void Remove(GunEntityView entityView)
        {
            _playerGunEntityView = null;
        }

        void DecrementAmmo(int ID, bool targetHasBeenHit)
        {
            _playerGunEntityView.gunComponent.ammo--;
        }

        void IncrementAmmo(int ammount)
        {
            _playerGunEntityView.gunComponent.ammo += ammount;
        }


        GunEntityView _playerGunEntityView;
    }

}
