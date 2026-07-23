using UnityEngine;

/// <summary>正方形の床マップと、その外周を囲む壁を自動生成する。</summary>
public class Mapping : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject wallPrefab;
    public int size = 15;

    void Start()
    {
        // 床を先に作り、その後に境界となる壁を配置する。
        GenerateSquareMap();
        GenerateWalls();
    }

    void GenerateSquareMap()
    {
        // 中心から左右・前後へ同じ距離だけ走査し、タイルを格子状に並べる。
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

    void GenerateWalls()
    {
        // 四辺に壁を並べて、生成したマップの外周を囲む。
        int half = size / 2;
        for (int x = -half; x <= half; x++)
        {
            Instantiate(wallPrefab, new Vector3(x, -1f, -half), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(x, -1f, half), Quaternion.identity);
        }

        for (int z = -half; z <= half; z++)
        {
            Instantiate(wallPrefab, new Vector3(-half, -1f, z), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(half, -1f, z), Quaternion.identity);
        }
    }
}
