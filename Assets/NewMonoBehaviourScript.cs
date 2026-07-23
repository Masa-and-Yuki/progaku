using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public GameObject enemyPrefab;

    void HandleAttackStart()
    {
        // プレイヤーの周囲にある Collider を取得し、Enemy を持つ相手だけを攻撃対象にする。
        float attackRange = 3.0f;
        Collider[] enemies = Physics.OverlapSphere(player.transform.position, attackRange);

        foreach (Collider col in enemies)
        {
            Enemy e = col.GetComponent<Enemy>();
            if (e != null)
            {
                //Debug.Log("Attack hit");
                player.HandleAttackInput(e);
            }
        }
    }

    void Update()
    {
        // 入力からプレイヤー状態を決定し、状態ごとの処理へ振り分ける。
        if (player == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.currentState = Playerstate.attack;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            player.currentState = Playerstate.Shield;
        }
        else
        {
            player.currentState = Playerstate.Normal;
        }

        // 状態に応じて移動・攻撃・防御の各入力処理を実行する。
        switch (player.currentState)
        {
            case Playerstate.Normal:
                player.HandleNormalInput();
                break;

            case Playerstate.attack:
                HandleAttackStart();
                break;

            case Playerstate.Shield:
                player.HandleShieldInput();
                break;
        }
        
    }
}
