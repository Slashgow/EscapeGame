using UnityEngine;

public class Rotator : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }
    [SerializeField] private RotationAxis rotationAxis = RotationAxis.Y; // Default to Y-axis
    [SerializeField, Range(0f,100f)] private float rotationSpeed = 5f; // Rotation speed

    private Vector3 axis;
    private void Awake()
    {
        axis = GetRotationAxis();
    }
    void Update()
    {
        transform.Rotate(axis * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private Vector3 GetRotationAxis()
    {
        switch (rotationAxis)
        {
            case RotationAxis.X: return Vector3.right;
            case RotationAxis.Y: return Vector3.up;
            case RotationAxis.Z: return Vector3.forward;
            default: return Vector3.up;
        }
    }

}
