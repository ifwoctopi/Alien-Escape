using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace AstroPlayer.FinalCharacterController
{

    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        [SerializeField] private bool holdToSprint = true;
        [SerializeField] private bool holdToCollect = true;
        
        public bool SprintToggledOn { get; private set; }
        public bool CollectToggledOn { get; private set; }
        public PlayerControls PlayerControls {get; private set;}
        
        public Vector2 MovementInput {get; private set;}
        public Vector2 LookInput {get; private set;}
        public bool JumpPressed { get; private set; }
        public bool CollectPressed { get; private set; }
        public bool Punch1Pressed { get; private set; }
        public bool Punch2Pressed { get; private set; }
        public bool KickPressed { get; private set; }

        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
            
            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            PlayerControls.PlayerLocomotionMap.Disable();
            PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }

        private void LateUpdate()
        {
            JumpPressed = false;
            CollectPressed = false;
            Punch1Pressed = false;
            Punch2Pressed = false;
            KickPressed = false;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
            print(MovementInput);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnToggleSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SprintToggledOn = holdToSprint || !SprintToggledOn;
               // print(SprintToggledOn);
            }
            else if (context.canceled)
            {
                SprintToggledOn = !holdToSprint && SprintToggledOn;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            JumpPressed = true;
        }

        public void OnCollect(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CollectToggledOn = holdToCollect || !CollectToggledOn;
                // print(SprintToggledOn);
            }
            else if (context.canceled)
            {
                CollectToggledOn = !holdToCollect && CollectToggledOn;
            }
        }

        public void OnPunch1(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            Punch1Pressed = true;
        }

        public void OnPunch2(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            Punch2Pressed = true;
        }

        public void OnKick(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            KickPressed = true;
        }
    }
}
