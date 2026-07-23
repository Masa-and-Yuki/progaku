using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");

        // 2秒後にタイトルへ戻る
        Invoke("ReturnToTitle", 2f);
    }

    void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");  // ← タイトルシーン名に変更してね
    }

    public void EnemyDefeated()
    {
        Debug.Log("Enemy Defeated");
    }
}
