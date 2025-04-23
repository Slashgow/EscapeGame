using UnityEngine;

public class LightChanger : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField, Range(2000f,20000f)] private float nightColorTemperature;

    public void ChangeTemperature()
    {
        directionalLight.useColorTemperature = true;
        directionalLight.colorTemperature = nightColorTemperature;
    }
}
