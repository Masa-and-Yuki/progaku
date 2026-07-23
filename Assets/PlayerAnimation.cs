using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsFlying", true);
    }

    void Update()
    {
        // 攻撃（左クリック）
        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }

        // 防御（右クリック押す）
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetBool("IsGuard", true);
        }

        // 防御解除（右クリック離す）
        if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("IsGuard", false);
        }
    }
}
