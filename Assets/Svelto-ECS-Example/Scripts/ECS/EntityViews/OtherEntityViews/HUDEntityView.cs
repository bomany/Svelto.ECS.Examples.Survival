namespace Svelto.ECS.Example.Survive.HUD
{
    public class HUDEntityView : EntityView
    {
        public IAnimationComponent      HUDAnimator;
        public IDamageHUDComponent      damageImageComponent;
        public IHealthSliderComponent   healthSliderComponent;
        public IScoreComponent          scoreComponent;
        public IAmmoComponent           ammoComponent;
        public ISpecialHUDComponent     specialComponent;

        public IEnemyCountComponent     enemyCountComponent;
        public IWaveWarningComponent    waveWarningComponent;
    }
}
