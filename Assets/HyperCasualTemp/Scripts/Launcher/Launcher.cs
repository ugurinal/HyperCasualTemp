using HyperCasualTemp.Player;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _targetPlatform;

    [Header("Force")]
    [Space(7.5f)]
    [SerializeField] private float _baseForce = 30f;

    private Vector3 _finalTargetDirection;


    private void Start()
    {
        _finalTargetDirection = (_targetPlatform.position - transform.position).normalized;
        _finalTargetDirection.y = _finalTargetDirection.z * 1.75f; // 45 degree
        _finalTargetDirection.z /= 2f; // 1.75 ?
    }

    private void OnTriggerEnter(Collider other)
    {
        // if it is not player don't do anything
        if (!other.CompareTag("Player")) return;

        Rigidbody playerBody = other.GetComponent<Rigidbody>();

        // if player is null then something is wrong
        if (playerBody == null)
        {
            Debug.Log("Player is null!");
            return;
        }

        playerBody.AddForce(_finalTargetDirection * _baseForce, ForceMode.VelocityChange);
    } // launcher
} // namespace
