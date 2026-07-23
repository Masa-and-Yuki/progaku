using UnityEngine;
using System.Collections;

public abstract class EnemyAttack : MonoBehaviour
{
    // 敵ごとの攻撃処理をコルーチンとして実装するための共通メソッド。
    public abstract IEnumerator ExecuteAttack(Enemy enemy, Player player);
}
