/// <summary>
/// Author: MjuTeX
/// Project: Mancala Defence
/// File: Unit.cs
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Unit : Entity
{
    public List<GameObject> enemys = new List<GameObject>();
    

    public float attackRateTime = 1;//攻击间隔

    public GameObject bulletPrefab;//子弹
    public Transform firePosition;
    




    public Slider LifeBar;
    public enum Type : int
    {
        White = 0,
        Green,
        Red,
    };
    public enum AttrEnum { Life, Damage, Skill }
    public Cell OnCell { get; set; } = null;
    public float Skill { get; set; }
    public float LifeLimit { get; set; }

    public Type UnitType = Type.White;


    public void SetUnitAttrs(UnitAttr unitAttr)
    {
        Life = unitAttr.Life;
        Damage = unitAttr.Damage;
        Skill = unitAttr.Skill;
        LifeLimit = Life;
    }

    public void Upgrade(AttrEnum unitAttrEnum)
    {
        switch (unitAttrEnum)
        {
            case AttrEnum.Life:
                LifeLimit++;
                Life = LifeLimit;
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

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy"|| col.tag == "E.enemy")
        {
            enemys.Add(col.gameObject);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy" || col.tag == "E.enemy")
        {
            enemys.Remove(col.gameObject);
        }
    }

    protected override void TryAttack()
    {
        if (this.OnCell.IsExhausted)
        {
            return;
        }
        if (enemys.Count == 0)
        {
            return;
        }
        if (enemys[0] == null)
        {
            UpdateEnemys();
        }
        GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
        if (enemys.Count == 0)
        {
            return;
        }
        bullet.GetComponent<Bullet>().SetDamage(this.Damage);
        bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);
        Invoke("WaitAttack", 0.3f);
        if (enemys[0].tag == "E.enemy")
        {
            Invoke("Wait", 0); 
        }
        else
        {
            Invoke("WaitAttack", 0);
        }
    }
    void Wait()
    {
        GameManager.Instance.EnemyAttack(this.OnCell);
    }

    void WaitAttack()
    {
        ReceiveDamage(this.Damage * 0.01f);
        this.LifeBar.value = this.Life / this.LifeLimit;
    }


    void UpdateEnemys()
    {
        enemys.RemoveAll(item => item == null);
    }


    protected override void OnDeath()
    {
        GameManager.Instance.UnitDeath(this);
        OnCell = null;
        Destroy(gameObject);
    }
}
