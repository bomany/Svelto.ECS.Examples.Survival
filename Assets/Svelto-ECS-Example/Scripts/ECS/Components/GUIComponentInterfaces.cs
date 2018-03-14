using UnityEngine;

namespace Svelto.ECS.Example.Survive.HUD
{
    public interface IDamageHUDComponent: IComponent
    {
        float speed { get; }
        Color flashColor { get; }
        Color imageColor { set; get;  }
    }

    public interface IHealthSliderComponent: IComponent
    {
        int value { set; }
    }

    public interface IScoreComponent: IComponent
    {
        int score { set; get; }
    }

    public interface IAmmoComponent: IComponent
    {
        int ammo { set; get; }
    }

    public interface IEnemyCountComponent : IComponent
    {
        int total { set; }
    }

    public interface IWaveWarningComponent : IComponent
    {
        int wave { set; }
        float flashSpeed { get; }
        Color flashColor { get; }
        Color textColor { set; get; }
    }
}

namespace Svelto.ECS.Example.Survive {
    public struct WaveInfo
    {
        public int wave;
        public int enemyCount;

        public WaveInfo(int wave, int enemyCount)
        {
            this.wave = wave;
            this.enemyCount = enemyCount;
        }
    }
}