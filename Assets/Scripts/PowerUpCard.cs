using UnityEngine;
using UnityEngine.UI;

public class PowerUpCard : MonoBehaviour
{
    public Image icon;   
    public PowerUp powerUp;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Setup(PowerUp p)
    {
        powerUp = p;
        icon.sprite = p.icon;   
    }

    public void OnClick()
    {
        PowerUpManager.Instance.ApplyPowerUp(powerUp);
    }

    public void SetInteractable(bool value)
    {
        if (button != null)
            button.interactable = value;
    }
}

