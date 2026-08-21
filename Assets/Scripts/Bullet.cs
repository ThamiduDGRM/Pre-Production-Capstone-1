using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;
    public float lifetime = 3f;

    public Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

   

    private void OnTriggerEnter(Collider other)
    {
        // ⭐ Guaranteed player detection (root + child)
        if (other.CompareTag("Player"))
        {
            // If your player has health:
            other.GetComponent<PlayerHealth>().TakeDamage(damage);

            Destroy(gameObject);
            
        }

        // ⭐ Delete on ground or environment
        
    }
}








