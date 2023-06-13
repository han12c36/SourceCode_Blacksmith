using UnityEngine;
using UnityEngine.UI;
using System;

public class WarningMessage : Window
{
    public Text warningText;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    public void TryOpenWarningMessageBox(string str, Action trueAction)
    {
        TryOpenWindow();

        warningText.text = str;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() => { trueAction?.Invoke();});
        noButton.onClick.AddListener(() => { this.gameObject.SetActive(false); });
    }
}
