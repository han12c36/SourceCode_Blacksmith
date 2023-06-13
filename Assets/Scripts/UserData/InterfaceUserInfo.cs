using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MVP;
//using MoreMountains.Feedbacks;

public class InterfaceUserInfo : MonoBehaviour , IView<InterfaceUserInfo>
{
    public static UserDataPresenter presenter;
    
    public TextMeshProUGUI level_Text;
    public TextMeshProUGUI name_Text;
    public TextMeshProUGUI gold_Text;
    
    //public MMF_Player goldFeedback;
    
    public Button btn;  //Test

    private void Start() => presenter = new UserDataPresenter(GameManager.userInfo, this);

    public void OnClickEventAddGold()
    { presenter.AddOnClickEvent(() => { gold_Text.text = presenter.V_AddGold(100); }); }
    
    public void OnClickEventAddGold(int amount)
    { presenter.AddOnClickEvent(() => { gold_Text.text = presenter.V_AddGold(amount); }); }
    
    public void OnClickEventAddLevel()
    { presenter.AddOnClickEvent(() => { level_Text.text = presenter.V_LevelUp(1); }); }

    public void OnClickEventChangeName()
    { presenter.AddOnClickEvent(() => { name_Text.text = presenter.V_ChangeName(Constants.TestUserName); }); }
}