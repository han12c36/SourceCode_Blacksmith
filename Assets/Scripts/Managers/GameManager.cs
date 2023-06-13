using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enums;
using System.IO;

//
// 요약:
//     Removes a GameObject, component or asset.
//
// 매개 변수:
//   obj:
//     The object to destroy.
//
//   t:
//     The optional amount of time to delay before destroying the object.


public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType(typeof(GameManager)) as GameManager;

            if (instance == null)
            {
                GameManager originGameManager = Resources.Load<GameManager>(Paths.GameManagerFilePath);
                if (originGameManager) instance = Instantiate<GameManager>(originGameManager);
                else
                {
                    GameObject obj = PublicLibrary.CreateBox(Constants.GameManagerName);
                    GameManager newGameManager = obj.AddComponent<GameManager>();
                    instance = newGameManager;
                }
            }

            return instance;
        }
    }
    #endregion

    //순서 보장으로 들어온다!!

    //id값으로 정렬해서 담아둘 곳

    public static List<SubManager> subManagers;
    //IsActivated가 체크된다면 
    public static List<SubManager> ActivatedsubManagers;


    public static SceneController sceneCtrl;
    public static CoroutineHelper coroutineHelper;
    public static UserData userInfo = new UserData();

    public T_Protocol protocol;

    #region Attributes

    /// <summary>
    /// > Project Setting - Script Exceution Order - GameManager = 1 <
    /// [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    /// private static void FirstFunc() => GameManager.Instance.EmptyFunc();
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void FirstFunc() => GameManager.Instance.EmptyFunc();
    #endregion

    private void Awake()
    {
        if (sceneCtrl == null)       sceneCtrl       = gameObject.AddComponent<SceneController>();
        if (coroutineHelper == null) coroutineHelper = gameObject.AddComponent<CoroutineHelper>();

        RegistrationTo_DontDestroyManagerBox(gameObject, Constants.DontDestroyManagerBoxName);

        SubManager.RegisterManager(this.gameObject);
        
        //if (!GameManager.Instance.protocol.isTutorialPlayed) { GameManager.Instance.protocol.OnTutorial(); }
        //protocol.isTutorialPlayed = true;
    }

    private void RegistrationTo_DontDestroyManagerBox(GameObject obj, string parentBoxName)
    {
        GameObject box = PublicLibrary.CreateBox(parentBoxName);
        DontDestroyOnLoad(box);
        obj.transform.SetParent(box.transform);
    }

    private void EmptyFunc() { }
    
    private void FixedUpdate() { foreach (SubManager manager in ActivatedsubManagers) manager?.ManagerFixedUpdate(); }
    private void Update()      { foreach (SubManager manager in ActivatedsubManagers) manager?.ManagerUpdate(); }
    private void LateUpdate()  { foreach (SubManager manager in ActivatedsubManagers) manager?.ManagerLateUpdate(); }

    private void OnDisable() { }
}
