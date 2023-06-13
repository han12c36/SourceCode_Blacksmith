using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum T_EType { None, Sprite, Window }
public enum T_Type { None, Button, Key, Window, ItemDrag }
public enum FrameType { Normal }

public enum StartEvent_Type { None, Button, Window, Kill, Scene, Item, Interact, SlotDrag}

[CreateAssetMenu(fileName = Constants.TutorialDataFileName, menuName = Constants.TutorialDataMenuName)]
public class TutorialData : ScriptableObject
{
    [HideInInspector] public T_Data[] talkDatas;
    public int registerOrder;
    public bool isFirst;

    public StartEvent_Type e_StartEvent;
    public UnityAction startEvent;
    public int startEvent_index;
    public string startEvent_str;
}

[System.Serializable]
public struct T_Data
{
    public int spriteIndex;
    public Sprite t_sprite;
    //public NpcData npc;
    public string data;
    public bool trigger;
    public bool e_trigger;
    public bool lock_trigger;
    public T_EType etype;
    public T_Type type;
    public int select;

    public TriggerData Serialize(T_Protocol ptorocol)
    {
        TriggerData triggerData = new TriggerData();

        if (type == T_Type.None) { }
        else if (type == T_Type.Button) triggerData.Button = ptorocol.allButton[select];
        else if (type == T_Type.Key) triggerData.Key = ptorocol.allKey[select];
        else if (type == T_Type.Window) triggerData.Window = ptorocol.allWindow[select];

        return triggerData;
    }
}
