using UnityEngine;
using System.Collections;

public class FireSpread : MonoBehaviour
{
    [Header("Fire Spread Settings")]
    public GameObject firePrefab;
    public float spreadInterval = 10f;
    public float spreadRadius = 3f;
    public int maxSpreads = 3;

    [Header("Wall Blocking Settings")]
    public LayerMask wallLayer;

    private int spreadCount = 0;

    private void Start()
    {
        StartCoroutine(SpreadRoutine());
    }

    private IEnumerator SpreadRoutine()
    {
        while (spreadCount < maxSpreads)
        {
            yield return new WaitForSeconds(spreadInterval);

            if (this != null && gameObject.activeInHierarchy)
            {
                SpreadFire();
            }
        }
    }

    private void SpreadFire()
    {
        Vector3 randomOffset = new Vector3(
            
            Random.Range(-spreadRadius, spreadRadius),
            0f,
            Random.Range(-spreadRadius, spreadRadius)
            );

        Vector3 newFirePos = transform.position + randomOffset;

        // Optional: check for overlapping fires before spawning
        Collider[] colliders = Physics.OverlapSphere(newFirePos, 1f);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Fire"))
                return; // too close to another fire, skip spawn
        }

        // check for walls
        if (Physics.Linecast(transform.position, newFirePos, wallLayer))
        {
            // wall in the way stop spread
            Debug.DrawLine(transform.position, newFirePos, Color.red, 1f);
            return;
        }
        else
        {
            Debug.DrawLine(transform.position, newFirePos, Color.green, 1f);
        }

        Instantiate(firePrefab, newFirePos, Quaternion.identity);
        spreadCount++;
    }

}
