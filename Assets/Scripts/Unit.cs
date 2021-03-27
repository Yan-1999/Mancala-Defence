/// <summary>
/// Author: MjuTeX
/// Project: Mancala Defence
/// File: Unit.cs
/// </summary>
using UnityEngine;


public class Unit : Entity
{

    public enum Type : int
    {
        White = 0,
        Green,
        Red,
    };
    public enum AttrEnum { Life, Damage, Skill }
    public Cell OnCell { get; set; } = null;
    public float Skill { get; set; }

    public Type UnitType = Type.White;


    public void SetUnitAttrs(UnitAttr unitAttr)
    {
        Life = unitAttr.Life;
        Damage = unitAttr.Damage;
        Skill = unitAttr.Skill;
    }

    public void Upgrade(AttrEnum unitAttrEnum)
    {
        switch (unitAttrEnum)
        {
            case AttrEnum.Life:
                Life++;
                break;
            case AttrEnum.Damage:
                Damage++;
                break;
            case AttrEnum.Skill:
                Skill++;
                break;
            default:
                break;
        }
    }

    protected override void TryAttack()
    {
        /*GameManager.Instance.UnitAttack(this);*/
    }

    protected override void OnDeath()
    {
        /*GameManager.Instance.UnitDeath(this);
        OnCell = null;
        Destroy(gameObject);*/
    }
}
