using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
using TMPro;

public class LoadingCanvas : CustomCanvas
{
    private TextMeshProUGUI loadingText;
    public TextMeshProUGUI LoadingText { get { return loadingText; } }

    //private DOTweenTMPAnimator tmpAnimator;

    private void Awake()
    {
        //loadingText = GetComponentInChildren<TextMeshProUGUI>();
        //SoundManager.Instance.StopAllSound();
        //SoundManager.Instance.GetMusicCanvas.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        //if(tmpAnimator == null) tmpAnimator = new DOTweenTMPAnimator(loadingText);

        //for (var i = 0; i < tmpAnimator.textInfo.characterCount; i++)
        //{
        //    tmpAnimator.DOOffsetChar(i, Vector3.zero, 0);
        //}
    }

    public void Play(float duration)
    {
        //const float EACH_DELAY_RATIO = 0.01f;
        //var eachDelay = duration * EACH_DELAY_RATIO;
        //var eachDuration = duration - eachDelay;
        //var charCount = tmpAnimator.textInfo.characterCount;

        //for (var i = 0; i < charCount; i++)
        //{
        //    DOTween.Sequence()
        //        .Append(tmpAnimator.DOOffsetChar(i, Vector3.up * 30, eachDuration / 4).SetEase(Ease.OutFlash, 2))
        //        .SetDelay(eachDelay * i)
        //        .SetLoops(3);
        //}
    }

    public void Stop()
    {
        //DOTween.Kill(loadingText);
    }
}