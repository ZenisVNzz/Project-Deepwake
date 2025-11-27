using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;
    private float pollingTime = 1f;
    private float time;
    private int frameCount;

    void Update()
    {
        time += Time.unscaledDeltaTime;
        frameCount++;

        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fpsText.text = $"{frameRate} FPS";

            time -= pollingTime;
            frameCount = 0;
        }
    }
}
