using UnityEngine;

namespace HyperCasualTemp.Player
{
    [CreateAssetMenu(menuName = "HyperCasualTemp/Player/PlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        public float MovementSpeed = 3f;
        public float RotationSpeed = 5f;
        public float[] _movementModifiers;
    }
}