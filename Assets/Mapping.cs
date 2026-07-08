using UnityEngine;

public class Mapping : MonoBehaviour
{
    public GameObject tilePrefab;   // 地面タイル
    public GameObject wallPrefab;   // 壁のプレハブ
    public int size = 15;           // フィールドの一辺の長さ

    void Start()
    {
        GenerateSquareMap();
        GenerateWalls();
    }

    // ■のフィールドを生成（中心が0,0,0）
    void GenerateSquareMap()
    {
        int half = size / 2;

        for (int x = -half; x <= half; x++)
        {
            for (int z = -half; z <= half; z++)
            {
                Vector3 pos = new Vector3(x, -1, z);
                Instantiate(tilePrefab, pos, Quaternion.identity);
            }
        }
    }

    // 外周に壁を生成（中心が0,0,0）
    void GenerateWalls()
    {
        int half = size / 2;

        for (int x = -half; x <= half; x++)
        {
            Instantiate(wallPrefab, new Vector3(x, -1f, -half), Quaternion.identity); // 下
            Instantiate(wallPrefab, new Vector3(x, -1f,  half), Quaternion.identity); // 上
        }

        for (int z = -half; z <= half; z++)
        {
            Instantiate(wallPrefab, new Vector3(-half, -1f, z), Quaternion.identity); // 左
            Instantiate(wallPrefab, new Vector3( half, -1f, z), Quaternion.identity); // 右
        }
    }
}
