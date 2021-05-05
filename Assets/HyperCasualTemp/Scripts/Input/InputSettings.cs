using UnityEngine;

namespace HyperCasualTemp.PlayerInput
{
    [CreateAssetMenu(menuName = "HyperCasualTemp/Input/InputSettings")]
    public class InputSettings : ScriptableObject
    {
        public float TouchSensitivity = 50f;
        public float[] TouchModifiers;
    }
}