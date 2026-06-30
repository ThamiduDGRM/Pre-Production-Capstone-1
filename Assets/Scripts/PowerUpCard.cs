using UnityEngine;
using UnityEngine.UI;

public class PowerUpCard : MonoBehaviour
{
    public Image icon;   
    public PowerUp powerUp;

    public void Setup(PowerUp p)
    {
        powerUp = p;
        icon.sprite = p.icon;   
    }

    public void OnClick()
    {
        PowerUpManager.Instance.ApplyPowerUp(powerUp);
    }
}
