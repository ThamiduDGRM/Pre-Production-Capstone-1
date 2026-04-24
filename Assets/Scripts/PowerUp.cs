using UnityEngine;

[CreateAssetMenu(menuName = "2.5DBeatEmUp/PowerUp")]
public class PowerUp : ScriptableObject
{
    public string powerUpName;
    public string description;
    public Sprite icon;

    public enum PowerUpType { Heal, CoinMultiplier, Bomb }
    public PowerUpType type;

    public float value; // how much it increases
}

