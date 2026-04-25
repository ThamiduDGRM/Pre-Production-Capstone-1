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
    // Make sure we have at least 3 power-ups
    if (all.Length < 3)
    {
        Debug.LogError("Need at least 3 power-ups in the array!");
        return;
    }

    // Assign all 3 cards directly
    cards[0].Setup(all[0]);
    cards[1].Setup(all[1]);
    cards[2].Setup(all[2]);
  }


}

