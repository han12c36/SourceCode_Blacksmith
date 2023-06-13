using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoResolution : MonoBehaviour
{
    [Header("DefaultScreenScale")]
    [SerializeField]
    private int defaultWidth = 1920;
    [SerializeField]
    private int defaultHeight = 1080;

    private void Awake()
    {
        SetResolution(Screen.width, Screen.height);
        SetCanvasResolution();
    }

    private void SetCanvasResolution()
    {
        int GCD = PublicLibrary.GetGCD(defaultWidth, defaultHeight);
        int resolutionRatio_Width = defaultWidth / GCD;
        int resolutionRatio_Height = defaultHeight / GCD;
        float DefaultRatio = (float)((float)resolutionRatio_Width / (float)resolutionRatio_Height);
        float curResolutionRatio = (float)((float)Screen.width / (float)Screen.height);

        if (curResolutionRatio > DefaultRatio) gameObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.0f;
        else if(curResolutionRatio < DefaultRatio) gameObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 1.0f;
    }

    private void SetResolution(int deviceWidth,int deviceHeight)
    {
        Screen.SetResolution(defaultWidth, (int)(((float)deviceHeight / deviceWidth) * defaultWidth), true);

        if((float)defaultWidth / defaultHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)defaultWidth / defaultHeight) / ((float)deviceWidth / deviceHeight);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)defaultWidth / defaultHeight);
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f , newHeight);
        }
    }
}