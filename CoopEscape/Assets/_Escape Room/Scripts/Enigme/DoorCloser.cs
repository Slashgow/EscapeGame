using UnityEngine;

public class DoorCloser : MonoBehaviour
{
    [SerializeField] private PlayerDetector playerDetector;
    [SerializeField] private Translator translator;

    private void OnEnable()
    {
        playerDetector.OnLastPlayerExit.AddListener(OnLastPlayerExit);

        if(playerDetector.PlayerCount <= 0)
        {
            translator.ReverseTranslate();
        }
    }

    private void OnDisable()
    {
        playerDetector.OnLastPlayerExit.RemoveListener(OnLastPlayerExit);
    }

    private void OnLastPlayerExit() => translator.ReverseTranslate();
}
