using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Player player;
    public Enemy enemy;
    public GameObject enemyPrefab;

    void HandleAttackStart()
    {
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
