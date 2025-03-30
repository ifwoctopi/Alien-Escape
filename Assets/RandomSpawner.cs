using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform cam;
    public float spawnInterval = 5f; // Time between spawns
    private float nextSpawnTime = 0f; // Timer
    private float spawnCounter = 0f;
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene" && Time.time >= nextSpawnTime && spawnCounter <= 20)
        {
            nextSpawnTime = Time.time + spawnInterval; // Reset timer
            spawnCounter += 1;

            // Spawn near the spawner’s position
            Vector3 randomSpawnPos = transform.position + new Vector3(Random.Range(-100, 50), 10, Random.Range(-100, 100));

            // Adjust Y position to match terrain height
            RaycastHit hit;
            if (Physics.Raycast(randomSpawnPos, Vector3.down, out hit, Mathf.Infinity))
            {
                randomSpawnPos.y = hit.point.y; // Set Y to the hit point on terrain
            }

            // Instantiate the prefab
            GameObject newObject = Instantiate(prefab, randomSpawnPos, Quaternion.identity);

            // Assign the camera to the HealthBar script
            HeathBar healthBar = newObject.GetComponent<HeathBar>();
            if (healthBar != null)
            {
                healthBar.SetCamera(cam); // Ensure HealthBar has SetCamera()
            }
        }
    }

    }
