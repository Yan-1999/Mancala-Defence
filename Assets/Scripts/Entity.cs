using UnityEngine;

public abstract class Entity : MonoBehaviour
{

    public float Life { get; set; }
    public float Damage { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TryAttack();
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
