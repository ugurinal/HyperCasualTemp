using System;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualTemp.Player
{
    public abstract class PlayerBase : MonoBehaviour
    {
        [Header("Energy And Wing")]
        [SerializeField] protected GameObject[] _wings;
        [SerializeField] protected int _currentEnergy = 0;
        [SerializeField] protected int _maxEnergy = 0;

        [SerializeField] private float _timeToShrinkWing = 2f;

        [SerializeField] private string _groundTag;

        public int CurrentEnergy => _currentEnergy;

        public bool IsGrounded
        {
            get => _isGrounded;
            set => _isGrounded = value;
        }

        public bool IsTouching
        {
            get => _isTouching;
            set => _isTouching = value;
        }


        private float _timeLeft = 0f;

        private bool _isGrounded = true;

        private bool _isTouching = false;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            // _isGrounded = true;
            // _currentEnergy = 1;

            _timeLeft = _timeToShrinkWing;
            _maxEnergy = _wings.Length;
        }

        private void Update()
        {
            HandlePlayerInAir();
        }

        private void HandlePlayerInAir()
        {
            // if the player is in the air, shrink wings
            if (_isGrounded || _currentEnergy <= 0 || !_isTouching) return;

            _timeLeft -= Time.deltaTime;

            if (_timeLeft <= 0)
            {
                _currentEnergy--;
                WingShrinker(_currentEnergy);
                _timeLeft = _timeToShrinkWing;
            }
        }

        /// <summary>
        /// If we are in the air and we touch the screen
        /// </summary>
        public void StartedTouching()
        {
            // if we are grounded or we touched the screen once then return
            // is_touching => just make sures that this method only works once
            if (_isTouching || _isGrounded) return;

            // if player is just launched from launcher and at the same time it is going up
            // then return because we don't want the player can move if its too early
            if (transform.position.y < 30f && _rigidbody.velocity.y > 0) return; // if it is going up

            // we are touching the screen
            _isTouching = true;

            // don't let the player fall down
            _rigidbody.drag = Mathf.Infinity;
        }

        /// <summary>
        /// If we are in the air and we stopped touching the screen
        /// </summary>
        public void StoppedTouching()
        {
            // if we are grounded or we touched the screen once then return
            // is_touching => just make sures that this method only works once
            if (!_isTouching || _isGrounded) return;

            // we are not touching screen
            _isTouching = false;

            // player goes down
            _rigidbody.velocity = new Vector3(0f, -7f, 0f);

            // let player fall down
            _rigidbody.drag = 0f;
        }

        private void OnCollisionEnter(Collision other)
        {
            // if collided with ground (platform)
            if (other.transform.CompareTag(_groundTag))
            {
                // we are grounded
                _isGrounded = true;

                // reset time
                _timeLeft = _timeToShrinkWing;

                //  if we are landing
                // reset player velocity to prevent player rotating
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                return;
            }

            if (other.transform.CompareTag("Obstacle"))
            {
                // it it is collided with obstacles that shrinks the wings
                string collidedName = other.GetContact(0).thisCollider.name; // which part of wing is collided
                WingShrinker(int.Parse(collidedName));
                Destroy(other.gameObject);

                return;
            }

            Debug.Log("SOMETHING IS WRONG!");
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.transform.CompareTag(_groundTag))
            {
                _isGrounded = false;
                return;
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnergyDrink"))
            {
                // if energy is not full then collect and extend wing
                if (_currentEnergy < _maxEnergy)
                {
                    _currentEnergy++;
                    Destroy(other.gameObject);
                    WingExtender();
                }
                else
                {
                    // energy is full
                    Debug.Log("FULL");
                }
            }
        }

        private void WingExtender()
        {
            DisableAllWings();
            _wings[_currentEnergy - 1].SetActive(true);
        }

        private void WingShrinker(int collidedWingNum)
        {
            // disable all of the wings just in case
            DisableAllWings();

            // if wing 0 is collided with obstacle then game is over
            if (collidedWingNum == 0)
            {
                _currentEnergy = 0;
                Debug.Log("GAME OVER!");

                // if we are in the air
                // let player fall down
                if (!_isGrounded)
                {
                    _rigidbody.drag = 0f;
                    _rigidbody.AddForce(Vector3.down * 30f, ForceMode.VelocityChange);
                }

                return;
            }

            // if another wing is collided then shrink
            _currentEnergy = collidedWingNum;
            _wings[_currentEnergy - 1].SetActive(true);
        }

        private void DisableAllWings()
        {
            foreach (var wing in _wings)
            {
                wing.SetActive(false);
            }
        }
    } // player base
} // namespace
