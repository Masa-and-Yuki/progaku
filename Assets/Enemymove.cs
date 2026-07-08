using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isBoss = false;
    public int maxHP = 50;
    public int currentHP;
    public int attackPower = 10;
    public EnemyAttack attackLogic;
    public float attackInterval = 1.5f;
    public bool wasHit = false;
    float attackTimer = 0f;

    void Start()
    {
        if (isBoss)
        {
            maxHP = 300;
            attackPower = 40;
        }

        currentHP = maxHP;

        if (attackLogic == null)
        {
            attackLogic = GetComponent<EnemyAttack>();
        }
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer < attackInterval)
        {
            return;
        }

        Player player = FindAnyObjectByType<Player>();
        if (attackLogic == null || player == null)
        {
            Debug.LogWarning(gameObject.name + " cannot attack because attackLogic or player is missing.");
            attackTimer = 0f;
            return;
        }

        StartCoroutine(attackLogic.ExecuteAttack(this, player));
        attackTimer = 0f;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log(gameObject.name + " took damage: " + damage);
        Debug.Log(gameObject.name + " HP: " + currentHP);
        wasHit = true;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isBoss)
        {
            Debug.Log("Boss defeated");
        }
        else
        {
            Debug.Log("Enemy defeated");
        }

        Destroy(gameObject);
    }
}
