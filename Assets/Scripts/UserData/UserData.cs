using MVP;

/// <summary>
/// presenter의 Model로 지정하기 위한 Interface상속
/// </summary>
public class UserData : IModel<UserData>
{
    public  string userName;
    public  float  viewRange;
    private int    maxLevel;
    public  int    level;
    private int    maxGold;
    public  int    gold;

    //public ReinforceData reinforceData;
    //public int stat_HasPoint;
    //public int stat_Strength;
    //public int stat_Dexterity;
    //public int stat_Luck;

    //private CharactorWindow charactorWindow;

    // 현재 플레이어가 판매 가능한 슬롯의 갯수
    //public int active_Slot;

    // 현재 플레이어가 가지고 있는 "마인"씬의 인벤토리 행의 칸 수 (기본 1)
    //public int active_SubInv_Slot_Row;
    public void ModelInitialized()
    {
        userName  = Constants.DefaultUserName;
        maxLevel  = Constants.DefaultMaxLevel;
        level     = Constants.DefaultLevel;
        maxGold   = Constants.DefaultMaxGold;
        gold      = Constants.DefaultGold;
        viewRange = Constants.DefaultViewRange;

        //reinforceData.hammerLevel = 1;
        //reinforceData.anvilLevel = 1;
        //reinforceData.smithyLevel = 1;
        //reinforceData.bagLevel = 1;

        //stat_HasPoint = 5;
        //stat_Strength = 1;
        //stat_Dexterity = 1;
        //stat_Luck = 1;
        //charactorWindow = Window.Get<CharactorWindow>();

        //active_Slot = 1; // 상점 올릴수 있는 슬롯 갯수
        //active_SubInv_Slot_Row = 1; // 광산 인벤토리 행
    }
    public string AddGold(int addAmount = 0)
    {
        gold += addAmount;
        if (gold > maxGold) gold = maxGold;
        return gold.ToString();
    }
    public string LevelUp(int addLevel = 0) 
    {
        level += addLevel;
        if(level > maxLevel) level = maxLevel;
        return Constants.Level + level.ToString();
    }
    public string ChangeName(string changeName = Constants.DefaultUserName) 
    {
        if (userName == changeName) return userName;
        return userName = changeName;
    }
    public void OnTriggerEventAddGold(int addAmount) => InterfaceUserData.presenter.M_AddGold(AddGold(addAmount));
    public void OnTriggerEventLevelUp(int addLevel) => InterfaceUserData.presenter.M_LevelUp(LevelUp(addLevel));
    public void OnTriggerEventChangeName()
        => InterfaceUserData.presenter.M_ChangeName(ChangeName(Constants.TestUserName));

    //public void AddStatPoint(int amount = 0)
    //{
    //    stat_HasPoint += amount;
    //}
    //public void AddStatStrength(int amount = 0)
    //{
    //    stat_Strength += amount;
    //}
    //public void AddStatDexterity(int amount = 0)
    //{
    //    stat_Dexterity += amount;
    //}
    //public void AddStatLuck(int amount = 0)
    //{
    //    stat_Luck += amount;
    //    charactorWindow.stat_Luck_Text.text = stat_Luck.ToString();
    //}
}
