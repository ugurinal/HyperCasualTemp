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
            //NormalizeInput(ref input);

            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, input,
                Time.deltaTime * _playerSettings.RotationSpeed, 0f);
            _rigidbody.MoveRotation(Quaternion.LookRotation(desiredForward));

            Vector3 moveDirection = input * (Time.deltaTime * _playerSettings.MovementSpeed);
            _rigidbody.MovePosition(_rigidbody.position + moveDirection);
        }

        // private void NormalizeInput(ref Vector3 input)
        // {
        //     // todo update this function based on screen size
        //     int movementModifier = Mathf.RoundToInt(input.magnitude / 90.0f);
        //
        //     input = movementModifier switch
        //     {
        //         0 => input.normalized * _playerSettings._movementModifiers[0],
        //         1 => input.normalized * _playerSettings._movementModifiers[1],
        //         2 => input.normalized * _playerSettings._movementModifiers[2],
        //         3 => input.normalized * _playerSettings._movementModifiers[3],
        //         4 => input.normalized * _playerSettings._movementModifiers[4],
        //         _ => input.normalized * _playerSettings._movementModifiers[5]
        //     };
        // }
    }
}