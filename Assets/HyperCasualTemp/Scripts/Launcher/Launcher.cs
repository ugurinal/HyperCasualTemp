using HyperCasualTemp.Player;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _force;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        //other.GetComponent<IMovementController>().CanMove(false); // disable character movement
        other.GetComponent<Rigidbody>().AddForce(_direction * _force, ForceMode.Impulse);
        //other.GetComponent<IMovementController>().CanMove(true); // disable character movement
        Debug.Log("Player entered launcher!");
    }
}