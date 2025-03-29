using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroPlayer.FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera _playerCamera;
        
        [Header("Base Movement")]
        public float runAcceleration = 35f;
        public float runSpeed = 4f;
        public float sprintAcceleration = 50f;
        public float sprintSpeed = 7f;
        public float drag = 3f; // Reduced drag for smoother movement
        public float movingThreshold = .01f;
        public float gravity = 25f;
        public float jumpSpeed = 1.0f;
        
        [Header("Camera Settings")]
        public float lookSenseH = .1f;
        public float lookSenseV = .1f;
        public float lookLimitV = 89f;
        
        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;
        private Vector2 cameraRotation = Vector2.zero;
        private Vector2 playerTargetRotation = Vector2.zero;
        private Vector3 _currentVelocity = Vector3.zero; // Track velocity manually
        private float verticalVelocity = 0f;
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
        }

        private void Update()
        { 
            UpdateMovementState();
            HandleVerticalMovement();
            HandleLateralMovement();
        }

        private void UpdateMovementState()
        {
            bool isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;
            bool isMovingLaterally = IsMovingLaterally();
            bool isSprinting = _playerLocomotionInput.SprintToggledOn && isMovingLaterally;
            bool isGrounded = IsGrounded();
            
            PlayerMovementState lateralState = isSprinting ? PlayerMovementState.Sprinting 
                : isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;
            _playerState.SetPlayerMovementState(lateralState);

            if (!isGrounded && _currentVelocity.y >= 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
            }
            else if (!isGrounded && characterController.velocity.y <= 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Falling);
            }
        }

        private void HandleLateralMovement()
        {
            bool isSprinting = _playerState.CurrentMovementState == PlayerMovementState.Sprinting;
            bool isGrounded = _playerState.InGroundedState();
            
            float lateralAcceleration = isSprinting ? sprintAcceleration : runAcceleration;
            float clampLateralMagnitude = isSprinting ? sprintSpeed : runSpeed;
            
            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;
            
            Vector3 movementDelta = movementDirection * lateralAcceleration * Time.deltaTime;
            _currentVelocity += movementDelta;
            
            _currentVelocity *= (1f - drag * Time.deltaTime); // Apply drag smoothly
            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, clampLateralMagnitude);
            _currentVelocity.y += verticalVelocity;
            characterController.Move(_currentVelocity * Time.deltaTime);
            
            Debug.Log($"Velocity: {_currentVelocity}, MovementInput: {_playerLocomotionInput.MovementInput}");
        }

        private void HandleVerticalMovement()
        {
            bool isGrounded = _playerState.InGroundedState();

            if (isGrounded && verticalVelocity < 0)
                verticalVelocity = 0f;
            
            verticalVelocity -= gravity * Time.deltaTime;

            if (_playerLocomotionInput.JumpPressed && isGrounded)
            {
                verticalVelocity += Mathf.Sqrt(jumpSpeed * 3 * gravity);
            }
            
        }

        private void LateUpdate()
        {
            cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            cameraRotation.y = Mathf.Clamp(cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);
            
            playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;
            transform.rotation = Quaternion.Euler(0f, playerTargetRotation.x, 0f);
            _playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f);
        }
        
        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_currentVelocity.x, 0f, _currentVelocity.z);
            Debug.Log("Lateral Velocity Magnitude: " + lateralVelocity.magnitude);
            return lateralVelocity.magnitude > movingThreshold;
        }

        private bool IsGrounded()
        {
            return characterController.isGrounded;
        }
    }
}
