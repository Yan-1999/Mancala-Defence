public class Enemy : Entity
{
    protected override void OnDeath()
    {
        GameManager.Instance.EnemyDeath(this);
    }

    protected override void TryAttack()
    {
        // UNDONE: More details in enemy attack.
        GameManager.Instance.EnemyAttack(this);
    }
}
