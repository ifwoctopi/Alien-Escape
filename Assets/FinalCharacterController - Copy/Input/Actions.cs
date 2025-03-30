using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AstroPlayer.FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class Actions : MonoBehaviour, PlayerControls.IPlayerActionMapActions
    {
        #region Class Variables
        public bool AttackPressed { get; private set; }

        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerInput _playerInput; // Unity's built-in PlayerInput component
        #endregion

        #region Startup
        private void Awake()
        {
            // Ensure PlayerLocomotionInput is attached and initialized
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerInput = GetComponent<PlayerInput>(); // Assumes PlayerInput is on the same GameObject
        }

        private void OnEnable()
        {
            if (_playerInput == null)
            {
                Debug.LogError("PlayerInput component is missing.");
                return;
            }

            // Enable input actions
            _playerInput.actions.Enable();
            _playerInput.actions["Attack"].performed += OnAttack;  // Bind "Attack" action
        }

        private void OnDisable()
        {
            if (_playerInput == null)
            {
                Debug.LogError("PlayerInput component is missing.");
                return;
            }

            // Disable input actions and remove callbacks
            _playerInput.actions.Disable();
            _playerInput.actions["Attack"].performed -= OnAttack; // Unbind "Attack" action
        }
        #endregion

        private void Update()
        {
            // Handle logic when the player is moving, or modify based on other input actions
        }

        public void SetAttackPressedFalse()
        {
            AttackPressed = false;
        }

        #region Input Callbacks
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AttackPressed = true;
            }
        }
        #endregion
    }
}
