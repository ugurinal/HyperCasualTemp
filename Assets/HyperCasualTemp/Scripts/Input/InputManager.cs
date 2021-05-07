using HyperCasualTemp.Player;
using UnityEngine;

namespace HyperCasualTemp.PlayerInput
{
    // enum TouchDirection
    // {
    //     None,
    //     LeftUp,
    //     Up,
    //     RightUp,
    //     Left,
    //     Right,
    //     LeftDown,
    //     Down,
    //     RightDown
    // }

    public class InputManager : MonoBehaviour
    {
        [Header("Player To Control")]
        [SerializeField] private GameObject _player;

        [Header("Touch Settings")]
        [Space(7.5f)]
        [SerializeField] private InputSettings _inputSettings;

        [Space(7.5f)]
        [SerializeField] private float _keyboardMovementSpeed = 600f;

        private Vector3 _startTouchPos;
        private Vector3 _currentTouchPos;
        private Vector3 _movementInput; // movement input

        private float _inputModifier; // to calculate input magnitude for all devices

        // [SerializeField] private TouchDirection _lastTouchDirection;
        // [SerializeField] private TouchDirection _currentTouchDirection;

        private IMovementController _playerMovementController;


        private void Awake()
        {
            _playerMovementController = _player.GetComponent<IMovementController>();

            _inputModifier = Screen.width / _inputSettings.TouchMagnitudeModifier;

            // _lastTouchDirection = TouchDirection.None;
            // _currentTouchDirection = TouchDirection.None;
        }

        private void Update()
        {
#if UNITY_EDITOR
            HandleKeyboardInput();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            HandleNewTouchInput();
#endif
        }

        private void FixedUpdate()
        {
            _playerMovementController.Move(_movementInput);
        }

        private void HandleKeyboardInput()
        {
            _movementInput =
                NormalizeInput(
                    new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized *
                    _keyboardMovementSpeed);
        }

        // private void HandleOldTouchInput()
        // {
        //     // todo update start touch input position
        //     if (Input.touchCount > 0)
        //     {
        //         Touch touch = Input.GetTouch(0);
        //
        //         switch (touch.phase)
        //         {
        //             case TouchPhase.Began:
        //                 _startTouchPos = touch.position;
        //                 _currentTouchPos = touch.position;
        //                 break;
        //             case TouchPhase.Moved:
        //                 _currentTouchPos = touch.position;
        //                 ComputeTouchInput(_currentTouchPos, _startTouchPos);
        //                 break;
        //             case TouchPhase.Stationary:
        //                 Debug.Log("STATIONARY!");
        //                 break;
        //             case TouchPhase.Ended:
        //                 _movementInput = Vector3.zero;
        //                 break;
        //             case TouchPhase.Canceled:
        //                 _movementInput = Vector3.zero;
        //                 break;
        //             default:
        //                 Debug.Log("DEFAULT INPUT!");
        //                 break;
        //         }
        //     }
        // }
#if UNITY_ANDROID && !UNITY_EDITOR
        private void HandleNewTouchInput()
        {
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
                        Debug.Log("MOVED!");
                        _currentTouchPos = touch.position;
                        ComputeTouchInput(_currentTouchPos, _startTouchPos);
                        break;
                    case TouchPhase.Stationary:
                        // update start touch position
                        //Debug.Log("STATIONARY!");
                        break;
                    case TouchPhase.Ended:
                        _movementInput = Vector3.zero;
                        break;
                    case TouchPhase.Canceled:
                        _movementInput = Vector3.zero;
                        break;
                    default:
                        Debug.Log("DEFAULT INPUT!");
                        break;
                }
            }
        }
#endif

        private void ComputeTouchInput(Vector3 startPos, Vector3 endPos)
        {
            if (!CheckSensitivity(startPos, endPos, _inputSettings.TouchSensitivity)) return;

            Vector3 temp = startPos - endPos;
            temp.z = temp.y;
            temp.y = 0f;

            _movementInput = NormalizeInput(temp);
        }

        private Vector3 NormalizeInput(Vector3 input)
        {
            float xDir = Mathf.Clamp(input.x / _inputModifier, -1, 1);
            float zDir = Mathf.Clamp(input.z / _inputModifier, -1, 1);

            // _lastTouchDirection = _currentTouchDirection;

            input.x = xDir;
            input.z = zDir;

            if (input.magnitude > 1.0)
                input.Normalize();

            return input;
        }

        private bool CheckSensitivity(Vector3 first, Vector3 second, float sensitivity)
        {
            return Mathf.Abs(Vector3.Distance(first, second)) > sensitivity;
        }

        // private bool CheckSensitivity(Vector2 first, float sensitivity)
        // {
        //     return first.magnitude > sensitivity;
        // }

        // private void CalculateTouchDirection(Vector2 touchDelta)
        // {
        //     if (!CheckSensitivity(touchDelta, 15)) return;
        //
        //     touchDelta.Normalize();
        //
        //     float xDir = touchDelta.x;
        //     float yDir = touchDelta.y;
        //
        //
        //     if (xDir < -0.33f) // LEFT
        //     {
        //         Debug.Log(yDir);
        //         if (yDir > 0.33f)
        //         {
        //             _currentTouchDirection = TouchDirection.LeftUp;
        //         }
        //         else if (yDir < -0.33)
        //         {
        //             _currentTouchDirection = TouchDirection.LeftDown;
        //         }
        //         else
        //         {
        //             Debug.Log("TEST!");
        //             _currentTouchDirection = TouchDirection.Left;
        //         }
        //     }
        //     else if (xDir > 0.33f) // RIGHT
        //     {
        //         if (yDir > 0.33f)
        //         {
        //             _currentTouchDirection = TouchDirection.RightUp;
        //         }
        //         else if (yDir < -0.33)
        //         {
        //             _currentTouchDirection = TouchDirection.RightDown;
        //         }
        //         else
        //         {
        //             _currentTouchDirection = TouchDirection.Right;
        //         }
        //     }
        //     else // MIDDLE
        //     {
        //         if (yDir > 0.33f)
        //         {
        //             _currentTouchDirection = TouchDirection.Up;
        //         }
        //         else if (yDir < 0.33f)
        //         {
        //             _currentTouchDirection = TouchDirection.Down;
        //         }
        //         else
        //         {
        //             Debug.Log("SOMETHING IS WRONG!!!!");
        //         }
        //     }
        // }
    }
}