using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffect : MonoBehaviour
{
    private static CameraEffect instance;

    private CinemachineVirtualCamera _cam;
    private CinemachineBasicMultiChannelPerlin _noise;
    private Volume _volume;
    private ChromaticAberration _chromatic;
    private ColorAdjustments _colorAdjustments;
    private FilmGrain _grain;
    private List<float> shakeList;

    public static void Shake(float intensity, float duration)
    {
        instance.InstanceShake(intensity, duration);
    }

    public static void Chromatic(float intensity, float duration)
    {
        instance.InstanceChromatic(intensity, duration);
    }

    public static void Saturate(float value, float duration)
    {
        instance.InstanceSaturate(value, duration);
    }
    
    public static void Grain(float intensity, float duration)
    {
        instance.InstanceGrain(intensity, duration);
    }


    // Start is called before the first frame update
    void Awake()
    {
        _volume = FindObjectOfType<Volume>();
        _chromatic = (ChromaticAberration) _volume.profile.components.Find(x => x.name == "ChromaticAberration(Clone)");
        _colorAdjustments = (ColorAdjustments) _volume.profile.components.Find(x => x.name == "ColorAdjustments(Clone)");
        _grain = (FilmGrain) _volume.profile.components.Find(x => x.name == "FilmGrain(Clone)");
        shakeList = new List<float>();
        Debug.Log("Set CamEffect");
        instance = this;
        _cam = GetComponent<CinemachineVirtualCamera>();
        _noise = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InstanceShake(float intensity, float duration)
    {
        _noise.m_AmplitudeGain += intensity;
        _noise.m_FrequencyGain += intensity;
        shakeList.Add(intensity);
        StartCoroutine(StopShake(intensity, duration));
    }

    public IEnumerator StopShake(float intensity, float duration)
    {

        yield return new WaitForSeconds(duration);

        _noise.m_AmplitudeGain -= intensity;
        _noise.m_FrequencyGain -= intensity;
        yield return null;

    }

    public void InstanceChromatic(float intensity, float duration)
    {
        _chromatic.intensity.value = intensity;
        StartCoroutine(StopChromatic(intensity, duration));
    }
    public IEnumerator StopChromatic(float intensity, float duration)
    {
        float originalDuration = duration;
        while (duration > 0)
        {
            float random = Random.Range(0.1f, duration);
            duration -= random;
            _chromatic.intensity.value = (Random.value - 0.5f) * 0.3f + intensity * (duration/originalDuration);
            yield return new WaitForSeconds(random);
            

        }
        _chromatic.intensity.value = 0;
        yield return null;

    }

    public void InstanceSaturate(float value, float duration)
    {
        StartCoroutine(SmoothSaturate(value, duration));
    }
    public IEnumerator SmoothSaturate(float value, float duration)
    {
        float originalValue = _colorAdjustments.saturation.value;
        float totalDuration = duration;
        while (duration > 0)
        {
            _colorAdjustments.saturation.value = originalValue + (value - originalValue) * (1 - duration / totalDuration);
            duration -= 0.1f;
            yield return new WaitForSeconds(0.1f);

        }
        _colorAdjustments.saturation.value = value;
        yield return null;

    }
    
    public void InstanceGrain(float intensity, float duration)
    {
        StartCoroutine(SmoothGrain(intensity, duration));
    }
    public IEnumerator SmoothGrain(float intensity, float duration)
    {
        float originalValue = _grain.intensity.value;
        float totalDuration = duration;
        while (duration > 0)
        {
            _grain.intensity.value = originalValue + (intensity - originalValue) * (1 - duration / totalDuration);
            duration -= 0.1f;
            yield return new WaitForSeconds(0.1f);

        }
        _grain.intensity.value = intensity;
        yield return null;

    }
}

