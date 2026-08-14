using UnityEngine;

public class PowerUpUI : MonoBehaviour
{
    public static PowerUpUI Instance;

    public PowerUpCard[] cards;
    private UIFader fader;

    private void Awake()
    {
        Instance = this;
        fader = GetComponent<UIFader>();

        // Enable cards ONLY when fade finishes
        fader.onFadeComplete = EnableCards;
    }

    // Called BEFORE fade starts
    public void PrepareCards(PowerUp[] all)
    {
        if (all.Length < cards.Length)
        {
            Debug.LogError("Not enough power-ups in array!");
            return;
        }

        // Assign card visuals
        for (int i = 0; i < cards.Length; i++)
            cards[i].Setup(all[i]);

        // Disable buttons BEFORE fade
        DisableCards();
    }

    private void DisableCards()
    {
        foreach (var card in cards)
            card.SetInteractable(false);
    }

    private void EnableCards()
    {
        foreach (var card in cards)
            card.SetInteractable(true);
    }
}




