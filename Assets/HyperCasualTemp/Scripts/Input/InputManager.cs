using System;
using HyperCasualTemp.Player;
using UnityEngine;

namespace HyperCasualTemp.PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private MovementController _playerMovementController;

        [SerializeField] private float _touchSensitivity = 5f;

        [SerializeField] private Vector3 _startTouchPos;
        [SerializeField] private Vector3 _currentTouchPos;

        [SerializeField] private Vector3 _movementDirection;

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _startTouchPos = touch.position;
                        break;
                    case TouchPhase.Moved:
                        _currentTouchPos = touch.position;
                        if (IsInputValid(_currentTouchPos, _startTouchPos))
                        {
                            _startTouchPos = _currentTouchPos;
                            _currentTouchPos = touch.position;
                        }

                        break;
                    case TouchPhase.Stationary:
                        //_playerMovementController.Move(_movementDirection.normalized);
                        break;
                    case TouchPhase.Ended:
                        _movementDirection = Vector3.zero;
                        break;
                    case TouchPhase.Canceled:
                        _movementDirection = Vector3.zero;
                        break;
                    default:
                        Debug.Log("IN SWITCH!");
                        break;
                }
            }

            //_playerMovementController.Move(_movementDirection);
        }

        private void FixedUpdate()
        {
            _playerMovementController.Move(_movementDirection);
        }

        private bool IsInputValid(Vector3 startPos, Vector3 endPos)
        {
            if (!(Mathf.Abs(Vector3.Distance(startPos, endPos)) > _touchSensitivity)) return false;

            Vector3 temp = startPos - endPos;
            _movementDirection = new Vector3(temp.x, 0f, temp.y);
            return true;
        }
    }
}