using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroPlayer.FinalCharacterController
{
    public class PlayerAnimation : MonoBehaviour
    {
     [SerializeField] private Animator animator;
     [SerializeField] private float locomotionBlendSpeed = 4f;
    
     
     private PlayerLocomotionInput playerLocomotionInput;
     private PlayerState playerState;

     private static int inputXHash = Animator.StringToHash("inputX");
     private static int inputYHash = Animator.StringToHash("inputY");
     private static int isGroundedHash = Animator.StringToHash("isGrounded");
     private static int isJumpingHash = Animator.StringToHash("isJumping");
     private static int isFallingHash = Animator.StringToHash("isFalling");
     private static int isCollectingHash = Animator.StringToHash("isCollecting");
     private Vector3 currentBlendInput = Vector3.zero;

     private void Awake()
     {
         playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
         playerState = GetComponent<PlayerState>();
     }

     private void Update()
     {
         UpdateAnimationState();
     }

     private void UpdateAnimationState()
     {
         bool isSprinting = playerState.CurrentMovementState == PlayerMovementState.Sprinting;
         bool isIdling = playerState.CurrentMovementState == PlayerMovementState.Idling;
         bool isRunning = playerState.CurrentMovementState == PlayerMovementState.Running;
         bool isJumping = playerState.CurrentMovementState == PlayerMovementState.Jumping;
         bool isFalling = playerState.CurrentMovementState == PlayerMovementState.Falling;
         bool isGrounded = playerState.InGroundedState();
         bool isCollecting = playerState.CurrentMovementState == PlayerMovementState.Collecting;
         animator.SetBool (isGroundedHash, isGrounded);
         animator.SetBool (isJumpingHash, isJumping);
         animator.SetBool(isFallingHash, isFalling);
         animator.SetBool(isCollectingHash, isCollecting);
         Vector2 inputTarget = isSprinting ? playerLocomotionInput.MovementInput * 1.5f : playerLocomotionInput.MovementInput;
         currentBlendInput = Vector3.Lerp(currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);
         animator.SetFloat(inputXHash, inputTarget.x);
         animator.SetFloat(inputYHash, inputTarget.y);
         
     }
    }
    
}

