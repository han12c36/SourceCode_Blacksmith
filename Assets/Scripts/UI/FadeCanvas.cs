using System.Collections;
using UnityEngine;

public class FadeCanvas : CustomCanvas
{
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    [HideInInspector] public float fadeInTime = Constants.DefaultFadeInTime;
    [HideInInspector] public float fadeOutTime = Constants.DefaultFadeOutTime;
    [HideInInspector] public int sortingOrder = Constants.FadeCanvasSortingOrder;
    public IEnumerator fadeIn;
    public IEnumerator fadeOut;
    public IEnumerator fadeInOut;

    public CanvasGroup CanvasGroup { get { return canvasGroup; } }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();
        canvas.sortingOrder = sortingOrder;
        fadeIn = FadeIn(fadeInTime);
        fadeOut = FadeOut(fadeOutTime);
        fadeInOut = FadeInOut(fadeInTime, fadeOutTime);
    }

    private void Start()
    {
        StartCoroutine(fadeOut);
    }

    //음향 : 점점 작아지고 화면 : 점점 검게 변함
    public IEnumerator FadeOut(float FadeTime)
    {
        canvas.sortingOrder = 2;
        canvasGroup.alpha = 1.0f;

        while (canvasGroup.alpha > 0.0f)
        {
            yield return null;
            canvasGroup.alpha -= Time.deltaTime / FadeTime;
        }

        canvas.sortingOrder = 0;
        canvasGroup.alpha = 0.0f;
    }

    //음향 : 점점 커지고 화면 : 점점 밝게 변함
    public IEnumerator FadeIn(float FadeTime)
    {
        canvas.sortingOrder = 2;
        canvasGroup.alpha = 0.0f;

        while (canvasGroup.alpha < 1.0f)
        {
            yield return null;
            canvasGroup.alpha += Time.deltaTime / FadeTime;
        }

        canvas.sortingOrder = 2;
        canvasGroup.alpha = 1.0f;
    }

    public IEnumerator FadeInOut(float FadeInTime, float FadeOutTime)
    {
        canvas.sortingOrder = 2;
        canvasGroup.alpha = 0.0f;

        while (canvasGroup.alpha < 1.0f)
        {
            yield return null;
            canvasGroup.alpha += Time.deltaTime / FadeInTime;
        }

        canvasGroup.alpha = 1.0f;
        while (canvasGroup.alpha > 0.0f)
        {
            yield return null;
            canvasGroup.alpha -= Time.deltaTime / FadeOutTime;
        }

        canvas.sortingOrder = 0;
    }
}
