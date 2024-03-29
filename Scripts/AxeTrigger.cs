using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeTrigger : MonoBehaviour
{
    public GameObject spawnLog;
    private bool logSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        // Reset the flag when axe enters the trigger area
        if (other.CompareTag("Axe")){
            logSpawned = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if a log has not been spawned yet
        if (!logSpawned && other.CompareTag("Axe"))
        {
            // Spawn the log
            Instantiate(spawnLog, transform.position, Quaternion.identity);
            Debug.Log("Log produced");

            // Set the flag to true to indicate that a log has been spawned
            logSpawned = true;
        }
    }
}
