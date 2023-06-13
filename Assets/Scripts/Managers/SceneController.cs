using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enums;
using System;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private SceneIndex curScene;
    private bool isLoading;
    private float curLoading_i;
    private float loadingCanvasDurationTime = 2.0f;

    public SceneIndex CurSceneIndex => curScene;

    protected void Awake()
    {
        curScene = (SceneIndex)SceneManager.GetActiveScene().buildIndex;

        //UserInterfaces.ChangeUI(curScene, curScene);
    }

    public void LoadScene(int nextSceneIndex)
    {
        if ((int)curScene == nextSceneIndex || isLoading) return;
        GameManager.coroutineHelper.StopAllCoroutines();
        GameManager.coroutineHelper.StartCoroutine(LoadSceneProcess(FinishLoad, nextSceneIndex));
    }

    private void FinishLoad(LoadingCanvas loadingCanvas, SceneIndex nextSceneIndex)
    {
        curScene = nextSceneIndex;
        loadingCanvas.gameObject.SetActive(false);

        CanvasManager canvasManager = SubManager.Get<CanvasManager>();

        FadeCanvas fadeCanvas = canvasManager.GetCanvas<FadeCanvas>(Constants.FadeCanvasKey);

        GameManager.coroutineHelper.StartCoroutine(fadeCanvas.FadeOut(0.8f));

        isLoading = false;

        StartCoroutine(WaitForSceneCompletedChanged());
    }

    IEnumerator WaitForSceneCompletedChanged()
    {
        while((int)curScene != SceneManager.GetActiveScene().buildIndex)
        {
            yield return null;
        }

        foreach (SubManager manager in GameManager.subManagers)
            manager.SettingManagerForSceneCompleteLoaded((int)curScene);
    }

    IEnumerator LoadSceneProcess(Action<LoadingCanvas,SceneIndex> action,int nextSceneIndex)
    {
        CanvasManager canvasManager = SubManager.Get<CanvasManager>();
        FadeCanvas fadeCanvas = canvasManager.GetCanvas<FadeCanvas>(Constants.FadeCanvasKey);
        LoadingCanvas loadingCanvas = canvasManager.GetCanvas<LoadingCanvas>(Constants.LoadingCanvasKey);

        yield return GameManager.coroutineHelper.StartCoroutine(fadeCanvas.FadeIn(Constants.FadeTime));

        //loadingCanvas.Initialize();
        //loadingCanvas.Play(2.5f);

        isLoading = true;

        UserInterfaces.ChangeUI(curScene,(SceneIndex)nextSceneIndex);

        SettingManagerForNextScene(nextSceneIndex);

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0f;
        while (!operation.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (operation.progress < 0.9f)
            {
                curLoading_i = Mathf.Lerp(curLoading_i, operation.progress, timer);
                if (curLoading_i >= operation.progress) timer = 0f;
            }
            else
            {
                curLoading_i = Mathf.Lerp(curLoading_i, 1f, timer);

                if (curLoading_i == 1.0f && timer >= loadingCanvasDurationTime)
                {
                    curLoading_i = 0.0f;
                    operation.allowSceneActivation = true;
                    break;
                }
            }
            string percent = ((int)(curLoading_i * 100.0f)).ToString();
            loadingCanvas.LoadingText.text = Constants.Loading;
        }
        //loadingCanvas.Stop();

        action?.Invoke(loadingCanvas,(SceneIndex)nextSceneIndex);
    }

    private void SettingManagerForNextScene(int nextSceneIndex)
    {
        foreach (SubManager manager in GameManager.subManagers)
            manager.SettingManagerForNextScene(nextSceneIndex);
    }

    //==========================================================================================
    //LoadScene
    public void GoStage(SceneIndex sceneIndex) { GameManager.sceneCtrl.LoadScene((int)sceneIndex); }
    public void GoIntroScene() { GoStage(SceneIndex.Intro); }
    public void GoSmithyScene() { GoStage(SceneIndex.Smithy); }
    public void GoWorkShopScene() { GoStage(SceneIndex.Workshop); }
    public void GoMineScene() { GoStage(SceneIndex.Mine); }
    public void GoSellScene() { GoStage(SceneIndex.Sell); }

    //==========================================================================================
}