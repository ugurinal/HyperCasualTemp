using UnityEngine;

namespace HyperCasualTemp.Player
{
    public class Wing : MonoBehaviour
    {
        // private LocalPlayer _player;
        //
        // private void Start()
        // {
        //     _player = GetComponent<LocalPlayer>();
        // }
        //
        // private void OnCollisionEnter(Collision other)
        // {
        //     if (other.transform.CompareTag("Ground")) return;
        //
        //     // it it is collided with obstacles that shrinks the wings
        //
        //     string collidedName = other.GetContact(0).thisCollider.name; // which part of wing is collided
        //     _player.WingShrinker(int.Parse(collidedName));
        //     Destroy(other.gameObject);
        // }
    }
}