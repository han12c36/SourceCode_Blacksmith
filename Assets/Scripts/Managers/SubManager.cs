using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SubManager : MonoBehaviour
{
    [SerializeField]
    private int id = -1;
    public int ID => id;
    
    public bool isActivated = false;

    public static int GetID<T>() where T : SubManager
    {
        if (Enum.TryParse(typeof(T).ToString(), out Enums.ID result)) return (int)result;
        else return -1;
    }

    public static int GetType(SubManager subManager)
    {
        Type type = subManager.GetType();
        if (Enum.TryParse(type?.ToString(), out Enums.ID result)) return (int)result;
        else return -1;
    }

    public static T Get<T>() where T : SubManager
    {
        int id = GetID<T>();
        if (id < 0) return null;
        return (T)GameManager.subManagers[id];
    }

    public static void RegisterManager(GameObject parent)
    {
        SubManager[] originPrefabs = Resources.LoadAll<SubManager>(Paths.SubManagerFilePath) as SubManager[];

        GameManager.subManagers = new List<SubManager>();
        GameManager.ActivatedsubManagers = new List<SubManager>();

        for (int i = 0; i < originPrefabs.Length; i++)
        {
            GameManager.subManagers.Add(null);
            GameManager.ActivatedsubManagers.Add(null);
        }

        for (int i = 0; i < originPrefabs.Length; i++)
        {
            int grantedID = GetType(originPrefabs[i]);

            if(grantedID < 0) continue;

            SubManager subManager = Instantiate<SubManager>(originPrefabs[i]);
            subManager?.transform.SetParent(parent.transform);
            subManager.id = grantedID;
            GameManager.subManagers[subManager.id] = subManager;
        }

        foreach (SubManager manager in GameManager.subManagers)
            manager.SettingManagerForNextScene((int)GameManager.sceneCtrl.CurSceneIndex);
    }

    public void SetActivated(bool activated) 
    {
        SubManager subManager = GameManager.subManagers[id];
        subManager.isActivated = activated;
        List<SubManager> activatedSubManager = GameManager.ActivatedsubManagers;

        if (activated) { if (activatedSubManager[id] == null) activatedSubManager[id] = subManager; }
        else { if (activatedSubManager[id] != null) activatedSubManager[id] = null; }

    }

    public abstract void SettingManagerForNextScene(int nextSceneIndex);

    public virtual void SettingManagerForSceneCompleteLoaded(int nextSceneIndex) { }

    public virtual void ManagerCompleteLoaded() { }

    public virtual void Awake() { ManagerInitailized(); }

    public abstract void ManagerInitailized();

    public virtual void ManagerUpdate() { }

    public virtual void ManagerFixedUpdate() { }

    public virtual void ManagerLateUpdate() { }

}