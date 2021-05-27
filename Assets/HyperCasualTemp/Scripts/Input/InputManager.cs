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
        [SerializeField] private GameObject _playerGameObject;

        [Header("Touch Settings")]
        [Space(7.5f)]
        [SerializeField] private InputSettings _inputSettings;

        [Space(7.5f)]
        [SerializeField] private float _keyboardMovementSpeed = 600f;

        [SerializeField] private Vector3 _startTouchPos;
        [SerializeField] private Vector3 _currentTouchPos;
        [SerializeField] private Vector3 _movementInput; // movement input

        private float _inputModifier; // to calculate input magnitude for all devices

        private PlayerBase _playerBase;
        private IMovementController _playerMovementController;

        private void Awake()
        {
            _playerBase = _playerGameObject.GetComponent<PlayerBase>();
            _playerMovementController = _playerGameObject.GetComponent<IMovementController>();

            _inputModifier = Screen.width / _inputSettings.ScreenModifier;
        }

        private void Update()
        {
// #if UNITY_EDITOR
//             HandleKeyboardInput();
// #endif

// #if UNITY_ANDROID && !UNITY_EDITOR
            HandleTouchInput();
// #endif
            // HandleMouseInput();
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
            if (_movementInput.magnitude > 0.5f)
            {
                _playerBase.StartedTouching();
            }
            else
            {
                _playerBase.StoppedTouching();
            }
        }


        private void HandleTouchInput()
        {
            //todo update start touch position
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _startTouchPos = touch.position;
                        _currentTouchPos = touch.position;
                        _playerBase.StartedTouching();
                        break;
                    case TouchPhase.Moved:
                        _currentTouchPos = touch.position;
                        ComputeTouchInput();
                        _playerBase.StartedTouching();
                        break;
                    case TouchPhase.Stationary:
                        _currentTouchPos = touch.position;
                        ComputeTouchInput();
                        _playerBase.StartedTouching();
                        break;
                    case TouchPhase.Ended:
                        _movementInput = Vector3.zero;
                        _playerBase.StoppedTouching();
                        break;
                    case TouchPhase.Canceled:
                        _movementInput = Vector3.zero;
                        _playerBase.StoppedTouching();
                        break;
                    default:
                        Debug.Log("DEFAULT INPUT!");
                        break;
                }
            }
        }

        private void ComputeTouchInput()
        {
            Vector3 moveDir = _currentTouchPos - _startTouchPos;

            if (moveDir.magnitude < _inputSettings.Threshold)
            {
                return;
            }

            if (moveDir.magnitude + _inputSettings.Threshold > _inputSettings.Radius)
            {
                _startTouchPos += moveDir.normalized * 5f;
            }

            // Vector3 temp = _currentTouchPos - _startTouchPos;
            moveDir.z = moveDir.y;
            moveDir.y = 0f;

            _movementInput = NormalizeInput(moveDir);
        }

        // private void ComputeTouchInput(Vector3 startPos, Vector3 endPos)
        // {
        //     if (!CheckSensitivity(startPos, endPos, _inputSettings.Threshold))
        //     {
        //         return;
        //     }
        //
        //     if ((startPos - endPos).magnitude > _inputSettings.Radius)
        //     {
        //     }
        //
        //     Vector3 temp = startPos - endPos;
        //     temp.z = temp.y;
        //     temp.y = 0f;
        //
        //     _movementInput = NormalizeInput(temp);
        // }
        //
        // private bool CheckSensitivity(Vector3 first, Vector3 second, float sensitivity)
        // {
        //     return Mathf.Abs(Vector3.Distance(first, second)) > sensitivity;
        // }

        private Vector3 NormalizeInput(Vector3 input)
        {
            float xDir = Mathf.Clamp(input.x / _inputModifier, -1, 1);
            float zDir = Mathf.Clamp(input.z / _inputModifier, -1, 1);

            input.x = xDir;
            input.z = zDir;

            if (input.magnitude > 1.0)
                input.Normalize();

            return input;
        }

        // private void HandleMouseInput()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         _startTouchPos = Input.mousePosition;
        //         _currentTouchPos = Input.mousePosition;
        //         _playerBase.IsTouching = true;
        //     }
        //     else if (Input.GetMouseButton(0))
        //     {
        //         _currentTouchPos = Input.mousePosition;
        //         ComputeTouchInput(_currentTouchPos, _startTouchPos);
        //     }
        //     else if (Input.GetMouseButtonUp(0))
        //     {
        //         _movementInput = Vector3.zero;
        //         _playerBase.IsTouching = false;
        //     }
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
    } // input manager
} // namespace
