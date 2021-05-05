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
#if UNITY_EDITOR
            Debug.Log("UNITY EDITOR!");
            _touchDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized *
                              200f;

#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("ANDROID!");
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
#endif
        }

        private void FixedUpdate()
        {
            _playerMovementController.Move(_touchDirection);
        }

        private void ComputeTouchDirection(Vector3 startPos, Vector3 endPos)
        {
            if (!(Mathf.Abs(Vector3.Distance(startPos, endPos)) > _touchSensitivity)) return;

            Vector3 temp = startPos - endPos;
            temp.z = temp.y;
            temp.y = 0f;

            _touchDirection = temp;
        }
    }
}