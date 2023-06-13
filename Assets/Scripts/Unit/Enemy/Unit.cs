
using System.Collections;
using UnityEngine;
using Structs;

/// <summary>
/// Unit은 Player와 Enemy를 구분없이 상호작용하기 위한 부모
/// </summary>

public abstract class Unit : MonoBehaviour
{
    public Status status;

    protected Rigidbody rigid;
    protected new Collider collider;
    protected Animator animCtrl;

    protected MeshRenderer meshRenderer;
    protected Material material;

    public bool canMoveZ;

    public int wallLayer;


    public abstract void Initialize();
    public abstract void UnitReset();

    public virtual void Awake()
    {
        //meshRenderer = GetComponentInChildren<MeshRenderer>();
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        animCtrl = GetComponent<Animator>();

        //CreateMaterial();

        Initialize();
    }

    public virtual void Start() { }
    
    public virtual void Update()
    {
        if (!canMoveZ && transform.position.z != 0.0f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
    }

    /// <summary>
    /// Pool에서 나왔을 떄 
    /// </summary>
    protected virtual void OnEnable() { }

    /// <summary>
    /// pool에 들어갈 때
    /// </summary>
    protected virtual void OnDisable() { UnitReset(); }
    
    protected virtual void FixedUpdate() { }
    protected virtual void LateUpdate() { }

    //public bool isCurrentAnimationOver(float time)
    //{
    //    return components.aniCtrl.GetCurrentAnimatorStateInfo(0).normalizedTime > time;
    //}

    public virtual void Hit(float dmg)
    {
        status.hitCount++;
        status.curHp -= dmg;
        //GameManager.coroutineHelper.StartCoroutine(ChangeColor());
        //SubManager<QuestManager>.Get().SearchQuestProgressKillEnemy(status);
    }

    void CreateMaterial()
    {

        Material origin;
        if (this.gameObject.GetComponentsInChildren<MeshRenderer>()[0] != null)
        {
            origin = this.gameObject.GetComponentsInChildren<MeshRenderer>()[0].material;
            Material newMaterial = new Material(origin);
            this.gameObject.GetComponentsInChildren<MeshRenderer>()[0].material = newMaterial;
            material = newMaterial;
        }
        else
        {
            origin = this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>()[0].material;
            Material newMaterial = new Material(origin);
            material = newMaterial;
        }
    }

    protected IEnumerator ChangeColor()
    {
        material.color = Color.red;
        yield return CachedCoroutine.waitForHalf;
        material.color = Color.white;
    }
}
