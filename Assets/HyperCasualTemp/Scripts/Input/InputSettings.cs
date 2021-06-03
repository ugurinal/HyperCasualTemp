using UnityEngine;

namespace HyperCasualTemp.PlayerInput
{
    [CreateAssetMenu(menuName = "HyperCasualTemp/Input/InputSettings")]
    public class InputSettings : ScriptableObject
    {
        [Header("Touch Settings")] 
        public float Threshold = 5f;
        public float Radius = 30f;
    }
}