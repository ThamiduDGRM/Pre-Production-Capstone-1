using UnityEngine;

public class TelegraphIndicator : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float maxScale = 1.3f;

    private void Update()
    {
        float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * 0.2f;
        transform.localScale = new Vector3(scale, scale, scale);
    }
}

