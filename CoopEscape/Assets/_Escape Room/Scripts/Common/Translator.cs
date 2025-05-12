using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Translator : MonoBehaviour
{
    [SerializeField] private Axis translationAxis;
    [SerializeField, Range(-100f, 100f)] private float startAxisPosition;
    [SerializeField, Range(-100f, 100f)] private float stopAxisPosition;
    [SerializeField, Range(0f, 10f)] private float translationDuration;
    public UnityEvent OnEndTranslation;

    private Tween translationTween;

    private void Awake()
    {
        if (translationAxis == Axis.X)
            this.transform.localPosition = new Vector3(startAxisPosition, this.transform.localPosition.y, this.transform.localPosition.z);
        else if (translationAxis == Axis.Y)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, startAxisPosition, this.transform.localPosition.z);
        else if (translationAxis == Axis.Z)
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, startAxisPosition);
    }
    public void StopTranslation()
    {
        if(translationTween != null)
            translationTween.Kill();
    }
    public void Translate()
    {
        if (translationAxis == Axis.X)
        {
            if (translationTween != null)
                translationTween.Kill();

            translationTween = this.transform.DOLocalMove(new Vector3(stopAxisPosition, this.transform.localPosition.y, this.transform.localPosition.z),
                (Mathf.Abs(stopAxisPosition - this.transform.localPosition.x) * translationDuration) / Mathf.Abs(stopAxisPosition - startAxisPosition))
                .OnComplete(() => OnEndTranslation?.Invoke());
        }
        else if (translationAxis == Axis.Y)
        {
            if (translationTween != null)
                translationTween.Kill();

            translationTween = this.transform.DOLocalMove(new Vector3(this.transform.localPosition.x, stopAxisPosition, this.transform.localPosition.z), 
                (Mathf.Abs(stopAxisPosition - this.transform.localPosition.y) * translationDuration) / Mathf.Abs(stopAxisPosition - startAxisPosition))
                .OnComplete(() => OnEndTranslation?.Invoke()); ;
        }

        else if (translationAxis == Axis.Z)
        {
            if (translationTween != null)
                translationTween.Kill();

            translationTween = this.transform.DOLocalMove(new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, stopAxisPosition),
                (Mathf.Abs(stopAxisPosition - this.transform.localPosition.z) * translationDuration) / Mathf.Abs(stopAxisPosition - startAxisPosition))
                .OnComplete(() => OnEndTranslation?.Invoke()); ;
        }
    }

    public void ReverseTranslate()
    {
        if (translationAxis == Axis.X)
        {
            if (translationTween != null)
                translationTween.Kill();

            translationTween = this.transform.DOLocalMove(new Vector3(startAxisPosition, this.transform.localPosition.y, this.transform.localPosition.z),
                (Mathf.Abs(startAxisPosition - this.transform.localPosition.x) * translationDuration) / Mathf.Abs(stopAxisPosition - startAxisPosition))
                .OnComplete(() => OnEndTranslation?.Invoke());
        }
        else if (translationAxis == Axis.Y)
        {
            if (translationTween != null)
                translationTween.Kill();

            translationTween = this.transform.DOLocalMove(new Vector3(this.transform.localPosition.x, startAxisPosition, this.transform.localPosition.z),
                (Mathf.Abs(startAxisPosition - this.transform.localPosition.y) * translationDuration) / Mathf.Abs(stopAxisPosition - startAxisPosition))
                .OnComplete(() => OnEndTranslation?.Invoke()); ;
        }

        else if (translationAxis == Axis.Z)
        {
            if (translationTween != null)
                translationTween.Kill();

            translationTween = this.transform.DOLocalMove(new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, startAxisPosition),
                (Mathf.Abs(startAxisPosition - this.transform.localPosition.z) * translationDuration) / Mathf.Abs(stopAxisPosition - startAxisPosition))
                .OnComplete(() => OnEndTranslation?.Invoke()); ;
        }
    }


}
