using UnityEngine;
using System.Collections;
using Svelto.ECS.Example.Survive.Player.Special;

namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerSpecialImplementor : MonoBehaviour, 
        IImplementor, 
        IPlayerSpecialAtributesComponent, 
        IPlayerSpecialTriggerComponent, 
        IPlayerSpecialAnimationComponent
    {
        Animator anim;
        public float Range = 10;
        public float MaxForce = 10;
        public float Cooldown = 10;
        
        public float range { get { return Range; } }
        public float maxForce { get { return MaxForce; } }
        public float cooldown { get { return Cooldown; } }
        public float timer { get; set; }

        public DispatchOnSet<bool> triggered { get { return _triggered; } }
        DispatchOnSet<bool> _triggered;

        public string trigger { set { anim.SetTrigger(value); } }

        void Awake()
        {
            timer = cooldown;
            anim = GetComponent<Animator>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}