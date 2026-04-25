using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    public static PowerUpUI Instance;

    public PowerUpCard[] cards;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayRandomCards(PowerUp[] all)
    {
    // Only show 2 cards
        for (int i = 0; i < 3; i++)
        {
            PowerUp random = all[Random.Range(0, all.Length)];
            cards[0].Setup(all[0]);
            cards[1].Setup(all[1]);

        }
    }

}

