using UnityEngine;

namespace HyperCasualTemp.Player
{
    public interface IMovementController
    {
        void Move(Vector3 input);
        void CanMove(bool canMove);
    }
}