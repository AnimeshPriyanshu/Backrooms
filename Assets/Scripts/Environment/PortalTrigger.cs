using UnityEngine;
using System.Collections;

public class PortalTrigger : MonoBehaviour
{
    [Header("Settings")]
    public float delayBeforeLoad = 2f; 

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            isTriggered = true;
            Debug.Log("Portal Reached. Moving to next section...");
            StartCoroutine(PortalSequence());
        }
    }

    private IEnumerator PortalSequence()
    {
        // Stalker appearance hook before level loads
        Debug.Log("Stalker watches you enter the portal...");
        
        // Wait for the animation to finish
        yield return new WaitForSeconds(delayBeforeLoad);
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NextLevel();
        }
        else
        {
            Debug.LogWarning("GameManager not found, can't load next level.");
        }
    }
}
