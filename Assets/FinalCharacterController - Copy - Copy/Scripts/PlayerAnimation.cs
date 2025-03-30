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
     private static int hit1Hash = Animator.StringToHash("hit1");
     private static int hit2Hash = Animator.StringToHash("hit2");
     private static int kick1Hash = Animator.StringToHash("kick1");
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
         bool isPunching1 = playerState.CurrentMovementState == PlayerMovementState.Punching1;
         bool isPunching2 = playerState.CurrentMovementState == PlayerMovementState.Punching2;
         bool isKicking = playerState.CurrentMovementState == PlayerMovementState.Kicking;


         animator.SetBool (isGroundedHash, isGrounded);
         animator.SetBool (isJumpingHash, isJumping);
         animator.SetBool(isFallingHash, isFalling);
         animator.SetBool(isCollectingHash, isCollecting);
         animator.SetBool(hit1Hash, isPunching1);
         animator.SetBool(hit2Hash, isPunching2);
         animator.SetBool(kick1Hash, isKicking);
         Vector2 inputTarget = isSprinting ? playerLocomotionInput.MovementInput * 1.5f : playerLocomotionInput.MovementInput;
         currentBlendInput = Vector3.Lerp(currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);
         animator.SetFloat(inputXHash, inputTarget.x);
         animator.SetFloat(inputYHash, inputTarget.y);
         
         
     }
    }
    
}

