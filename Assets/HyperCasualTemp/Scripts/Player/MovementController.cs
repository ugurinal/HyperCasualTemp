using UnityEngine;

namespace HyperCasualTemp.Player
{
    public class MovementController : MonoBehaviour, IMovementController
    {
        [SerializeField] private PlayerSettings _playerSettings;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 input)
        {
            Vector3 desiredForward =
                Vector3.RotateTowards(transform.forward, input, Time.deltaTime * _playerSettings.RotationSpeed, 0f);
            _rigidbody.MoveRotation(Quaternion.LookRotation(desiredForward));

            Vector3 moveDirection = input * (Time.deltaTime * _playerSettings.MovementSpeed);
            _rigidbody.MovePosition(_rigidbody.position + moveDirection);
        }
    }
}