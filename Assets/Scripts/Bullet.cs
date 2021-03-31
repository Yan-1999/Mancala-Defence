using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 50;
    public Cell cell;
    public float speed = 20;

    public GameObject explosionEffectPrefab;

    private Transform target;

    public void SetDamage(float Damage)
    {
        this.damage = Damage;
    }

    public void SetTarget(Transform _target)
    {
        this.target = _target;
    }



    void Update()
    {
        if(target==null)
        {
            Die();
            return;
        }
        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy" || col.tag == "E.enemy")
        {
            col.GetComponent<Enemy>().ReceiveDamage(damage);
            Die();
        }
    }
    void Die()
    {
        GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
        Destroy(effect, 1);
    }
}
