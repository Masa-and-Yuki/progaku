using UnityEngine;
using System.Collections;

public class NormalEnemyAttack : EnemyAttack
{
    public float range = 2f;
    public int damage = 10;

    public override IEnumerator ExecuteAttack(Enemy enemy, Player player)
    {
        float dist = Vector3.Distance(enemy.transform.position, player.transform.position);

        if (dist <= range)
        {
            player.TakeDamage(damage);
        }
        yield return null;
    }
}
