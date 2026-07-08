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

    // 近接攻撃（半径2f）
    IEnumerator CloseRangeAttack(Enemy enemy, Player player)
    {
        float radius = 3f;

        // ボスの真下に攻撃位置を設定
        Vector3 pos = enemy.transform.position + new Vector3(0, effectHeight, 0);

        // --- Warning（予兆） ---
        GameObject warn = Instantiate(warningPrefab, pos, Quaternion.identity);
        warn.SetActive(true);

        // 半径2f → 直径4f の円
        warn.transform.localScale = new Vector3(radius * 2, 0.1f, radius * 2);

        yield return new WaitForSeconds(warningTime);

        Destroy(warn);

        // --- Hitbox（攻撃本体） ---
        GameObject hit = Instantiate(hitboxPrefab, pos, Quaternion.identity);
        hit.SetActive(true);

        hit.transform.localScale = new Vector3(radius * 2, 1f, radius * 2);

        // ダメージ判定
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
        for (int i = 0; i < attackCount; i++)
        {
            if (enemy == null || player == null)
                yield break;

            // ★ プレイヤーが攻撃してきたら近接攻撃を優先
            if (enemy.wasHit)
            {
                enemy.wasHit = false; // フラグリセット
                yield return StartCoroutine(CloseRangeAttack(enemy, player));
                continue;
            }

            // ★ ここからは今までのランダム攻撃
            Vector3 pos = enemy.transform.position;
            pos += new Vector3(Random.Range(-spawnRange, spawnRange), effectHeight, Random.Range(-spawnRange, spawnRange));

            GameObject warn = Instantiate(warningPrefab, pos, Quaternion.identity);
            warn.transform.localScale = new Vector3(attackRadius * 2, 0.1f, attackRadius * 2);

            yield return new WaitForSeconds(warningTime);
            Destroy(warn);

            GameObject hit = Instantiate(hitboxPrefab, pos, Quaternion.identity);
            hit.transform.localScale = new Vector3(attackRadius * 2, 1f, attackRadius * 2);

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
