using Svelto.ECS.Example.Survive.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
    public class EnemyCountHUDImplementor : MonoBehaviour, IImplementor, IEnemyCountComponent
    {
        public int total { set { _text.text = "Enemies: " + value; } }
        
        void Awake()
        {
            _text = GetComponent<Text>();
        }

        Text _text;
    }
}
