using HyperCasualTemp.Player;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _targetPlatform;
    [SerializeField] private bool _useTransform;
    [SerializeField] private Vector3 _targetDirection;

    [Header("Force")]
    [Space(7.5f)]
    [SerializeField] private float _baseForce = 30f;

    private Vector3 _finalTargetDirection;

    private void Start()
    {
        if (_useTransform)
        {
            _finalTargetDirection = (_targetPlatform.position - transform.position).normalized;
            _finalTargetDirection.y = _finalTargetDirection.z; // 45 degree
            _finalTargetDirection.z /= 1.4f;
        }
        else
        {
            _finalTargetDirection = _targetDirection;
        }
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

        other.GetComponent<Rigidbody>()
            .AddForce(_finalTargetDirection * _baseForce * playerEnergy, ForceMode.VelocityChange);
    }
}
