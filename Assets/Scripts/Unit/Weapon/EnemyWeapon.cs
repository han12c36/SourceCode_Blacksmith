using CustomAction;

public class EnemyWeapon<T> : Weapon<Unit> where T : Enemy<IActionTable>
{
    protected override void Start()
    {
        base.Start();
        //if(weaponType == EnemyWeaponType.Range) OnOffWeaponCollider(true);
    }
}
