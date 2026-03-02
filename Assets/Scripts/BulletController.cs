using UnityEngine;

public class BulletController : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object we hit has a ZombieController script
        if (collision.gameObject.GetComponent<ZombieController>() != null)
        {
            // Destroy the zombie
            Destroy(collision.gameObject);
            
            // Destroy the bullet itself
            Destroy(gameObject);
        }
    }
}
