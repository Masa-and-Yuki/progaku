using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Text hpText;
    private Player player;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        if (player != null && hpText != null)
        {
            hpText.text = "HP: " + player.currentHP + " / " + player.maxHP;
        }
    }
}
