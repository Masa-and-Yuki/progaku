using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isBoss = false;

    public int maxHP = 50;
    public int currentHP;
    public int attackPower = 10;

    public EnemyAttack attackLogic;
    public float attackInterval = 1.5f;
    float attackTimer = 0f;

    public bool wasHit = false;

    void Start()
    {
        if (isBoss)
        {
            maxHP = 300;
            attackPower = 20;
        }

        currentHP = maxHP;

        PrintHP(); // 最初にログ出す

        if (attackLogic == null)
        {
            attackLogic = GetComponent<EnemyAttack>();
        }
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer < attackInterval)
            return;

        Player player = FindAnyObjectByType<Player>();
        if (attackLogic == null || player == null)
        {
            attackTimer = 0f;
            return;
        }

        StartCoroutine(attackLogic.ExecuteAttack(this, player));
        attackTimer = 0f;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        wasHit = true;

        PrintHP(); // ログ出力

        if (currentHP <= 0)
        {
            GameManager.instance.EnemyDefeated();
            Destroy(gameObject);
        }
    }

    void PrintHP()
    {
        if (isBoss)
            Debug.Log("Boss HP: " + currentHP + " / " + maxHP);
        else
            Debug.Log("Enemy HP: " + currentHP + " / " + maxHP);
    }
}
