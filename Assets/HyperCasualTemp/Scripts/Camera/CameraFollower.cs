using UnityEngine;

namespace HyperCasualTemp.PlayerCamera
{
    public class CameraFollower : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private CameraSettings _cameraSettings;

        [Header("Target")]
        [Space(10f)]
        [SerializeField] private Transform _target;


        private void FixedUpdate()
        {
            Vector3 targetPos = _target.position + _cameraSettings.Offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _cameraSettings.Speed);
        }
    }
}