using Svelto.ECS.Example.Survive.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
    public class AmmoHUDImplementor : MonoBehaviour, IImplementor, IAmmoComponent
    {
        public int ammo { get { return _ammo; } set { _ammo = value; _text.text = "Ammo: " + _ammo; } }

        void Awake()
        {
            _text = GetComponent<Text>();
            _ammo = 100;
        }

        int _ammo;
        Text _text;
    }
}
