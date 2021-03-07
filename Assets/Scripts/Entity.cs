using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float AttackTimerMax { get; set; } = 2.0f;

    public float AttackTimer { get; private set; } = 0.0f;
    public float Life { get; set; }
    public float Damage { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = GameManager.Instance.deltaTime();
        AttackTimer += deltaTime;
        if (AttackTimer >= AttackTimerMax)
        {
            TryAttack();
            AttackTimer = 0;
        }
    }

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
