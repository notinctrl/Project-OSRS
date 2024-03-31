using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public GameObject toDestroy;
    void OnTriggerEnter(Collider other){
        if (other.CompareTag("Giant Key") || other.CompareTag("Dragon Key"))
        {
            AudioManager.instance.PlayUnlockSound();
            Destroy(toDestroy);
        }
    }
}
