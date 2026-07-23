using UnityEngine;

/// <summary>ボス攻撃の予告エフェクトを、時間とともに赤く濃く表示する。</summary>
public class WarningEffect : MonoBehaviour
{
    public float duration = 1.5f;
    Material mat;
    float timer;

    void Start()
    {
        // 開始時は透明にして、Update で徐々に見えるようにする。
        mat = GetComponent<MeshRenderer>().material;
        Color c = mat.color;
        c.a = 0f;
        mat.color = c;
    }

    void Update()
    {
        // 経過時間を 0〜1 の進行率に変換し、色と透明度を補間する。
        timer += Time.deltaTime;
        float t = timer / duration;
        Color c = mat.color;
        c.r = 1f;
        c.g = Mathf.Lerp(1f, 0f, t);
        c.b = Mathf.Lerp(1f, 0f, t);
        c.a = Mathf.Lerp(0.2f, 0.8f, t);
        mat.color = c;
    }
}
