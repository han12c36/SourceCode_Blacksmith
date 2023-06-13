using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enums;

public interface IWindow { public bool TryOpenWindow(); public bool Activated { get; set; } }

public abstract class Window : MonoBehaviour, IWindow
{
    public WindowID id;
    [HideInInspector] public bool windowActivated = false;
    public bool optional;
    //public Animator animCtrl;

    public static List<Window> windowList = new List<Window>();

    public bool Activated { get { return windowActivated; } set { windowActivated = value; } }

    public static T Get<T>() where T : Window
    {
        return (T)(object)windowList.Find(window => window is T);
    }

    public virtual void Awake()
    {
        if (gameObject.activeSelf) TryOpenWindow();
    }

    public static void RegisterWindow(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Window window = parent.transform.GetChild(i).GetComponent<Window>();
            //window.animCtrl = window.GetComponent<Animator>();
            if (window != null) windowList.Add(window);
        }
    }

    /// <summary>
    /// 탐색데이터가 많지 않음. Dictionary를 사용할 만큼의 메모리 소비보단 단순 탐색
    /// </summary>
    /// <returns>Window</returns>

    public static Window GetWindow(WindowID id)
    {
        foreach (Window win in windowList) if (win.id == id) return win;
        return null;
    }

    public bool TryOpenWindow()
    {
        if (!windowActivated)
        {
            if (GameManager.sceneCtrl.CurSceneIndex == SceneIndex.Mine)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            gameObject.SetActive(true);

        }
        else
        {
            if (GameManager.sceneCtrl.CurSceneIndex == SceneIndex.Mine)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            gameObject.SetActive(false);

            //StartCoroutine(WindowClose(() => Close(), animCtrl, "Reduction"));
        }
        return gameObject.activeSelf;
    }
    private void Close() { gameObject.SetActive(false); }
    IEnumerator WindowClose(Action action, Animator animCtrl, string AnimationName, int layerIndex = 0)
    {
        if (animCtrl) animCtrl.SetTrigger(AnimationName);
        
        bool result = false;
        
        while (!result)
        {
            yield return null;
        
            if (animCtrl.GetCurrentAnimatorStateInfo(layerIndex).IsName(AnimationName))
            {
                //if (PublicAnimationLidrary.isCurrentAnimationOver(animCtrl, layerIndex, 1.0f))
                //{
                //    result = true;
                //}
            }
        }

        if (result) action?.Invoke();
    }

    public virtual void OnEnable() { if(optional) windowActivated = true; }
    public virtual void OnDisable() { if(optional) windowActivated = false; }
}