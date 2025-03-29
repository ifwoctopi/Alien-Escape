using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      PlayerInventory playerInventory = other.gameObject.GetComponent<PlayerInventory>();

      if (playerInventory != null)
      {
         playerInventory.DiamondCollected();
         gameObject.SetActive(false);
      }
   }
}
