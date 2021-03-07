/// <summary>
/// Author: MjuTeX
/// Project: Mancala Defence
/// File: Unit.cs
/// </summary>

public class Unit : Entity
{

    public enum Type : int
    {
        White = 0,
        Green,
        Red,
    };
    public Cell OnCell { get; set; } = null;
    public Type UnitType { get; set; } = Type.White;
    public float Skill { get; set; }

    public void SetUnitAttrs(UnitAttr unitAttr)
    {
        Life = unitAttr.Life;
        Damage = unitAttr.Damage;
        Skill = unitAttr.Skill;
    }

    protected override void TryAttack()
    {
        GameManager.Instance.UnitAttack(this);
    }

    protected override void OnDeath()
    {
        GameManager.Instance.UnitDeath(this);
        OnCell = null;
        Destroy(gameObject);
    }
}
