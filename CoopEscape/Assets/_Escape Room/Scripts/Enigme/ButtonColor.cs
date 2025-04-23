using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private Button button;
    public Color Color => color;
    public Button Button => button;

    private void Awake()
    {
        var colors = button.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        colors.highlightedColor = color;
        colors.pressedColor = color;
        button.colors = colors;
    }
}
