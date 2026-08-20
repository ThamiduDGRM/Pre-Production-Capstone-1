using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float fallSpeed = 20f;
    public int damage = 1;

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}

