using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;
using System;

public class MainCanvas : MonoBehaviour
{
    [Header("Clock")]
    public Image fill_Image;
    //public Clock clock;

    [Header("PlayerInfo")]
    public InterfaceUserInfo interfaceUserInfo;
    //public CharactorWindow charactorWindow;

    //private TimeManager timeManager;

    [Header("Shop")]
    //public SellCanvas sellCanvas;

    [Header("Inventory")]
    //public InventoryCanvas inventoryCanvas;

    Action act;

    public Button[] allButton;
    public KeyCode[] allKey;
    public Window[] allWindow;

    public void Awake()
    {
        //timeManager = SubManager<TimeManager>.Get();
        allButton = GetComponentsInChildren<Button>();
        allKey = new KeyCode[(int)KeyCode.Mouse6];
        for (int i = 0; i < (int)KeyCode.Mouse6; i++) allKey[i] = (KeyCode)i;
        allWindow = GetComponentsInChildren<Window>();
    }

    public void TryOpenSkipMessage()
    {
        act += Button_Skip_Yes;
        Window.Get<WarningMessage>().TryOpenWarningMessageBox(Constants.ChangeTimeMessage, act);
        act -= Button_Skip_Yes;
    }

    public void Button_Skip_Yes() 
    {
        //timeManager.SetTime();
        //timeManager.SetTime(timeManager.CurTimeName);

        Button_Skip_No();
    }
    public void Button_Skip_No()
    {
        WarningMessage message = Window.Get<WarningMessage>();

        message.gameObject.SetActive(false);
        message.warningText.text = Constants.DefaultWarningMessage;
    }

    //===========================================================================================
}
