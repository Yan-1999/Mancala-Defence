public class Enemy : Entity
{
    protected override void OnDeath()
    {
        GameManager.Instance.EnemyDeath(this);
    }

    protected override void TryAttack()
    {
        // TODO
        GameManager.Instance.EnemyAttack(this);
    }
}
