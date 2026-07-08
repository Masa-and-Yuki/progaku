using UnityEngine;
using System.Collections;

public abstract class EnemyAttack : MonoBehaviour
{
    public abstract IEnumerator ExecuteAttack(Enemy enemy, Player player);
}
