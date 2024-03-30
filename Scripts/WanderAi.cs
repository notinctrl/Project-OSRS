using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderAi : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    private bool isScriptEnabled = true; // Flag to track if the script is enabled

    void Update()
    {
        if (isScriptEnabled && !isWandering)
        {
            StartCoroutine(Wander());
        }
        else
        {
            StopCoroutine(Wander());
            isWandering = false;
            isRotatingLeft = false;
            isRotatingRight = false;
            isWalking = false;
        }

        if (isRotatingRight == true)
        {
            gameObject.GetComponent<Animator>().Play("Idle");
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }
        if (isRotatingLeft == true)
        {
            gameObject.GetComponent<Animator>().Play("Idle");
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
        if (isWalking == true)
        {
            gameObject.GetComponent<Animator>().Play("Walk");
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator Wander() // This is the coroutine version of Wander
    {
        Debug.Log("wandering");
        int rotTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 4);
        int rotateLorR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 5);
        int walkTime = Random.Range(1, 6);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        isWandering = false;
    }

    // Method to enable or disable the script
    public void SetScriptEnabled(bool enabled)
    {
        isScriptEnabled = enabled;
        enabled = enabled; // This will enable or disable the script
    }
}
