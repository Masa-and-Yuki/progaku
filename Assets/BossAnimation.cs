using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("usually"); // 常時アニメを再生
    }

    void Update()
    {
        // 攻撃タイミング（例：一定時間ごと）
        if (Time.frameCount % 200 == 0) // 200フレームごとに攻撃
        {
            anim.SetTrigger("Attack");
        }
    }
}
