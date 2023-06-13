using UnityEngine;
using CustomAction;

public abstract class Enemy<A> : Unit where A : IActionTable
{
    protected Player target;
    protected Transform targetTran;
    private IActionTable actionTable;
    public EnemyWeapon<Enemy<IActionTable>> enemyWeapon;
    public Weapon<Unit> weapon;

    #region CanAccessProperty
    public int HitCount { get { return status.hitCount; } set { status.hitCount = value; } }
    public float CurHP => status.curHp; public float MaxHP => status.maxHp;
    public float Range => status.perceiveRange;
    public float AttRange => status.attRange;
    public bool IsCombat => status.isCombat;
    public Animator AnimCtrl => animCtrl;
    public Material Material => material;
    public Vector3 t_Pos => target.transform.position;
    public float DistToTarget => target != null ? Vector3.Distance(targetTran.position, transform.position) : 0;
    public A ActionTable => (A)actionTable;
    public EnemyWeapon<Enemy<IActionTable>> EnemyWeapon => enemyWeapon;
    public Weapon<Unit> Weapon => weapon;
    #endregion

    public override void Awake()
    {
        base.Awake();
        actionTable = GetComponent<IActionTable>();
        enemyWeapon = GetComponentInChildren<EnemyWeapon<Enemy<IActionTable>>>();

        if (enemyWeapon != null) weapon = (Weapon<Unit>)enemyWeapon;
        
        if (weapon != null) weapon.detectionLayer = 1 << LayerMask.NameToLayer("Player");
    }
    public override void Start()
    {
        base.Start();
        //target = SubManager.Get<InGameManager>().Player;
        //targetTran = target.transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //if (target == null)
        //{
        //    target = SubManager.Get<InGameManager>().Player;
        //    targetTran = target.transform;
        //}
    }
    
    public override void Hit(float dmg)
    {
        //SubManager.Get<QuestManager>().SearchQuestProgressKillEnemy(status);
    }
}
