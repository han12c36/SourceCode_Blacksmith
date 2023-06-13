using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvas : Window
{
    public Button nextButton;               //대화창을 넘기기 위한 버튼
    public TutorialButton tutorialButton;   //공용 튜토리얼 버튼
    
    public GameObject tutorialImageBg;
    public Image tutorialImage;
    
    public Image npcImage;
    public Text npcNameText;
    
    public Text contentText;
    
    public string contentString;
    
    public Transform t_Panel;
    
    public Sprite defaultHumanSprite;
    
    public override void Awake()
    {
        base.Awake();
        tutorialButton = GetComponentInChildren<TutorialButton>();
    }
    
    public void DrawText(T_Data t_data)
    {
        //if (t_data.npc.npc_name == "없음" || !t_data.trigger) npcImage.gameObject.SetActive(false);
        //else npcImage.gameObject.SetActive(true);
        //
        //npcImage.sprite = t_data.npc.sprite;
        //npcNameText.text = t_data.npc.npc_name;
        //if (t_data.npc.npc_name == "없음" || !t_data.trigger) npcNameText.text = "";
        //
        //contentString = t_data.data;
        //contentText.DOKill();
        //contentText.text = "";
        // NpcDataList npcDataList = SubManager<QuestManager>.Get().npcDataList;
        // nameText.text = npcDataList.npcs[_talk.NpcIndex].npc_name;
        // npcImage.sprite = npcDataList.npcs[_talk.NpcIndex].sprite;
    
        //float typingTime = t_data.data.Length * 0.03f;
        //contentText.DOText(t_data.data, typingTime).SetUpdate(true);
    }
}



