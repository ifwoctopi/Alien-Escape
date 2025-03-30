using System;
using System.Collections;
using System.Collections.Generic;
using AstroPlayer.FinalCharacterController;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    
   public int NumberOfArtifacts{get; private set;}

   

   public void ArtifactCollected()
   {
      
           NumberOfArtifacts++;
       
   }
}
