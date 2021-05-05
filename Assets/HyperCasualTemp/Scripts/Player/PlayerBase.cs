using UnityEngine;

namespace HyperCasualTemp.Player
{
    public abstract class PlayerBase : MonoBehaviour
    {
        [SerializeField] protected GameObject[] _wings;
        [SerializeField] protected int _currentEnergy = 0;
        [SerializeField] protected int _maxEnergy = 0;

        private void Start()
        {
            _maxEnergy = _wings.Length - 1;
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.CompareTag("Ground")) return;

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

        protected void WingExtender()
        {
            _wings[_currentEnergy - 1].SetActive(false);
            _wings[_currentEnergy].SetActive(true);
        }

        public void WingShrinker(int collidedWingNum)
        {
            if (collidedWingNum == 0)
            {
                Debug.Log("GAME OVER!");
                //  deactivate all wings
                return;
            }

            for (int i = collidedWingNum; i < _wings.Length; i++)
            {
                _wings[i].SetActive(false);
            }

            _currentEnergy = collidedWingNum - 1;
            _wings[_currentEnergy].SetActive(true);
        }
    } // playerbase
} // namespace