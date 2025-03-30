using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

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
        
        [Header("Environment Settings")]
        [SerializeField] private LayerMask groundLayers;



        
        private PlayerLocomotionInput _playerLocomotionInput;
        public PlayerState _playerState;
        private Vector2 cameraRotation = Vector2.zero;
        private Vector2 playerTargetRotation = Vector2.zero;
        private bool _jumpedLastFrame = false;


        private Vector3 _currentVelocity = Vector3.zero; // Track velocity manually
        private float verticalVelocity = 0f;
        private float _antiBump;
        private int HP = 100;
        
        public Slider healthBar;
        public Animator animator; 
        public int NumberOfArtifacts{get; private set;}
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
            _antiBump = sprintSpeed;

        }

        private void Update()
        { 
            healthBar.value = HP;
            UpdateMovementState();
            HandleVerticalMovement();
            HandleLateralMovement();
            Fighting();
            ArtifactCollected();
            
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

            if ((!isGrounded || _jumpedLastFrame) && _currentVelocity.y >= 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
                _jumpedLastFrame = false;

            }
            else if ((!isGrounded || _jumpedLastFrame) && characterController.velocity.y <= 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Falling);
                _jumpedLastFrame = false;

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
            
            Vector3 movementDelta = movementDirection * lateralAcceleration;
            _currentVelocity += movementDelta;
            
            _currentVelocity *= (1f - drag * Time.deltaTime); // Apply drag smoothly
            _currentVelocity = Vector3.ClampMagnitude(_currentVelocity, clampLateralMagnitude);
            _currentVelocity.y += verticalVelocity;
            characterController.Move(_currentVelocity * Time.deltaTime);
            
            //Debug.Log($"Velocity: {_currentVelocity}, MovementInput: {_playerLocomotionInput.MovementInput}");
        }

        private void HandleVerticalMovement()
        {
            bool isGrounded = _playerState.InGroundedState();

            verticalVelocity -= gravity * Time.deltaTime;


            if (isGrounded && verticalVelocity < 0)
                verticalVelocity = -_antiBump;
            

            if (_playerLocomotionInput.JumpPressed && isGrounded)
            {
                verticalVelocity += _antiBump + Mathf.Sqrt(jumpSpeed * 3 * gravity);
                _jumpedLastFrame = true;
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
            //Debug.Log("Lateral Velocity Magnitude: " + lateralVelocity.magnitude);
            return lateralVelocity.magnitude > movingThreshold;
        }

        private bool IsGrounded()
        {
            bool grounded = _playerState.InGroundedState() ? IsGroundedWhileGrounded() : isGroundedWhileAirborne();
            
            return characterController.isGrounded;
        }



        private bool IsGroundedWhileGrounded()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - characterController.radius, transform.position.z);
            
            bool grounded = Physics.CheckSphere(spherePosition, characterController.radius, groundLayers, QueryTriggerInteraction.Ignore);

            return grounded;
        }

        private bool isGroundedWhileAirborne()
        {
            return characterController.isGrounded;
        }
        
        public void ArtifactCollected()
        {
            bool isCollecting = _playerLocomotionInput.CollectToggledOn;
            if (isCollecting)
            {
                //print("Collecting Artifact");
                _playerState.SetPlayerMovementState(PlayerMovementState.Collecting);
                print(_playerState.CurrentMovementState);
            }
        }

        private void Fighting()
        {
            bool isPunching1 = _playerLocomotionInput.Punch1Pressed;
            bool isPunching2 = _playerLocomotionInput.Punch2Pressed;
            bool isKicking = _playerLocomotionInput.KickPressed;

            if (isPunching1)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Punching1);
                
            }

            if (isPunching2)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Punching2);
            }

            if (isKicking)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Kicking);
            }
        }
        public void TakeDamage(int damage)
        {
            HP -= damage;
            healthBar.value = HP;
            if (HP <= 0)
            {
                //Play Death Animation
                animator.SetTrigger("die");
                GetComponent<Collider>().enabled = false;
            }
            else
            {
                //PlayGetHitAnimation
                animator.SetTrigger("gethit");
            
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Player Collided with {other.gameObject.name}"); 
            if (other.tag == "Robot" && _playerState.CurrentMovementState == PlayerMovementState.Punching1)
            {
                Debug.Log($"Player Dealt Damage with {other.gameObject.name}"); 
                
                    other.GetComponent<Robot>().TakeDamage(20);

            }
            if (other.tag == "Player")
            {
                if(_playerState.CurrentMovementState == PlayerMovementState.Punching1)
                    other.GetComponent<PlayerController>().TakeDamage(10);
                if(_playerState.CurrentMovementState == PlayerMovementState.Punching2)
                    other.GetComponent<PlayerController>().TakeDamage(20);
                if(_playerState.CurrentMovementState == PlayerMovementState.Kicking)
                    other.GetComponent<PlayerController>().TakeDamage(5);
            }
        }
    }
}
