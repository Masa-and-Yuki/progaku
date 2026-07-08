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
    public int maxHP = 100;
    public int currentHP = 100;
    public int attackPower = 20;
    public Playerstate currentState = Playerstate.Normal;
    bool isDead = false;
    public float attackCooldown = 1.0f; // 攻撃のリキャストタイム
    private bool canAttack = true;      // 攻撃可能かどうか

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        currentState = Playerstate.Normal;
    }


    public void HandleNormalInput()
    {   
        if (currentState == Playerstate.Shield)
        {return;}
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
         Vector3 moveDir = new Vector3(x, 0, z);
        // 移動処理
        transform.Translate(moveDir * Movespeed * Time.deltaTime, Space.World);

        // 向きを移動方向に合わせる
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }
    }
    
    void Update()
    {
        // 毎フレーム入力を監視
        if (currentState == Playerstate.Normal)
        {
            HandleNormalInput();
        }
    }
    

    public void HandleParliInput()
    {
    }

    public void HandleAttackInput(Enemy enemy)
    {
        if (!canAttack)
            return;

        if (enemy != null)
        {
            // 敵との方向ベクトル
            Vector3 toEnemy = enemy.transform.position - transform.position;

            // プレイヤーの前方向との角度を計算
            float angle = Vector3.Angle(transform.forward, toEnemy);

            // 攻撃範囲（手前120°）内ならダメージを与える
            if (angle <= 60f) // 120°の半分（左右60°）
            {
                enemy.TakeDamage(attackPower);
                Debug.Log("Enemy hit! Angle: " + angle);
            }
            else
            {
                Debug.Log("Enemy out of range. Angle: " + angle);
            }
        }

        // クールダウン開始
        canAttack = false;
        currentState = Playerstate.attack;
        StartCoroutine(AttackCooldownRoutine());
    }


    public void HandleShieldInput()
    {
    }

    public void HandledodgeInput()
    {
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        int finalDamage = damage;

        if (currentState == Playerstate.Shield)
        {
            finalDamage = Mathf.RoundToInt(damage - 30);
            Debug.Log("Shield reduced damage: " + finalDamage);
        }

        currentHP = Mathf.Max(0, currentHP - finalDamage);
        Debug.Log("Player took damage: " + finalDamage + " HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        Debug.Log("Player died");
        Destroy(gameObject);
    }
}
