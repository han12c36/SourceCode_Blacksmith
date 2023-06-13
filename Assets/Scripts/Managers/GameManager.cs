using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// Instance할당 우선순위
    /// 1. Hierarchy에 GameManager를 Instance로 지정
    /// 2. GameManagerPreFab 사본을 생성 후 이를 Instance로 지정
    /// 3. 위 상황이 모두 아닐 시 새로운 GameManager를 생성 후 이를 Instance로 지정
    /// </summary>
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

    /// <summary>
    /// 'subManagers' Hierarchy에 객체로 존재하는 SubManager를 담아두는 곳(활동 여부에 무관)
    /// 'ActivatedsubManagers' 활성화 된 SubManager를 담아두는 곳(실제 작동할 서브매니저들)
    /// </summary>
    public static List<SubManager> subManagers;
    public static List<SubManager> ActivatedsubManagers;

    /// <summary>
    /// SubManager단위가 아닌 GameManager에 붙어 기생하는 컴포넌트
    /// </summary>
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
