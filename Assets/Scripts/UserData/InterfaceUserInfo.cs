using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MVP;
//using MoreMountains.Feedbacks;

/// <summary>
/// presenter의 View로 지정하기 위한 interface상속
/// 어떤 데이터의 기능을 끌어다 쓸건지에 대한 명시가 필요 => IView<T> where T : IModel
/// </summary>
public class InterfaceUserData : MonoBehaviour , IView<InterfaceUserData>
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