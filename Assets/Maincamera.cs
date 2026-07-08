using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform target; // プレイヤー

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPos = target.position + new Vector3(0, 10, -10);
            transform.position = Vector3.Lerp(transform.position, desiredPos, 0.1f);
            transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        }
    }


}
