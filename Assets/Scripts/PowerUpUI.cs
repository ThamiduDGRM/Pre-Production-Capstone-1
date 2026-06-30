using UnityEngine;

public class PowerUpUI : MonoBehaviour
{
    public static PowerUpUI Instance;

    public PowerUpCard[] cards;   // MUST be size 4 in Inspector

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayRandomCards(PowerUp[] all)
    {
        if (all.Length < 4)
        {
            Debug.LogError("Need at least 4 power-ups in the array!");
            return;
        }
     
        // Assign all 4 cards
        cards[0].Setup(all[0]);
        cards[1].Setup(all[1]);
        cards[2].Setup(all[2]);
        cards[3].Setup(all[3]);

        gameObject.SetActive(true);
    }
}

