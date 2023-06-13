using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Paths
{
    //Manager

    public const string GameManagerFileDirectory  = "Assets/Resources/GameManager";
    //public const string GameManagerFilePath       = "Assets/Resources/GameManager/GameManager";
    public const string GameManagerFilePath       = "Managers/GameManager";
    public const string SubManagerFilePath        = "Managers/SubManagers/";

    public const string PlayerPrefabsPath         = "UnitPrefabs/Player";
    public const string loadingCanvasPrefabsPath  = "UIPrefabs/LoadingCanvas";
    public const string MainCanvasPrefabsPath     = "UIPrefabs/MainCanvas/MainCanvas";
    public const string SystemCanvasPrefabsPath   = "UIPrefabs/SystemCanvas/SystemCanvas";
    public const string TutorialCanvasPrefabsPath = "UIPrefabs/TutorialCanvas/TutorialCanvas";
    public const string FadeCanvasPrefabsPath     = "UIPrefabs/FadeCanvas/FadeCanvas";

    public const string EffectDataFilePath        = "EffectDatas";

    //Sound

    public const string AudioFolderPath_BGM       = "AudioClips/BGM";
    public const string AudioFolderPath_Effect    = "AudioClips/Effect";

    //Canvas

    public const string MainCanvasPath            = "MainCanvas";
    public const string LoadingCanvasPath         = "LoadingCanvasGroup";
    public const string FadeCanvasPath            = "FadeCanvas";
    public const string SystemCanvasPath          = "SystemCanvas";
    public const string TutorialCanvasPath        = "TutorialCanvas";

    //TutorialData
    public const string TutorialDataPath          = "TutorialData";



}

public static partial class Constants
{
    //LayerName
    public const string EnemyDectionTargetLayerName = "Player";

    //UI Header
    public const string PlayerData = "UserData";

    public const string Clock = "Clock";
    public const string UserData = "UserData";
    public const string Shop = "Shop";
    public const string Inventory = "Invertory";

    //UIMaxPosX
    public const float maxValue = -1200;
}


public static partial class Constants
{
    //GameManager

    public const string GameManagerName = "GameManager";
    public const string DontDestroyManagerBoxName = "DontDestroyManagerBox";


    //SubManager

    //SceneController
    public const string Loading = "Loading...";
    public const float FadeTime = .8f;


    //SoundManager
    public const string SoundObjectName = "Sound";

    public const string BGM_Intro = "Intro";
    public const string BGM_Smithy0 = "Smithy_0";
    public const string BGM_Smithy1 = "Smithy_2";
    public const string BGM_Mine = "Mine_1";
    public const string BGM_Sell = "Sell_0";

    //PoolingManager
    public const string Box = "_Box";
    public const string Clone = "(Clone)";


    //CanvasManager
    public const string MainCanvasName = "MainCanvas";
    public const string LoadingCanvasName = "LoadingCanvas";
    public const string FadeCanvasName = "FadeCanvas";
    public const string TutorialCanvasName = "TutorialCanvas";

    public const string MainCanvasKey = "MainCanvas";
    public const string LoadingCanvasKey = "LoadingCanvas";
    public const string FadeCanvasKey = "FadeCanvas";
    public const string TutorialCanvasKey = "TutorialCanvas";

    //MainCanvas

    public const string ChangeTimeMessage = "시간을 넘긱시겠습니까??";
    public const string DefaultWarningMessage = "Warning //Massage...";

    //FadeCanvas

    public const float DefaultFadeInTime = 3.0f;
    public const float DefaultFadeOutTime = 2.0f;
    public const int FadeCanvasSortingOrder = 2;

    //UserData
    public const string DefaultUserName = "DefaultUserName";
    public const string TestUserName = "TestUserName";
    public const string Level = "Lv_ ";
    public const int DefaultLevel = 1;
    public const int DefaultGold = 1;
    public const int DefaultMaxLevel = 99;
    public const int DefaultMaxGold = 999999999;
    public const int DefaultViewRange = 2;

    //TutorialData
    public const string TutorialDataFileName = "Tutorial Data";
    public const string TutorialDataMenuName = "Scriptable Object/Tutorial Data";

    //UserInterface
    public const string UI = "_UI";
    public const string Common_UI = "Common_UI";

}
