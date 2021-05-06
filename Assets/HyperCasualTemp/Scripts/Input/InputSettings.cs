using UnityEngine;

namespace HyperCasualTemp.PlayerInput
{
    [CreateAssetMenu(menuName = "HyperCasualTemp/Input/InputSettings")]
    public class InputSettings : ScriptableObject
    {
        [Header("Touch Settings")]
        public float TouchSensitivity = 50f;
        public float[] TouchModifiers;
        //old =  0.5 0.8 1.1 1.4 1.7 2.0
    }
}