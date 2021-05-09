using HyperCasualTemp.Player;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Transform _targetPlatform;
    [SerializeField] private float _baseForce;

    private Vector3 _targetDirection;

    private void Start()
    {
        _targetDirection = (_targetPlatform.position - transform.position).normalized;
        _targetDirection.y = _targetDirection.z;    // 45
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerBase player = other.GetComponent<PlayerBase>();

        if (player == null)
        {
            Debug.Log("Player is null!");
            return;
        }

        float playerEnergy = player.CurrentEnergy;
        player.IsGrounded = false;

        other.GetComponent<Rigidbody>().AddForce(_targetDirection * _baseForce * playerEnergy, ForceMode.Acceleration);
    }
}