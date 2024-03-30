using UnityEngine;
using System.Collections;

public class CombatPrefabSwitcher : MonoBehaviour
{
    public GameObject[] prefabs; // Array of prefabs to switch between
    public float attackDuration = 1.0f; // Duration of attack action in seconds
    public float blockDuration = 1.0f; // Duration of block action in seconds
    public float deathDuration = 1.0f;
    public float idleDuration = 1.0f;
    public Transform povCamera;
    public int hitPoints = 3;
    // public GameObject deathSpawn;

    private Vector3 previousPosition;
    private bool isInCombat = false; // Flag to track if the NPC is in combat
    private float actionTimer = 0.0f; // Timer for tracking action duration
    private int currentPrefabIndex = 0; // Index of the current active prefab
    private bool blockActionInitiated = false; // Flag to track if the block action has been initiated
    // private WanderAi wanderAiScript; // Reference to the WanderAi script

    void Start()
    {
        // Initialize previousPosition with the initial position of the parent GameObject
        previousPosition = transform.position;
        // Disable all prefabs except the first one (idle)
        for (int i = 1; i < prefabs.Length; i++)
        {
            prefabs[i].SetActive(false);
        }
        // Subscribe to the OnEnterCombat event
        GetComponent<CombatTrigger>().inCombat += HandleEnterCombat;
    }

    // Method to handle entering combat
    private void HandleEnterCombat(bool inCombat)
    {
        isInCombat = inCombat;

        if (isInCombat)
        {
            // Unsubscribe from the event after the first invocation
            GetComponent<CombatTrigger>().inCombat -= HandleEnterCombat;
            Vector3 cameraPosition = povCamera.position;
            transform.LookAt(cameraPosition);
        }
    }

    void Update()
    {
        // If in combat, decrement action timer
        if (isInCombat)
        {
            actionTimer -= Time.deltaTime;

            if (actionTimer <= 0.0f && currentPrefabIndex != 0)
            {
                SwitchPrefab(0); // Switch to idle prefab
                blockActionInitiated = false; // Reset the block action flag
            }
            else if (actionTimer <= 0.0f)
            {
                SwitchPrefab(2);
                actionTimer = attackDuration;
            }
        }
        else
        {
            // Check if the position of the parent GameObject has changed
            if (transform.position != previousPosition)
            {
                // Parent GameObject is moving
                if (currentPrefabIndex != 1){
                    SwitchPrefab(1);
                }
            }
            else
            {
                // Parent GameObject is not moving
                SwitchPrefab(0);
            }

            // Update previousPosition for the next frame
            previousPosition = transform.position;
        }
    }

    // Method to switch to a specific prefab
    private void SwitchPrefab(int prefabIndex)
    {
        prefabs[currentPrefabIndex].SetActive(false);
        prefabs[prefabIndex].SetActive(true);
        currentPrefabIndex = prefabIndex;
    }

    // OnTriggerEnter is called when another collider enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is an axe
        if (other.CompareTag("Axe"))
        {
            HandleBlockTrigger(other);
        }
    }

    private void HandleBlockTrigger(Collider other)
    {
        if (other.CompareTag("Axe"))
        {
            SwitchPrefab(3);
            actionTimer = blockDuration;
            hitPoints--;

            if (hitPoints <= 0)
            {
                StopAllCoroutines();
                // Play death animation and schedule destruction
                HandleDeath();
            }
        }
    }

    
    private void HandleDeath()
    {
        // isInCombat = false;
        SwitchPrefab(4); // Play death animation
        StartCoroutine(DestroyAfterAnimation(deathDuration)); // Delay destruction
    }

    private IEnumerator DestroyAfterAnimation(float delay)
    {
        // yield return new WaitForSeconds(delay);
        float elapsedTime = 0.0f;

    while (elapsedTime < delay)
    {
        Debug.Log("Time remaining: " + (delay - elapsedTime) + " seconds");
        yield return new WaitForSeconds(1.0f); // Wait for 1 second
        elapsedTime += 1.0f; // Increment elapsed time by 1 second
    }

    Debug.Log("Animation finished after " + delay + " seconds");

        // Destroy all prefabs
        for (int i = 0; i < prefabs.Length; i++)
        {
            Destroy(prefabs[i]);
        }
        // Destroy the parent game object
        Destroy(gameObject);
    }
}
