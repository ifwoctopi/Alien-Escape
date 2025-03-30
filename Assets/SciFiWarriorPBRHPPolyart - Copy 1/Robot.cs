using System;
using System.Collections;
using System.Collections.Generic;
using AstroPlayer.FinalCharacterController;
using UnityEngine;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;
    PlayerState playerState;

    private void Update()
    {
        healthBar.value = HP;
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
            animator.SetTrigger("damage");
            
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Robot Trigger Entered with {other.gameObject.name}"); 
        if (other.CompareTag("Player") && animator.GetBool("isAttacking"))
            other.GetComponent<PlayerController>().TakeDamage(5);
        
    }
}
