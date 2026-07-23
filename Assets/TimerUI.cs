using UnityEngine;
using UnityEngine.UI;

/// <summary>経過時間を秒単位で UI に表示する。</summary>
public class TimerUI : MonoBehaviour
{
    public Text timerText;
    public float timeCount;

    void Update()
    {
        // フレームごとの経過時間を足し、秒未満を切り捨てて文字列へ変換する。
        timeCount += Time.deltaTime;
        timerText.text = Mathf.Floor(timeCount).ToString();
    }
}
