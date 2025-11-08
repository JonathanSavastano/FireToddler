using UnityEngine;

public class WaterCollisionHandler : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Fire"))
        {
            Destroy(other);
        }
    }
}
