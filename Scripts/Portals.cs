using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portals : MonoBehaviour
{
    public GameObject spawnPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            other.transform.position = spawnPoint.transform.position;
        }
    }
}
