using UnityEngine;
using System.Collections;

public enum Playerstate
{
    Normal,
    attack,
    parli,
    Shield,
    dodge
}

public class Player : MonoBehaviour
{
    public float Movespeed = 5f;

    public int maxHP = 60;
    public int currentHP = 60;

    public int attackPower = 20;
    public Playerstate currentState = Playerstate.Normal;
    bool isDead = false;
    public float attackCooldown = 1.0f;
    private bool canAttack = true;

    public float shieldDuration = 2.0f;
    public int shieldReduction = 30;
    private bool isShielding = false;

    void Start()
    {
        PrintHP(); // 最初にログ出す
    }

    void Update()
    {
        if (currentState == Playerstate.Normal)
        {
            HandleNormalInput();
        }
    }

    public void HandleNormalInput()
    {
        if (currentState == Playerstate.Shield)
            return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, 0, z);
        transform.Translate(moveDir * Movespeed * Time.deltaTime, Space.World);

        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }
    }

    public void HandleAttackInput(Enemy enemy)
    {
        if (!canAttack)
            return;

        if (enemy != null)
        {
            Vector3 toEnemy = enemy.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, toEnemy);

            if (angle <= 60f)
            {
                enemy.TakeDamage(attackPower);
            }
        }

        canAttack = false;
        currentState = Playerstate.attack;
        StartCoroutine(AttackCooldownRoutine());
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        currentState = Playerstate.Normal;
    }

    public void HandleShieldInput()
    {
        if (isShielding)
            return;

        StartCoroutine(ShieldRoutine());
    }

    private IEnumerator ShieldRoutine()
    {
        isShielding = true;
        currentState = Playerstate.Shield;

        yield return new WaitForSeconds(shieldDuration);

        isShielding = false;
        currentState = Playerstate.Normal;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        int finalDamage = isShielding ? Mathf.Max(0, damage - shieldReduction) : damage;

        currentHP = Mathf.Max(0, currentHP - finalDamage);

        PrintHP(); // ログ出力

        if (currentHP <= 0)
        {
            isDead = true;
            GameManager.instance.GameOver();
            Destroy(gameObject);
        }
    }

    void PrintHP()
    {
        Debug.Log("Player HP: " + currentHP + " / " + maxHP);
    }
}
