using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameTimeController : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private int hour = 6;
    [SerializeField] private int minute = 0;
    [SerializeField] private float timeSpeed = 60f; 

    [Header("Lights")]
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private Light2D _sunLight;

    [Header("Objects With Shadow")]
    public List<ObjectShadowCast> ShadowCasters = new List<ObjectShadowCast>();

    private float timer;

    void Update()
    {
        TimeProcess();
        ShadowProcess();
        LightingProcess();
    }

    private void TimeProcess()
    {
        timer += Time.deltaTime * timeSpeed;

        if (timer >= 60f)
        {
            minute++;
            timer = 0f;

            if (minute >= 60)
            {
                minute = 0;
                hour++;
                if (hour >= 24)
                    hour = 0;
            }
        }
    }  
    
    private void ShadowProcess()
    {
        foreach (var shadowCaster in ShadowCasters)
        {
            shadowCaster.UpdateShadowTransparency(hour);
            shadowCaster.RotateByTime(hour, minute);
        }
    }

    private void LightingProcess()
    {
        float t = (hour + minute / 60f) / 24f;

        float sunAngle = t * 360f;
        if (_sunLight != null)
            _sunLight.transform.rotation = Quaternion.Euler(0f, 0f, sunAngle);

        // Ban ngày (6h -> 18h)
        if (hour >= 6 && hour <= 18)
        {
            float daylight = Mathf.InverseLerp(6f, 18f, hour + minute / 60f);

            if (_sunLight != null)
            {
                _sunLight.intensity = Mathf.Lerp(0f, 0.5f, Mathf.Sin(daylight * Mathf.PI));
            }

            if (_globalLight != null)
            {
                _globalLight.intensity = 0.7f;
            }
        }
        else if (hour > 18 && hour < 20) // Hoàng hôn (18h -> 20h)
        {
            float dusk = Mathf.InverseLerp(18f, 20f, hour + minute / 60f);
            if (_globalLight != null)
            {
                _globalLight.intensity = Mathf.Lerp(0.7f, 0.3f, dusk);
            }
        }
        else if (hour >= 20 || hour < 4) // Ban đêm (20h -> 6h)
        {
            if (_sunLight != null)
                _sunLight.intensity = 0f;

            if (_globalLight != null)
            {
                _globalLight.intensity = 0.3f;
            }
        }
        else if (hour >= 4 && hour < 6) // Bình minh (4h -> 6h)
        {
            float dawn = Mathf.InverseLerp(4f, 6f, hour + minute / 60f);
            if (_globalLight != null)
            {
                _globalLight.intensity = Mathf.Lerp(0.3f, 0.7f, dawn);
            }
        }
    }
}
