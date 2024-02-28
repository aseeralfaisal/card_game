using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDeactivator : MonoBehaviour
{
    public GameObject deactivateThisAlso;
    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to deactivate the object after 5 seconds
        StartCoroutine(DeactivateObjectAfterDelay(5f));
    }

    IEnumerator DeactivateObjectAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Deactivate the game object
        gameObject.SetActive(false);
        deactivateThisAlso.SetActive(false);
    }
}

