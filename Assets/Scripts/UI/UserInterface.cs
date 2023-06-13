using System.Collections.Generic;
using UnityEngine;

public abstract class UserInterface : MonoBehaviour
{
    public static List<UserInterface> UIList = new List<UserInterface>();

    public void Button_GotoNextScene(Enums.SceneIndex nextScene)
        => GameManager.sceneCtrl.LoadScene((int)nextScene);

    public static T Get<T>() where T : UserInterface
    {
        return (T)(object)UserInterface.UIList.Find(ui => ui is T);
    }

    public static UserInterface FindByName(string UIName)
    {
        UIName += "_UI";
        foreach (UserInterface ui in UIList) { if (ui.name == UIName) return ui; }
        return null;
    }

    public static void RegisterUserInterface(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            UserInterface ui = parent.transform.GetChild(i).GetComponent<UserInterface>();
            if(ui != null)
            {
                //if (GameManager.sceneCtrl.CurSceneIndex.ToString() + "_UI" == ui.name)
                //    ui.gameObject.SetActive(true);
                UIList.Add(ui);
            }
        }

        InitUI(GameManager.sceneCtrl.CurSceneIndex.ToString());
    }

    public static void InitUI(string sceneName)
    {
        for (int i = 0; i < UIList.Count; i++)
        {
            string uiName = UIList[i].gameObject.name;

            if(uiName == "Common_UI") UIList[i].gameObject.SetActive(true);
            else
            {
                if (uiName == sceneName + "_UI") UIList[i].gameObject.SetActive(true);
                else UIList[i].gameObject.SetActive(false);
            }
        }
    }
}