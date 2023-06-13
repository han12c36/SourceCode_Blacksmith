using System.Collections.Generic;
using UnityEngine;

public abstract class UserInterface : MonoBehaviour
{
    public static List<UserInterface> UIList = new List<UserInterface>();

    public void Button_GotoNextScene(Enums.SceneIndex nextScene) => GameManager.sceneCtrl.LoadScene((int)nextScene);

    /// <summary>
    /// 소량 데이터이기에 선형 탐색
    /// </summary>
    /// <typeparam name="T">UserInterface의 파생 클래스</typeparam>
    /// <returns></returns>
    public static T Get<T>() where T : UserInterface => (T)(object)UserInterface.UIList.Find(ui => ui is T);

    public static UserInterface FindByName(string UIName)
    {
        UIName += Constants.UI;
        foreach (UserInterface ui in UIList) { if (ui.name == UIName) return ui; }
        return null;
    }

    /// <summary>
    /// 생성시 참조하는 매니저들이 각각 존재할 수 있기에 매니저생성 이후 순서를 보장하기 위한 Func
    /// </summary>
    /// <param name="parent">Hierarchy의 ParentObject</param>
    public static void RegisterUserInterface(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            UserInterface ui = parent.transform.GetChild(i).GetComponent<UserInterface>();
            if(ui != null)
            {
                if (GameManager.sceneCtrl.CurSceneIndex.ToString() + Constants.UI == ui.name)
                    ui.gameObject.SetActive(true);
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

            if(uiName == Constants.Common_UI) UIList[i].gameObject.SetActive(true);
            else
            {
                if (uiName == sceneName + Constants.UI) UIList[i].gameObject.SetActive(true);
                else UIList[i].gameObject.SetActive(false);
            }
        }
    }
}