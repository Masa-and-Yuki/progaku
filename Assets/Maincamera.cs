using UnityEngine;

/// <summary>プレイヤーを少し遅れて追従するカメラ。</summary>
public class Maincamera : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        // 移動後のプレイヤー位置を使うため、通常の Update より後に追従処理を行う。
        if (target == null)
            return;

        // 目標位置との差を線形補間して、カメラ移動を滑らかにする。
        Vector3 desiredPos = target.position + new Vector3(0, 10, -10);
        transform.position = Vector3.Lerp(transform.position, desiredPos, 0.1f);
        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }
}
