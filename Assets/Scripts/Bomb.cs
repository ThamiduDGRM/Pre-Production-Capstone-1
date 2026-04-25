using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float fuseTime = 1.2f;
    public float explosionRadius = 3f;
    public int damage = 20;
    public LayerMask damageLayerMask;

    private void Start()
    {
        StartCoroutine(FuseRoutine());
    }

    private IEnumerator FuseRoutine()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    private void Explode()
    {
        // REMOVE explosion VFX
        // (no Instantiate(explosionVFX) here)

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, damageLayerMask);

        foreach (Collider c in hits)
        {
            IDamageable dmg = c.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}



