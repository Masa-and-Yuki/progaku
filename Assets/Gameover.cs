using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>ゲームオーバー画面のボタン操作を担当する。</summary>
public class Gameover : MonoBehaviour
{
    public void Retry()
    {
        // 停止していたゲーム時間を戻してから、ゲームシーンを読み直す。
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void BackToTitle()
    {
        // タイトル画面でも時間が進むようにしてから遷移する。
        Time.timeScale = 1f;
        SceneManager.LoadScene("title");
    }
}
