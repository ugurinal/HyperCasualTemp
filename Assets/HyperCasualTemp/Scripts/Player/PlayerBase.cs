using System;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualTemp.Player
{
    public abstract class PlayerBase : MonoBehaviour
    {
        [SerializeField] protected GameObject[] _wings;
        [SerializeField] protected int _currentEnergy = 0;
        [SerializeField] protected int _maxEnergy = 0;

        [SerializeField] private float _timeToShrinkWing = 2f;
        [SerializeField] private float _timeLeft = 0f;

        private bool _isGrounded;
        private bool _isTouching;

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

        public int CurrentEnergy => _currentEnergy;

        private void Start()
        {
            _isGrounded = true;
            _timeLeft = _timeToShrinkWing;
            _currentEnergy = 1;
            _maxEnergy = _wings.Length;
        }

        private void Update()
        {
            if (_isGrounded || _currentEnergy <= 0 || !_isTouching) return;

            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _currentEnergy--;
                WingShrinker(_currentEnergy);
                _timeLeft = _timeToShrinkWing;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Platform"))
            {
                Debug.Log("This will work once");
                _isGrounded = true;
                _timeLeft = _timeToShrinkWing;

                GetComponent<Rigidbody>().velocity = Vector3.zero; // to prevent player rotate
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // to prevent player rotate

                return;
            }

            // it it is collided with obstacles that shrinks the wings
            string collidedName = other.GetContact(0).thisCollider.name; // which part of wing is collided
            WingShrinker(int.Parse(collidedName));
            Destroy(other.gameObject);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnergyDrink"))
            {
                if (_currentEnergy < _maxEnergy)
                {
                    Destroy(other.gameObject);
                    _currentEnergy++;
                    WingExtender();
                    Debug.Log("ENERGY DRINK!");
                }
                else
                {
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
            DisableAllWings();

            if (collidedWingNum == 0)
            {
                _currentEnergy = 0;
                Debug.Log("GAME OVER!");
                return;
            }


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
    } // playerbase
} // namespace
