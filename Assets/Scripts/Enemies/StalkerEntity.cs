using UnityEngine;
using System.Collections;

public class StalkerEntity : MonoBehaviour
{
    [Header("Settings")]
    public float appearDuration = 3f;
    public float timeBetweenAppearances = 20f;
    public float distanceToPlayer = 8f;
    
    private Transform playerTransform;

    private void Start()
    {
        // Hide initially until it's time to stalk
        SetVisible(false);
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            StartCoroutine(StalkerRoutine());
        }
    }

    private IEnumerator StalkerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenAppearances);

            if (playerTransform != null)
            {
                // Teleport to a position near the player
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                transform.position = (Vector2)playerTransform.position + (randomDir * distanceToPlayer);
                
                SetVisible(true);

                yield return new WaitForSeconds(appearDuration);
                
                SetVisible(false);
            }
        }
    }

    private void SetVisible(bool isVisible)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = isVisible;
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = isVisible;
    }
}
