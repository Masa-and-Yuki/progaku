using UnityEngine;
using System.Collections;

public class BossEnemyAttack : EnemyAttack
{
    public GameObject warningPrefab;
    public GameObject hitboxPrefab;
    public float warningTime = 1.5f;
    public float hitboxTime = 1.0f;
    public float attackRadius = 10f;
    public float spawnRange = 10f;
    public float effectHeight = 1.0f;
    public int attackCount = 5;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // ボスの足元へ範囲攻撃を出す。予告表示の後、範囲内のプレイヤーにダメージを与える。
    IEnumerator CloseRangeAttack(Enemy enemy, Player player)
    {
        float radius = 3f;
        // エフェクトが地面に埋まらないよう、ボスの位置へ高さを加える。
        Vector3 pos = enemy.transform.position + new Vector3(0, effectHeight, 0);

        // 攻撃範囲を知らせる予告エフェクトを表示する。
        GameObject warn = Instantiate(warningPrefab, pos, Quaternion.identity);
        warn.SetActive(true);
        // 直径で拡大するため、半径を 2 倍して大きさを合わせる。
        warn.transform.localScale = new Vector3(radius * 2, 0.1f, radius * 2);
        yield return new WaitForSeconds(warningTime);
        Destroy(warn);

        // 予告時間後に実際の攻撃判定用エフェクトへ切り替える。
        GameObject hit = Instantiate(hitboxPrefab, pos, Quaternion.identity);
        hit.SetActive(true);
        hit.transform.localScale = new Vector3(radius * 2, 1f, radius * 2);

        // 中心からの距離を判定し、攻撃範囲内なら一度だけダメージを与える。
        float dist = Vector3.Distance(player.transform.position, pos);
        if (dist <= radius)
        {
            player.TakeDamage(enemy.attackPower);
        }

        yield return new WaitForSeconds(hitboxTime);
        Destroy(hit);
    }

    public override IEnumerator ExecuteAttack(Enemy enemy, Player player)
    {
        // 指定回数ぶん、攻撃予告→判定→消去の流れを繰り返す。
        for (int i = 0; i < attackCount; i++)
        {
            if (enemy == null || player == null)
                yield break;

            // 被弾直後はランダム攻撃ではなく、足元への反撃を行う。
            if (enemy.wasHit)
            {
                anim.SetTrigger("Attack");
                enemy.wasHit = false; // 同じ被弾に対して反撃を繰り返さないようフラグを戻す。
                yield return StartCoroutine(CloseRangeAttack(enemy, player));
                continue;
            }

            // それ以外は、ボスを中心としたランダムな地点に範囲攻撃を出す。
            Vector3 pos = enemy.transform.position;
            pos += new Vector3(Random.Range(-spawnRange, spawnRange), effectHeight, Random.Range(-spawnRange, spawnRange));

            GameObject warn = Instantiate(warningPrefab, pos, Quaternion.identity);
            warn.transform.localScale = new Vector3(attackRadius * 2, 0.1f, attackRadius * 2);
            yield return new WaitForSeconds(warningTime);
            Destroy(warn);

            GameObject hit = Instantiate(hitboxPrefab, pos, Quaternion.identity);
            hit.transform.localScale = new Vector3(attackRadius * 2, 1f, attackRadius * 2);

            // 攻撃の瞬間に距離を判定し、範囲内ならダメージを与える。
            float dist = Vector3.Distance(player.transform.position, pos);
            if (dist <= attackRadius)
            {
                player.TakeDamage(enemy.attackPower);
            }

            yield return new WaitForSeconds(hitboxTime);
            Destroy(hit);
        }
    }
}
