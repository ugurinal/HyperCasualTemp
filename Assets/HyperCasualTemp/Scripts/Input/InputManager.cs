using HyperCasualTemp.Player;
using UnityEngine;

namespace HyperCasualTemp.PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        [Header("Player To Control")] [SerializeField]
        private GameObject _player;


        [Header("Touch Settings")] [Space(7.5f)] [SerializeField]
        private InputSettings _inputSettings;

#if UNITY_ANDROID && !UNITY_EDITOR
        private Vector3 _startTouchPos;
        private Vector3 _currentTouchPos;

#endif
        private Vector3 _touchInput; // movement input

        private Vector2 _screenSize; // to calculate input magnitude for all devices

        private IMovementController _playerMovementController;

        private void Awake()
        {
            _playerMovementController = _player.GetComponent<IMovementController>();
            _screenSize = new Vector2(Screen.width, Screen.height);
        }

        private void Update()
        {
#if UNITY_EDITOR
            _touchInput =
                NormalizeInput(
                    new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized * 500f);

#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount > 0)
            {
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
                        _touchInput = Vector3.zero;
                        break;
                    case TouchPhase.Canceled:
                        _touchInput = Vector3.zero;
                        break;
                    default:
                        Debug.Log("DEFAULT INPUT!");
                        break;
                }
            }
#endif
        }

        private void FixedUpdate()
        {
            _playerMovementController.Move(_touchInput);
        }

        private void ComputeTouchDirection(Vector3 startPos, Vector3 endPos)
        {
            if (!(Mathf.Abs(Vector3.Distance(startPos, endPos)) > _inputSettings.TouchSensitivity)) return;

            Vector3 temp = startPos - endPos;
            temp.z = temp.y;
            temp.y = 0f;

            _touchInput = NormalizeInput(temp);
        }

        private Vector3 NormalizeInput(Vector3 input)
        {
            int magnitudeModifier = Mathf.RoundToInt(_screenSize.y / 21.3f);
            int movementModifier = Mathf.RoundToInt(input.magnitude / magnitudeModifier);

            input = movementModifier switch
            {
                0 => input.normalized * _inputSettings.TouchModifiers[0],
                1 => input.normalized * _inputSettings.TouchModifiers[1],
                2 => input.normalized * _inputSettings.TouchModifiers[2],
                3 => input.normalized * _inputSettings.TouchModifiers[3],
                4 => input.normalized * _inputSettings.TouchModifiers[4],
                _ => input.normalized * _inputSettings.TouchModifiers[5]
            };

            return input;
        }
    }
}