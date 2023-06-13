using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enums;


public class UserInterfaces : MonoBehaviour
{
    private void Awake()
    {
        UserInterface.RegisterUserInterface(this.gameObject);
    }

    public static void ChangeUI(SceneIndex curScene ,SceneIndex nextScene)
    {
        UserInterface.FindByName(curScene.ToString()).gameObject.SetActive(false);
        UserInterface.FindByName(nextScene.ToString()).gameObject.SetActive(true);
    }
}
