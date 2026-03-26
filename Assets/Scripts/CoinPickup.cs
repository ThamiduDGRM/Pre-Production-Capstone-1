using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add to player currency here
            Debug.Log("Picked up coin!");
            CurrencyManager.Instance.AddCoins(1);
            Destroy(gameObject);
        }
    }
}

