using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AstroPlayer.FinalCharacterController
{
    public class PlayerState : MonoBehaviour
    {
        [field: SerializeField] public PlayerMovementState CurrentMovementState { get; private set; } = PlayerMovementState.Idling;

        public void SetPlayerMovementState(PlayerMovementState playerMovementState)
        {
            CurrentMovementState = playerMovementState;
        }

        public bool InGroundedState()
        {
            return CurrentMovementState == PlayerMovementState.Idling ||
                   CurrentMovementState == PlayerMovementState.Walking ||
                   CurrentMovementState == PlayerMovementState.Running ||
                   CurrentMovementState == PlayerMovementState.Sprinting;

        }
        
    }
    public enum PlayerMovementState
    {
        Idling = 0,
        Walking = 1,
        Running = 2,
        Sprinting = 3,
        Jumping = 4,
        Falling = 5,
        Strafing = 6,
            
    }
    
}

