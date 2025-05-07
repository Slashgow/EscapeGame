using System;
using PurrNet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockNumber : Pin
{
    [SerializeField, Range(0, 9)] private int unlockNumber; 
    [SerializeField] private UnityEngine.UI.Button previousNumberButton, nextNumberButton;
    [SerializeField] private TextMeshProUGUI numberText;

    private static readonly int[] digits = { 0,1, 2,3, 4,5,6,7,8,9 };
    private int currentDigitIndex;
    public int CurrentNumber { get; private set; }
    public override bool IsUnlocked => CurrentNumber == unlockNumber;

    protected override void OnSpawned()
    {
        base.OnSpawned();
        //Debug.Log("Register event ");
        previousNumberButton.onClick.AddListener(UpdateToPreviousNumber);
        nextNumberButton.onClick.AddListener(UpdateToNextNumber);

        currentDigitIndex = 0;
        CurrentNumber = digits[currentDigitIndex];
        numberText.text = CurrentNumber.ToString();
    }

    private void OnDisable()
    {
        previousNumberButton.onClick.RemoveListener(UpdateToPreviousNumber);
        nextNumberButton.onClick.RemoveListener(UpdateToNextNumber);
    }

    [ObserversRpc]
    private void UpdateToNextNumber()
    {
        //Debug.Log("update to next number observer");
        currentDigitIndex++;
        CurrentNumber = digits[MathsUtility.Modulo(currentDigitIndex, digits.Length)];
        numberText.text = CurrentNumber.ToString();
    }

    [ObserversRpc]
    private void UpdateToPreviousNumber()
    {
        //Debug.Log("update to previous number observer");
        currentDigitIndex--;
        CurrentNumber = digits[MathsUtility.Modulo(currentDigitIndex, digits.Length)];
        numberText.text = CurrentNumber.ToString();
    }

}
