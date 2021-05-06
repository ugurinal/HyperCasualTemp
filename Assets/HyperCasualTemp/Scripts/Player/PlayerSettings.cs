using UnityEngine;

namespace HyperCasualTemp.Player
{
    [CreateAssetMenu(menuName = "HyperCasualTemp/Player/PlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        public float MovementSpeed = 5f;
        public float RotationSpeed = 7f;
    }
}