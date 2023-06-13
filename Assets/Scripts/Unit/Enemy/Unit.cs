
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Structs;
using CustomAction;


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
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        rigid = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        animCtrl = GetComponent<Animator>();

        //CreateMaterial();

        Initialize();
    }

    public virtual void Start()
    {

    }
    public virtual void Update()
    {
        if (!canMoveZ && transform.position.z != 0.0f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
    }

    protected virtual void OnEnable() { }   //풀에서 다시 나왔을때

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
            // Material 컴포넌트가 없을 경우, SkinnedMeshRenderer들을 검색해서 머테리얼 등록
            origin = this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>()[0].material;
            Material newMaterial = new Material(origin);
            material = newMaterial;
        }
    }

    protected IEnumerator ChangeColor()
    {
        material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        material.color = Color.white;
    }
}
