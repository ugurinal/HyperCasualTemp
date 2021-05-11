using UnityEngine;

namespace HyperCasualTemp.PlayerCamera
{
    [CreateAssetMenu(menuName = "HyperCasualTemp/Camera/CameraSettings")]
    public class CameraSettings : ScriptableObject
    {
        public Vector3 Offset = Vector3.zero;
        public float Speed = 5f;
    } // camera settings
} // namespace
