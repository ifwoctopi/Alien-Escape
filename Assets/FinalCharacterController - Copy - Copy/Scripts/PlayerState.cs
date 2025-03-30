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
                   CurrentMovementState == PlayerMovementState.Sprinting ||
                   CurrentMovementState == PlayerMovementState.Collecting ||
                   CurrentMovementState == PlayerMovementState.Punching1 ||
                   CurrentMovementState == PlayerMovementState.Punching2 ||
                   CurrentMovementState == PlayerMovementState.Kicking;

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
        Collecting = 6,
        Punching1 = 7,
        Punching2 = 8,
        Kicking = 9,
        Strafing = 10
            
    }
    
}

