using UnityEngine;

public class PingPongTranslation : MonoBehaviour
{
    public enum TranslationAxis { X, Y, Z }
    [SerializeField] private TranslationAxis translationAxis = TranslationAxis.X; // Default to X-axis
    [SerializeField, Range(0f,5f)] private float translationSpeed = 0.2f; // Speed of translation
    [SerializeField, Range(0f,5f)] float translationAmount = 1.5f; // Distance before reversing

    private Vector3 startPosition;
    private Vector3 axis;

    void Awake()
    {
        axis = GetTranslationAxis();
        startPosition = transform.position;
    }

    void Update()
    {
        float pingPongValue = Mathf.PingPong(Time.time * translationSpeed, translationAmount) - (translationAmount / 2f);
        transform.position = startPosition + axis * pingPongValue;
    }

    private Vector3 GetTranslationAxis()
    {
        switch (translationAxis)
        {
            case TranslationAxis.X: return Vector3.right;
            case TranslationAxis.Y: return Vector3.up;
            case TranslationAxis.Z: return Vector3.forward;
            default: return Vector3.right;
        }
    }

}
