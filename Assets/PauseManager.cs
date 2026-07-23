using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>ポーズ画面の表示とゲーム時間の停止・再開を管理する。</summary>
public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    bool isPaused;

    public void TogglePause()
    {
        // 状態に合わせてパネル表示と Time.timeScale を同時に切り替える。
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void Update()
    {
        // Escape キーでポーズの切り替えを受け付ける。
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void RestartGame()
    {
        // 時間停止を解除してから現在のシーンを再読み込みする。
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
