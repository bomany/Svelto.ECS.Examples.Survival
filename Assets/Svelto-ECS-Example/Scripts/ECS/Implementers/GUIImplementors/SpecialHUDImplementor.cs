﻿using Svelto.ECS.Example.Survive.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
    public class SpecialHUDImplementor : MonoBehaviour, IImplementor, ISpecialHUDComponent
    {
        public float timer { get { return _timer; } set { _timer = value; _text.text = string.Format("{0:0.0}", _timer); } }
        public bool showTimer { set { _text.enabled = value; } }

        void Awake()
        {
            _text = GetComponent<Text>();
            _timer = 0;
        }

        float _timer;
        Text _text;
    }
}
