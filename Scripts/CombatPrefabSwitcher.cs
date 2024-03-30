using UnityEngine;
using System.Collections;

public class CombatPrefabSwitcher : MonoBehaviour
{
    public GameObject[] prefabs; // Array of prefabs to switch between
    public float attackDuration = 1.0f; // Duration of attack action in seconds
    public float blockDuration = 1.0f; // Duration of block action in seconds
    public float deathDuration = 1.0f;
    public Transform povCamera;
    public int hitPoints = 3;
    // public GameObject deathSpawn;

    private Vector3 previousPosition;
    private bool isInCombat = false; // Flag to track if the NPC is in combat
    private bool firstHit = true;
    private float actionTimer = 0.0f; // Timer for tracking action duration
    private int currentPrefabIndex = 0; // Index of the current active prefab

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
            if (firstHit)
            {
                UnityEngine.AI.NavMeshAgent navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (navMeshAgent != null)
                {
                    navMeshAgent.enabled = false;
                }
                // Determine the position of the point of view camera
                Vector3 cameraPosition = povCamera.position;
                // Make the NPC look at the point of view camera
                transform.LookAt(cameraPosition);
                firstHit = false;
            }
            // Start with attack action
            SwitchPrefab(2);
            actionTimer = attackDuration;
        }
    }


    private void HandleDeath()
    {
        isInCombat = false;
        SwitchPrefab(4); // Play death animation
        Invoke("DestroyNPC", deathDuration); // Delay destruction
    }

    private void DestroyNPC()
    {
        // Destroy all prefabs
        for (int i = 0; i < prefabs.Length; i++)
        {
            Destroy(prefabs[i]);
        }
        // Destroy the parent game object
        Destroy(gameObject);
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
                // Play death animation and schedule destruction
                HandleDeath();
            }
        }
    }


    // Method to switch to a specific prefab
    private void SwitchPrefab(int prefabIndex)
    {
        // Deactivate the current prefab
        prefabs[currentPrefabIndex].SetActive(false);

        // Activate the new prefab
        prefabs[prefabIndex].SetActive(true);

        // Update the current prefab index
        currentPrefabIndex = prefabIndex;
    }

    void Update()
    {
        // If in combat, decrement action timer
        if (isInCombat)
        {
            actionTimer -= Time.deltaTime;

            // If action timer reaches zero, switch to idle prefab
            if (actionTimer <= 0.0f)
            {
                SwitchPrefab(0); // Switch to idle prefab
            }
        }
        else
        {
            // Check if the position of the parent GameObject has changed
            if (transform.position != previousPosition)
            {
                // Parent GameObject is moving
                prefabs[0].SetActive(false);
                prefabs[1].SetActive(true);
            }
            else
            {
                // Parent GameObject is not moving
                prefabs[1].SetActive(false);
                prefabs[0].SetActive(true);
            }

            // Update previousPosition for the next frame
            previousPosition = transform.position;
        }
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
}
