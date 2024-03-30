using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToInstantiate; // Reference to the prefab you want to instantiate

    void Start()
    {
        // Instantiate the prefab at a specific position and rotation
        Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
    }
}