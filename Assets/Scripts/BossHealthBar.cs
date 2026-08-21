using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider slider;
    public BossHealth boss;

    private void Start()
    {
        if (boss != null)
        {
            slider.maxValue = boss.maxHealth;
            slider.value = boss.maxHealth;
        }
    }

    private void Update()
    {
        if (boss != null)
        {
            slider.value = boss.CurrentHealth;
        }
    }
}

