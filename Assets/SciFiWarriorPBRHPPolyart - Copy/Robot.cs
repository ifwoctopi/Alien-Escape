using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    private int HP = 100;
    public Slider healthBar;
    public Animator animator;

    private void Update()
    {
        healthBar.value = HP;
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
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
}
