using UnityEngine;

namespace Enums
{
    public enum ID
    {
        //TimeManger,
        InGameManager,
        CanvasManager,
        //InventoryManager,
        PoolingManager,
        SoundManager,
        //QuestManager,
        End,
    }
    public enum WindowID
    {
        None = -1,
        CheckWindow,
        WarningWindow,
        ShortWindow,
        SystemWindow,
        SettingWindow,
        InventoryWindow,
        CharactorWindow,
        QuestWindow,
        QuestInfoWindow,
        SellWindow,
        ReinforceWindow,
        ShopWindow,
        End,
    }

    public enum SceneIndex
    {
        Intro,
        Smithy,
        Workshop,
        Mine,
        Sell,
    }

    public enum SoundType
    {
        BGM_1,
        BGM_2,
        Effect,
        End
    }

    public enum UnitNameTable
    {
        None,
        Mouse,
        Centipede,
        Bat,
        Spider,
        Zombie,
        Imp,
        Oak,
        Basilisk,
        Kerberos,
        GoblinWarrior,
        Demon,
        End
    }

    public enum eAttackType
    {
        Weak,
        Hard
    }

    public struct DamagedStruct
    {
        public float dmg;

        public Vector3 dmgDir;
        public GameObject attackObj;

        public Enums.eAttackType atkType;   //가드 판단
    }

}
