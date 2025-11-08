using UnityEngine;

public class FireSpawner : MonoBehaviour
{
    [Header("Fire Prefab to Spawn")]
    public GameObject firePrefab;

    [Header("Spawn Area")]
    public Vector3 center = Vector3.zero;
    public Vector3 size = new Vector3(10f, 0f, 10f);

    [Header("Spawn Settings")]
    public int fireCount = 5;

    private void Start()
    {
        SpawnFires();
    }

    void SpawnFires()
    {
        for (int i =0; i < fireCount; i++)
        {
            Vector3 randomPos = GetRandomPosition();
            Instantiate(firePrefab, randomPos, Quaternion.identity);
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2)
            );
        return center + randomOffset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
