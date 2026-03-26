using UnityEngine;

[CreateAssetMenu(menuName = "2.5DBeatEmUp/PowerUp")]
public class PowerUp : ScriptableObject
{
    public string powerUpName;
    public string description;
    public Sprite icon;

    public enum PowerUpType { MaxHealth, CoinMultiplier }
    public PowerUpType type;

    public float value; // how much it increases
}

