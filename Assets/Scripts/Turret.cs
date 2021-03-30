using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public List<GameObject> enemys = new List<GameObject>();
    void OnTriggerEnter(Collider col)
    {
        if(col.tag =="Enemy")
        {
            enemys.Add(col.gameObject);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemys.Remove(col.gameObject);
        }
    }

    public float attackRateTime = 1;//攻击间隔
    private float timer = 0;

    public GameObject bulletPrefab;//子弹
    public Transform firePosition;
    
    void Start()
    {
        timer = attackRateTime;
    }

    void Update()
    {
        timer = timer + Time.deltaTime;
        if (enemys.Count>0&&timer>=attackRateTime)
        {
            timer = 0;
            Attack();
        }
    }

    void Attack()
    {
        if(enemys[0]==null)
        {
            UpdateEnemys();
        }

        if(enemys.Count>0)
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemys[0].transform);
        }
        else
        {
            timer = attackRateTime;
        }
    }

    void UpdateEnemys()
    {
        enemys.RemoveAll(item=>item==null);
    }
}
