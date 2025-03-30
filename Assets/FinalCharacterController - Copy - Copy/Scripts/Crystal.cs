using System;
using System.Collections;
using System.Collections.Generic;
using AstroPlayer.FinalCharacterController;
using UnityEngine;

public class Crystal : MonoBehaviour
{
   
   public bool crystalCollected { get; private set; }

   private void OnTriggerEnter(Collider other)
   {
      PlayerInventory playerInventory = other.gameObject.GetComponent<PlayerInventory>();
      PlayerState playerState = other.gameObject.GetComponent<PlayerState>();
      print(playerState.CurrentMovementState);
      if (playerInventory != null) //&& playerState.CurrentMovementState == PlayerMovementState.Collecting)
      {
         print("Crystal Collected");
         playerInventory.ArtifactCollected();
         crystalCollected = true;
         gameObject.SetActive(false);
         
      }
      while(crystalCollected)
         other.GetComponent<PlayerController>().TakeDamage(5);
   }
}
