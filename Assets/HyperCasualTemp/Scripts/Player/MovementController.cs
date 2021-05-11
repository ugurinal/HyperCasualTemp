using UnityEngine;

namespace HyperCasualTemp.Player
{
    public class MovementController : MonoBehaviour, IMovementController
    {
        [Header("Player Settings")]
        [SerializeField] private PlayerSettings _playerSettings;

        // [SerializeField] private bool _canMove = true;

        // public bool CanMove
        // {
        //     get => _canMove;
        //     set => _canMove = value;
        // }

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 input)
        {
            // if (!_canMove) return;

            // rotate player
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, input,
                Time.deltaTime * _playerSettings.RotationSpeed, 0f);
            _rigidbody.MoveRotation(Quaternion.LookRotation(desiredForward));

            // move player
            Vector3 moveDirection = input * (Time.deltaTime * _playerSettings.MovementSpeed);
            _rigidbody.MovePosition(_rigidbody.position + moveDirection);
        }

        // void IMovementController.CanMove(bool canMove)
        // {
        //     _canMove = canMove;
        // }
    } // movement controller
} // namespace
