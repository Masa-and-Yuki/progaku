using UnityEngine;

public class WarningEffect : MonoBehaviour
{
    public float duration = 1.5f; // warningTime と同じ値
    private Material mat;
    private float timer = 0f;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        Color c = mat.color;
        c.a = 0f; // 最初は透明
        mat.color = c;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        // 徐々に赤く・濃くなる
        Color c = mat.color;
        c.r = 1f;
        c.g = Mathf.Lerp(1f, 0f, t); // 白→赤
        c.b = Mathf.Lerp(1f, 0f, t);
        c.a = Mathf.Lerp(0.2f, 0.8f, t); // 透明→濃い赤
        mat.color = c;
    }
}
