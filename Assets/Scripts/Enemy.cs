using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed = 1;
    private Transform[] positions;
    private int index = 0;
    public float Damage = 10;
    public float Hp = 150;
    void Start()
    {
        positions = Waypoints.positions;
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

    }
}
