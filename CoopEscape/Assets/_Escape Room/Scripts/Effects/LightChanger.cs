using DG.Tweening;
using UnityEngine;

public class LightChanger : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField, Range(0f,1f)] private float nightIntensity = 0.3f;
    [SerializeField, Range(2000f,20000f)] private float nightColorTemperature;
    [SerializeField, Range(0f, 5f)] private float timeToChangeIntensity;

    public void ChangeTemperature()
    {
        directionalLight.useColorTemperature = true;
        directionalLight.colorTemperature = nightColorTemperature;

        directionalLight.DOIntensity(nightIntensity, timeToChangeIntensity);
        
    }
}
