using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEditor;


public interface IFrame { void Setting(); void Enter(); void Escape(); }

public class TriggerData
{
    public Type type;

    private Button button;
    private KeyCode key;
    private Window window;

    private Transform o_trButton;
    private Transform o_trWindow;

    public Button Button { get { return button; } set {  button = value; o_trButton = value.transform.parent; } }
    public KeyCode Key { get { return key; } set { key = value; } }
    public Window Window { get { return window; } set { window = value; o_trWindow = value.transform.parent; } }
    public Transform O_trButton => o_trButton;
    public Transform O_trWindow => o_trWindow;
}

public class T_Frame
{
    protected T_Protocol     protocol;
    protected T_Data[]       talkDatas;
    protected TriggerData[]  triggerDatas;

    protected int            curTalkIndex = 0;
    protected int            triggerIndex;
    protected TutorialCanvas canvas;
    protected string         text;
    protected Button         nextbutton;

    public void ChangeCanvas(GameObject obj, Transform transform) => obj.transform.parent = transform;

    #region Adapter
    public Button triggerButton()
    { return protocol.triggerDataList[protocol.curTutorialIndex][curTalkIndex].Button; }
    public KeyCode triggerKey()
    { return protocol.triggerDataList[protocol.curTutorialIndex][curTalkIndex].Key; }
    public Window triggerWindow()
    { return protocol.triggerDataList[protocol.curTutorialIndex][curTalkIndex].Window; }
    public Transform TrButton()
    { return protocol.triggerDataList[protocol.curTutorialIndex][curTalkIndex].O_trButton; }
    public Transform TrWindow()
    { return protocol.triggerDataList[protocol.curTutorialIndex][curTalkIndex].O_trWindow; }

    /*
     * 상황에 대한 처리는 여기서 하는게 맞다.
     * Button을 예시로 위임하는 것
     * T_Type.Key는 키 값을 받아오려면 Update 를 돌려야 하기 때문에 Coroutine을 넘겨줘서 키 누름을 받아옴.
     * 
     * 트리거가 체크 되어있으면 Up
     * 트리거가 체크 되어있지 않으면 Down
     * 
     * 다음 대화창 넘어갈 때 .trigger 비교 후 넘겨 줌
     */

    public void TriggerStepUp() 
    {
        switch (talkDatas[curTalkIndex].type)
        {
            case T_Type.None:
                break;
            case T_Type.Button:
                Button _triggerButton = triggerButton();
                Entrust_FrameStep(_triggerButton,true);
                ChangeCanvas(_triggerButton.gameObject, canvas.t_Panel);
                break;
            case T_Type.Key:
                Entrust_FrameStep(true);
                GameManager.Instance.StartCoroutine(KeyInputCoro(triggerKey()));
                break;
            case T_Type.Window:
                Entrust_FrameStep(true);
                GameManager.Instance.StartCoroutine(WindowInputCoro(triggerWindow()));
                break;
            case T_Type.ItemDrag:
                Entrust_FrameStep(true);
                break;
        }
    }

    public void TriggerStepDown() 
    {
        switch (talkDatas[curTalkIndex].type)
        {
            case T_Type.None:
                break;
            case T_Type.Button:
                {
                    Button _triggerButton = triggerButton();
                    Entrust_FrameStep(_triggerButton, false);
                    ChangeCanvas(triggerButton().gameObject, TrButton());
                }
                break;
            case T_Type.Key:
                {
                    Entrust_FrameStep(false);
                    GameManager.Instance.StopCoroutine(KeyInputCoro(triggerKey()));
                }
                break;
            case T_Type.Window:
                Entrust_FrameStep(false);
                GameManager.Instance.StopCoroutine(WindowInputCoro(triggerWindow()));
                break;
        }
    }

    #endregion
    
    public void FrameStep()
    {
        if (talkDatas[curTalkIndex].trigger) TriggerStepDown();

        curTalkIndex++;
        if (curTalkIndex > talkDatas.Length - 1) { ((IFrame)this).Escape(); return; }
        TalkPrint(talkDatas[curTalkIndex]);
        ShowImageCanvas(true);

        //if (GameManager.sceneCtrl.CurSceneIndex == Enums.SceneIndex.Mine)
        //    Player.Instance.actionTable.lockInput = talkDatas[curTalkIndex].lock_trigger;

        if (talkDatas[curTalkIndex].trigger) TriggerStepUp();
    }

    public void TalkPrint(T_Data talk) { if (text != talk.data) canvas.DrawText(talk); }

    public void Entrust_FrameStep(Button triggerButton,bool toTrigger)
    {
        hookerCheck(triggerButton, toTrigger);

        if (toTrigger)
        { nextbutton.onClick.RemoveListener(FrameStep); triggerButton.onClick.AddListener(FrameStep); nextbutton.gameObject.SetActive(false);}
        else
        { nextbutton.gameObject.SetActive(true); nextbutton.onClick.AddListener(FrameStep); triggerButton.onClick.RemoveListener(FrameStep); }
    }

    public void Entrust_FrameStep(bool toTrigger)
    {
        if (toTrigger)
        { nextbutton.onClick.RemoveListener(FrameStep); nextbutton.gameObject.SetActive(false); }
        else
        { nextbutton.gameObject.SetActive(true); nextbutton.onClick.AddListener(FrameStep); }
    }

    private void hookerCheck(Button triggerButton, bool toTrigger)
    {
        //TutorialButtonHooker hooker = triggerButton.gameObject.GetComponent<TutorialButtonHooker>();
        //if (hooker != null)
        //{
        //    hooker.Init();
        //    if (toTrigger)
        //    {
        //        hooker.transform.position = hooker.originPos;
        //    }
        //    else
        //    {
        //        hooker.invisibleBtn.onClick.RemoveAllListeners();
        //        hooker.transform.position = new Vector3(9999999, 9999999, 0);
        //    }
        //}
    }

    //기본 0 ~ 1.0 기준
    public static IEnumerator Btn_Blink(Image _image)
    {
        float t = 0;
        float blinkSpeed = 4.0f;    

        while (true)
        {
            t += Time.deltaTime * blinkSpeed;
            _image.color = new Vector4(1, 1, 1, (float)Math.Cos(t));

            yield return null;
        }
    }

    public IEnumerator WindowInputCoro(Window win)
    {
        bool isLoop = true;
        bool isActive;
        if (win.windowActivated == true || win.Activated == true)
            isActive = true;
        else
            isActive = false;
        while (isLoop)
        {
            if (isActive == false && (win.windowActivated == true || win.Activated == true) || isActive == true && (win.windowActivated == false || win.Activated == false))
            {
                isLoop = false;
            }
            yield return null;
        }
        GameManager.Instance.StopCoroutine(WindowInputCoro(triggerWindow()));

        FrameStep();
    }

    public IEnumerator KeyInputCoro(KeyCode key)
    {
        bool isTrigged = false;
        while (isTrigged == false)
        {
            if (Input.GetKeyDown(key)) isTrigged = true;
            yield return null;
        }

        FrameStep();
    }

    public IEnumerator ItemDragCoro()
    {
        bool isTrigged = false;
        while (isTrigged == false) { yield return null; }
        FrameStep();
    }

    public void ShowImageCanvas(bool isShow)
    {
        if (isShow)
        {
            if (talkDatas[curTalkIndex].t_sprite != null)
            {
                if(talkDatas[curTalkIndex].t_sprite.name != "없음")
                {
                    canvas.tutorialImageBg.SetActive(true);
                    canvas.tutorialImage.sprite = talkDatas[curTalkIndex].t_sprite;
                }
                else canvas.tutorialImageBg.SetActive(false);
            } 
            else canvas.tutorialImageBg.SetActive(false);
        } 
        else canvas.tutorialImageBg.SetActive(false);
    }
}

public class NormalFrame : T_Frame, IFrame
{
    //공통적으로 앵간치 특별한 튜토리얼이 아닌이상 임마로 돌릴꺼임
    public void Setting() { }
    public void Enter()
    {
        if (nextbutton)
        {
            nextbutton.onClick.RemoveAllListeners();
            nextbutton.onClick.AddListener(FrameStep);
        }


        talkDatas = protocol.dataList[protocol.CurTutorialIndex].talkDatas;

        canvas.gameObject.SetActive(true);

        TalkPrint(talkDatas[curTalkIndex]);
        ShowImageCanvas(true);
        if (talkDatas[curTalkIndex].trigger) TriggerStepUp();

        //if (GameManager.sceneCtrl.CurSceneIndex == Enums.SceneIndex.Mine)
        //    Player.Instance.actionTable.lockInput = talkDatas[curTalkIndex].lock_trigger;
    }
    public void Escape()
    {
        canvas.gameObject.SetActive(false);
        curTalkIndex = 0;
        TriggerStepDown();
        ShowImageCanvas(false);

        //if (GameManager.sceneCtrl.CurSceneIndex == Enums.SceneIndex.Mine)
        //    Player.Instance.actionTable.lockInput = false;

        talkDatas = null;
        protocol.curTutorialIndex++;

        NextFrameCheck();
    }

    private void NextFrameCheck()
    {
        //Debug.Log("체크");
        //protocol.dataList[protocol.curTutorialIndex].startEvent = null; // 초기화
        //
        //if (protocol.curTutorialIndex <= protocol.dataList.Length)
        //{
        //    switch(protocol.dataList[protocol.curTutorialIndex].e_StartEvent)
        //    {
        //        case StartEvent_Type.Button:
        //            {
        //                Button[] btns = SubManager<CanvasManager>.Get().MainCanvas.GetComponentsInChildren<Button>();
        //                Debug.Log(protocol.curTutorialIndex);
        //                Debug.Log(btns[(protocol.dataList[protocol.curTutorialIndex]).startEvent_index].name);
        //
        //                Button targetBtn = PublicLibrary.FindButtonByName(btns, protocol.dataList//[protocol.curTutorialIndex].startEvent_str);
        //                targetBtn.onClick.AddListener(() => { protocol.OnTutorial(); });
        //                //btns[(protocol.dataList[protocol.curTutorialIndex]).startEvent_index].onClick.AddListener(()/=>/ { protocol.OnTutorial(); });
        //                protocol.dataList[protocol.curTutorialIndex].startEvent += () => { protocol.OnTutorial(); };
        //            }
        //            break;
        //        case StartEvent_Type.Window:
        //            {
        //                Window[] allWindow = SubManager<CanvasManager>.Get/().MainCanvas.GetComponentsInChildren<Window>/();
        //                Window targetWindow = PublicLibrary.FindWindowByName(allWindow, protocol.dataList//[protocol.curTutorialIndex].startEvent_str);
        //                protocol.dataList[protocol.curTutorialIndex].startEvent += () => { protocol.OnTutorial(); };
        //                //0515 JM
        //                GameManager.coroutineHelper.StartCoroutine(CheckWindowCoro(targetWindow));
        //            }
        //            break;
        //        case StartEvent_Type.Kill:
        //            protocol.dataList[protocol.curTutorialIndex].startEvent += () => { protocol.OnTutorial(); };
        //            break;
        //        case StartEvent_Type.Scene:
        //            protocol.dataList[protocol.curTutorialIndex].startEvent += () => { protocol.OnTutorial(); };
        //            break;
        //        case StartEvent_Type.Item:
        //            protocol.dataList[protocol.curTutorialIndex].startEvent += () => { protocol.OnTutorial(); };
        //            break;
        //        case StartEvent_Type.Interact:
        //            protocol.dataList[protocol.curTutorialIndex].startEvent += () => { protocol.OnTutorial(); };
        //            break;
        //        case StartEvent_Type.SlotDrag:
        //            protocol.dataList[protocol.curTutorialIndex].startEvent += () => { protocol.OnTutorial(); };
        //            break;
        //    }
        //}
    }

    private IEnumerator CheckWindowCoro(Window _window)
    {
        bool isTrigged = false;
        while(!isTrigged)
        {
            if (_window.Activated == true)
            {
                isTrigged = true;
                GameManager.Instance.protocol.dataList[GameManager.Instance.protocol.curTutorialIndex].startEvent?.Invoke();
                yield break;
            }
            yield return null;
        }
    }

    #region 생성자
    public NormalFrame(bool trash, T_Protocol _protocol)
    {
        protocol = _protocol; 
        canvas = protocol.canvas;
        text = canvas.contentString;
        nextbutton = canvas.nextButton;
        Setting();
    }
    #endregion
}

public class T_Protocol
{
    public int curTutorialIndex = 0;
    public TutorialCanvas canvas;
    public Dictionary<FrameType, IFrame> frame_Dic = new Dictionary<FrameType, IFrame>();
    public TutorialData[] dataList;
    public TriggerData[][] triggerDataList;
    public Button[] allButton;
    public KeyCode[] allKey;
    public Window[] allWindow;

    public UnityAction action;
    public bool isTutorialPlayed = false;

    public T_Protocol(TutorialCanvas _canvas, Button[] _allButton, KeyCode[] _allKey, Window[] _allWindow) 
    {
        canvas = _canvas; allButton = _allButton; allKey = _allKey; allWindow = _allWindow;
        RegisterData();
    }

    public int CurTutorialIndex => curTutorialIndex;

    public void RegisterData()
    {
        dataList = Resources.LoadAll<TutorialData>(Paths.TutorialDataPath);
        
        PublicLibrary.SortArrayByField(dataList, order => order.registerOrder);
        
        frame_Dic.Add(FrameType.Normal, G_Frame<NormalFrame>.Create(this));
        triggerDataList = new TriggerData[dataList.Length][];
        for (int i = 0; i < dataList.Length; i++)
        {
            triggerDataList[i] = new TriggerData[dataList[i].talkDatas.Length];
            for (int j = 0; j < triggerDataList[i].Length; j++)
            {
                TriggerData data = dataList[i].talkDatas[j].Serialize(this);
                if (data != null) triggerDataList[i][j] = data;
            }
        }
    }
    private IFrame GetFrame(FrameType key = FrameType.Normal)
    {
        if (frame_Dic.ContainsKey(key)) return frame_Dic[key];
        else return null;
    }

    public void OnTutorial() => GetFrame().Enter();
    public void OnTutorial(int index) { curTutorialIndex = index; OnTutorial(); }

    public void OffTutorial(int key) => GetFrame().Escape();
    public void OffTutorial() => GetFrame().Escape();
}

public static class G_Frame<T> where T : T_Frame
{
    #region
    public static T Create(T_Protocol _protocol)
    { T frame = (T)Activator.CreateInstance(typeof(T), true, _protocol); return frame; }
    #endregion
}