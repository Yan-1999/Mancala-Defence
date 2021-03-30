using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float AttackTimerMax { get; set; } = 1.0f;

    public float AttackTimer { get; private set; } = 0.0f;
    public float Life { get; set; }
    public float Damage { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        AttackTimer = AttackTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        AttackTimer += deltaTime;
        if (AttackTimer >= AttackTimerMax)
        {
            TryAttack();
            AttackTimer = 0;
        }
    }
    /*void Update()
    {
        timer = timer + Time.deltaTime;
        if (enemys.Count > 0 && timer >= attackRateTime)
        {
            timer = 0;
            Attack();
        }
    }*/

    protected abstract void TryAttack();

    public void ReceiveDamage(float damage)
    {
        Life -= damage;
        if (Life <= 0)
        {
            OnDeath();
        }
    }

    protected abstract void OnDeath();
}
