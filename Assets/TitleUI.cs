using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>タイトル画面の UI 操作を担当する。</summary>
public class TitleUI : MonoBehaviour
{
    public void StartGame()
    {
        // 開始ボタンからゲーム本編のシーンへ遷移する。
        SceneManager.LoadScene("GameScene");
    }
}
