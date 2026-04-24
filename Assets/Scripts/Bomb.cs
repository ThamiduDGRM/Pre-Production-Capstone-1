// Bomb.cs
using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    public float fuseTime = 1.2f;
    public float explosionRadius = 3f;
    public int damage = 20;
    public LayerMask damageLayerMask; // set to Enemy layer(s)

    [Header("VFX / SFX")]
    public GameObject explosionVFX; // optional particle prefab
    public AudioClip explosionSFX;
    public float explosionForce = 5f; // optional physics push

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FuseRoutine());
    }

    private IEnumerator FuseRoutine()
    {
        // Optional: play ticking sound or animate
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    private void Explode()
    {
        // Spawn VFX
        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }

        // Play SFX
        if (explosionSFX != null)
        {
            if (audioSource != null)
                audioSource.PlayOneShot(explosionSFX);
            else
                AudioSource.PlayClipAtPoint(explosionSFX, transform.position);
        }

        // Damage enemies in radius
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, damageLayerMask);
        foreach (Collider c in hits)
        {
            // Try IDamageable or specific enemy component
            IDamageable dmg = c.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
            else
            {
                // If your enemies use a concrete script, call it here:
                // EnemyHealth eh = c.GetComponentInParent<EnemyHealth>();
                // if (eh != null) eh.TakeDamage(damage);
            }

            // Optional: add explosion force if enemy has rigidbody
            Rigidbody rb = c.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            }
        }

        // Destroy bomb object after short delay to allow SFX to play
        Destroy(gameObject, 0.1f);
    }

    // Visualize radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

