using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float lifetime = 3f;   // how long the arrow lasts if it misses
    public int damage = 1;        // optional: damage to player

    private void Start()
    {
        // Delete after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // OPTIONAL: apply damage here
            // other.GetComponent<PlayerHealth>().TakeDamage(damage);

            Destroy(gameObject); // delete arrow on hit
        }

        // If it hits the ground or walls
        if (other.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }
    }
}

