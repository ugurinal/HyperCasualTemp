using HyperCasualTemp.Player;
using UnityEngine;

namespace HyperCasualTemp.PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        [Header("Player To Control")] [SerializeField]
        private GameObject _player;

        [Header("Touch Settings")] [Space(7.5f)] [SerializeField]
        private float _touchSensitivity = 30f;

        [Header("Movement Modifiers")] [Space(7.5f)] [SerializeField]
        private float[] _movementModifiers;

        private Vector3 _startTouchPos;
        private Vector3 _currentTouchPos;
        private Vector3 _touchDirection; // movement direction

        private IMovementController _playerMovementController;

        private void Awake()
        {
            _playerMovementController = _player.GetComponent<IMovementController>();
        }

        private void Update()
        {
            if (Input.touchCount <= 0) return;

            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startTouchPos = touch.position;
                    _currentTouchPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    _currentTouchPos = touch.position;
                    ComputeTouchDirection(_currentTouchPos, _startTouchPos);
                    break;
                case TouchPhase.Stationary:
                    Debug.Log("STATIONARY!");
                    break;
                case TouchPhase.Ended:
                    _touchDirection = Vector3.zero;
                    break;
                case TouchPhase.Canceled:
                    _touchDirection = Vector3.zero;
                    break;
                default:
                    Debug.Log("DEFAULT INPUT!");
                    break;
            }
        }

        private void FixedUpdate()
        {
            _playerMovementController.Move(_touchDirection);
        }

        private void ComputeTouchDirection(Vector3 startPos, Vector3 endPos)
        {
            if (!(Mathf.Abs(Vector3.Distance(startPos, endPos)) > _touchSensitivity))
            {
                return;
            }

            Vector3 temp = startPos - endPos;
            temp.z = temp.y;
            temp.y = 0f;

            int movementModifier = Mathf.RoundToInt(temp.magnitude / 90.0f);

            temp = movementModifier switch
            {
                0 => temp.normalized * _movementModifiers[0],
                1 => temp.normalized * _movementModifiers[1],
                2 => temp.normalized * _movementModifiers[2],
                3 => temp.normalized * _movementModifiers[3],
                4 => temp.normalized * _movementModifiers[4],
                _ => temp.normalized * _movementModifiers[5]
            };

            _touchDirection = temp;
        }
    }
}