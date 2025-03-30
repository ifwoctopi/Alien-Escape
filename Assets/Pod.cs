using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Pod : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.gameObject.GetComponent<PlayerInventory>();
        Crystal crystal = other.gameObject.GetComponent<Crystal>();
        if (playerInventory != null && crystal.crystalCollected) //&& playerState.CurrentMovementState == PlayerMovementState.Collecting)
        {
            print("going home !");
            SceneManager.LoadSceneAsync("End Credits");
        }
    }
}
