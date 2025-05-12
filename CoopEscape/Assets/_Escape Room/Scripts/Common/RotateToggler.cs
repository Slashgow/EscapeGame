using DG.Tweening;
using UnityEngine;
public enum Axis
{
    X, Y, Z
}
public class RotateToggler : MonoBehaviour
{
    [SerializeField] private Axis rotationAxis;
    [SerializeField, Range(0f,5f)] private float timeToSwitch = 0.7f;
    [SerializeField, Range(-360f, 360f)] private float startAngle;
    [SerializeField, Range(-360f, 360f)] private float endAngle;

    private bool isInDefaultPosition = true;
    private Tween rotateTween;
    private void Awake()
    {
        if (rotationAxis == Axis.X)
            this.transform.localEulerAngles = new Vector3(startAngle, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z);
        else if (rotationAxis == Axis.Y)
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, startAngle, this.transform.localEulerAngles.z);
        else if (rotationAxis == Axis.Z)
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, startAngle);
    }

    public void SwitchRotationPosition()
    {
        float angle = isInDefaultPosition ? endAngle : startAngle;
        Debug.Log(angle);

        Debug.Log("switch rotation");
        if(rotationAxis == Axis.X)
        {
            //Debug.Log("switch rotation x");
            //Debug.Log(this.transform.localEulerAngles.x);
            //Debug.Log(Mathf.Abs(this.transform.localEulerAngles.x - startAngle));
            //float angle = Mathf.Abs(this.transform.localEulerAngles.x - startAngle) <= 0.1f ? endAngle : startAngle;
          
                //Debug.Log(angle);
            if(rotateTween != null)
                rotateTween.Kill();
            
            rotateTween = this.transform.DOLocalRotate(new Vector3(angle, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z), timeToSwitch);
        }
        else if (rotationAxis == Axis.Y)
        {
            //float angle = Mathf.Abs(this.transform.localEulerAngles.y - startAngle) <= 0.1f ? endAngle : startAngle;
            
            if (rotateTween != null)
                rotateTween.Kill();

            rotateTween = this.transform.DOLocalRotate(new Vector3(this.transform.localEulerAngles.x, angle, this.transform.localEulerAngles.z), timeToSwitch);
        }
            
        else if (rotationAxis == Axis.Z)
        {
            //float angle = Mathf.Abs(this.transform.localEulerAngles.z - startAngle) <= 0.1f ? endAngle : startAngle;

            if (rotateTween != null)
                rotateTween.Kill();

            rotateTween = this.transform.DOLocalRotate( new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, angle), timeToSwitch);
        }

        isInDefaultPosition = !isInDefaultPosition;
    }

   
          
}
