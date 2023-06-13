using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCanvas : MonoBehaviour { }

public class CanvasManager : SubManager
{
    private Dictionary<string, CustomCanvas> canvasDic = new Dictionary<string, CustomCanvas>();

    public override void SettingManagerForNextScene(int nextSceneIndex)
    {
        if (!isActivated) SetActivated(true);

        //mainCanvas.TryOpenCurSceneUI(nextSceneIndex);
    }

    public T GetCanvas<T>(string key) where T : CustomCanvas
    {
        CustomCanvas canvas = canvasDic[key];
        if (canvas == null) return null;

        return (T)canvas;
    }

    private bool RegisterCanvas(string path, string InstanceName,string key)
    {
        CanvasGroup[] prefabs = Resources.LoadAll<CanvasGroup>(path) as CanvasGroup[];

        if (prefabs.Length <= 0) return false;

        GameObject box = PublicLibrary.CreateBox(InstanceName);
        box.transform.SetParent(gameObject.transform);
        foreach (CanvasGroup prefab in prefabs)
        {
            CanvasGroup canvas = Instantiate(prefab);
            canvas.gameObject.SetActive(false);
            canvas.transform.SetParent(box.transform);

            canvasDic.Add(key, canvas.GetComponent<CustomCanvas>());
        }


        return true;
    }

    public override void ManagerInitailized()
    {
        RegisterCanvas(Paths.MainCanvasPath,     Constants.MainCanvasName,     Constants.MainCanvasKey);
        RegisterCanvas(Paths.LoadingCanvasPath,  Constants.LoadingCanvasName,  Constants.LoadingCanvasKey);
        RegisterCanvas(Paths.FadeCanvasPath,     Constants.FadeCanvasName,     Constants.FadeCanvasKey);
        RegisterCanvas(Paths.TutorialCanvasPath, Constants.TutorialCanvasName, Constants.TutorialCanvasKey);
    }

    public override void ManagerUpdate()
    {
        base.ManagerUpdate();

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SystemCanvas.TryOpenSystemCanvas();
        //}
    }
}
