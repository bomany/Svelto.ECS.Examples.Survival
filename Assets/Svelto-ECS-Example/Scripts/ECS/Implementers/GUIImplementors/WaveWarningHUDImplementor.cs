using Svelto.ECS.Example.Survive.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
    public class WaveWarningHUDImplementor : MonoBehaviour, IImplementor, IWaveWarningComponent
    {
        public float FlashSpeed = 5f;
        public float flashSpeed { get { return FlashSpeed; } }

        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
        public Color flashColor { get { return flashColour; } }

        public int wave { set { _text.text = "Wave " + value; } }

        public Color textColor
        {
            get { return _text.color; }
            set { _text.color = value; }
        }

        void Awake()
        {
            _text = GetComponent<Text>();
        }

        Text _text;
    }
}
