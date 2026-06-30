using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 5;
    public float lifetime = 3f;

    // Assigned by PlayerController when spawning
    public Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move strictly left/right (flat 2D)
        transform.position += direction * speed * Time.deltaTime;
    }

    private void LateUpdate()
    {
        // Keep fireball flat (no 3D rotation)
        transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore player
        if (other.CompareTag("Player"))
            return;

        // Only damage enemies
        if (!other.CompareTag("Enemy"))
            return;

        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}



