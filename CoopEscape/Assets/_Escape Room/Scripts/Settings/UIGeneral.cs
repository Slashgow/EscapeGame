using UnityEngine;
using UnityEngine.UI;

public class UIGeneral : MonoSingleton<UIGeneral>
{
    [SerializeField] private Button buttonQuit;

    private void Start()
    {
        buttonQuit.onClick.AddListener(Quit);
    }
    private void OnDisable()
    {
        buttonQuit.onClick.RemoveListener(Quit);
    }
    private void Quit()
    {
        GameManager.Instance.Quit();
    }
}
