using UnityEngine;
using System.Collections;

namespace Svelto.ECS.Example.Survive.Player.Special
{
    public interface IPlayerSpecialAtributesComponent
    {
        float range { get; }
        float maxForce { get; }
        float cooldown { get; }
        float timer { get; set; }
    }
}