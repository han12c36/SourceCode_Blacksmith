using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;
using Enums;
using CustomAction;

public interface IWeapon { }

public abstract class Weapon<T> : MonoBehaviour, IWeapon where T : Unit
{
    [SerializeField]
    protected T owner;
    [SerializeField]
    protected new Collider collider;

    public List<GameObject> hitObjs;

    public int detectionLayer;
    public float dmg;
    public eAttackType attackType;

    public int GetDetectionLayer { get { return detectionLayer; } }
    public T Owner { get { return owner; } set { owner = value; } }
    public Collider GetCollider { get { return collider; } }

    protected virtual void Awake()
    {

        if (Owner == null) Owner = GetComponentInParent<T>();
        if (Owner != null) collider = GetComponent<Collider>();
        if (collider == null) collider = GetComponentInChildren<Collider>();
        hitObjs = new List<GameObject>();

    }
    protected virtual void Start() { }
    protected virtual void Update() { }
    protected virtual void FixedUpdate() { }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (hitObjs.Find(x => x == other.transform.root.gameObject)) return;

        //맞는놈이 플레이어
        if (other.transform.root.gameObject.GetComponent<Player>() != null)
        {
            DamagedStruct dmgst = new DamagedStruct();
            dmgst.dmg = dmg;
        
            other.transform.root.gameObject.GetComponent<Player>().Hit(dmgst.dmg);
        }
        //맞는놈이 몬스터
        else if (other.transform.root.gameObject.GetComponent<Enemy<IActionTable>>() != null)
        {
            owner.status.hitCount++;
        
            //float weaponAtkValue = 1f;
            //float dmg = Player.Instance.playerInfo.power * Player.Instance.playerInfo.curAtkWeight * weaponAtkValue;
            //other.transform.root.gameObject.GetComponent<Enemy<IActionTable>>().Hit(dmg);
        
            //GameObject damageFont =
            //    SubManager.Get<PoolingManager>().LentalObj("DamageFont");
        
            //damageFont.transform.position = Camera.main.WorldToScreenPoint(other.transform.position);
            //damageFont.GetComponent<DamageFont>().PlayEffect(dmg);
        }
        
    }
    public void OnOffWeaponCollider(bool value)
    {
        if (value)
        {
            if (!collider.enabled) collider.enabled = value;
            else { hitObjs.Clear(); return; }
        }
        else
        {
            if (!collider.enabled) { hitObjs.Clear(); return; }
            else collider.enabled = value;
        }

        hitObjs.Clear();
    }
}
