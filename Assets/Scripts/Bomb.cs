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
         
    private void Start()
    {
        
        
    }
  
    private void Explode()
    {
               
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
                EnemyHealth eh = c.GetComponentInParent<EnemyHealth>();
                if (eh != null) eh.TakeDamage(damage);
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

