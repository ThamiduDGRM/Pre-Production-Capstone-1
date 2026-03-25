using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(damage);
        }
    }
}

