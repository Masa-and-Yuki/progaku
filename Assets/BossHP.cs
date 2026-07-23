using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    public Text hpText;
    private Enemy boss;

    void Start()
    {
        boss = FindAnyObjectByType<Enemy>();
    }

    void Update()
    {
        if (boss != null && hpText != null)
        {
            hpText.text = "Boss HP: " + boss.currentHP + " / " + boss.maxHP;
        }
    }
}
