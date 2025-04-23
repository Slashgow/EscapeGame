using System;
using System.Collections.Generic;
using PurrNet;
using UnityEngine;
using UnityEngine.UI;

public class PinColor : Pin
{
    [SerializeField] private GameObject selectColorsParent;
    [SerializeField] private List<ButtonColor> wrongColorButtons = new List<ButtonColor>();
    [SerializeField] private ButtonColor goodColorButton;
    [SerializeField] private Button chooseColorButton;
    [SerializeField] private List<Button> otherChooseColorButtons;

    public override bool IsUnlocked => chooseColorButton.colors.normalColor == goodColorButton.Color;

    private bool isSelected;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        selectColorsParent.SetActive(false);

        chooseColorButton.onClick.AddListener(OnClikChooseColor);
        wrongColorButtons.ForEach(wrongColor => wrongColor.Button.onClick.AddListener(() => OnClickOnColor(wrongColor.Color)));
        goodColorButton.Button.onClick.AddListener(() => OnClickOnColor(goodColorButton.Color));
        otherChooseColorButtons.ForEach(button => button.onClick.AddListener(ClickOnOtherChooseColorButton));
    }

    private void OnDisable()
    {
        chooseColorButton.onClick.RemoveListener(OnClikChooseColor);
        wrongColorButtons.ForEach(wrongColor => wrongColor.Button.onClick.RemoveAllListeners());
        goodColorButton.Button.onClick.RemoveAllListeners();
        otherChooseColorButtons.ForEach(button => button.onClick.RemoveListener(ClickOnOtherChooseColorButton));
    }

    [ObserversRpc]
    private void OnClickOnColor(Color color)
    {
        if(!isSelected)
            return;

        selectColorsParent.SetActive(false);
        ChangeChooseColorNormalColor(color);
    }

    [ObserversRpc]
    private void OnClikChooseColor()
    {
        isSelected = true;
        selectColorsParent.SetActive(true);
    }

    [ObserversRpc]
    private void ClickOnOtherChooseColorButton()
    {
        isSelected = false;
    }

    private void ChangeChooseColorNormalColor(Color color)
    {
        var colors = chooseColorButton.colors;
        colors.normalColor = color;
        chooseColorButton.colors = colors;
    }

   
}
