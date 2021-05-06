using UnityEngine;

namespace HyperCasualTemp.PlayerInput
{
    [CreateAssetMenu(menuName = "HyperCasualTemp/Input/InputSettings")]
    public class InputSettings : ScriptableObject
    {
        [Header("Touch Settings")] public float TouchSensitivity = 50f;
        public float TouchMagnitudeModifier = 13f;
    }
}