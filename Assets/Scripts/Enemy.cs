using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Slider LifeBar;
    public float Speed = 1;
    public GameObject explosionEffect;
    private Transform[] positions;
    private Vector3 v1 = new Vector3(6.25f, 0.5f, -3.75f);
    private int index = 0;
    public float Damage = 10;
    public float Life = 150;
    private float MaxLife = 150;

    /* public Slider LifeBar;
     public Type EnemyType = Type.Common;
     public enum AttrEnum { Life, Damage, Speed }
     public enum Type : int
     {
         Common = 0,
         elite,
     };
     public void SetEnemyAttrs(EnemyAttr enemyAttr)
     {
         Life = enemyAttr.Life;
         Damage = enemyAttr.Damage;
         Speed = enemyAttr.Speed;
     }*/

    void Start()
    {
        MaxLife = Life;
        if(transform.position== v1)
        {
            positions = Waypoints.positions;

        }
        else
        {
            positions = Waypoint1.positions;
        }
        

    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (index >= positions.Length)
            return;
        transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * Speed);
        if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
        {
            index++;
        }
        if (index >= positions.Length)
        {
            ReachDestination();
        }
    }

    //enemy reach player base
    void ReachDestination()
    {
        GameManager.Instance.EnemyPass();
        GameObject.Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        EnemySpawner.CountEnemyAlive--;
    }

    public void ReceiveDamage(float damage)
    {
        if (Life <= 0)
            return;
        Life = Life - damage;
        LifeBar.value = Life / MaxLife;
        if (Life <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        GameObject effect=GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(this.gameObject);
        GameManager.Instance.Assets.GainCoinOnEmemyKill();
    }

    /*protected override void TryAttack()
    {
        GameManager.Instance.EnemyAttack(this);
    }*/

    /*protected override void OnDeath()
    {
        GameManager.Instance.UnitDeath(this);
        OnCell = null;
        Destroy(gameObject);
    }*/
}
