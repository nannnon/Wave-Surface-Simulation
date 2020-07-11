using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class WaveSurfaceController : MonoBehaviour
{
    public const int kWidth = 101;
    public const int kHeight = kWidth;
    public const float kGap = 1f;

    Mesh m_mesh;
    Vector3[] m_vertices;
    Color[] m_colors;


    // Start is called before the first frame update
    void Start()
    {
        m_mesh = new Mesh();

        // 頂点
        m_vertices = new Vector3[kWidth * kHeight];
        for (int i = 0; i < kWidth; ++i)
        {
            for (int j = 0; j < kHeight; ++j)
            {
                Vector3 v = new Vector3(i * kGap, 0, j * kGap);
                m_vertices[XYtoI(i, j)] = v;
            }
        }

        // インデックス
        var triangles = new int[6 * (kWidth - 1) * (kHeight - 1)];
        {
            int index = 0;

            for (int i = 0; i < kWidth - 1; ++i)
            {
                for (int j = 0; j < kHeight - 1; ++j)
                {
                    int vbi0 = kWidth * j + i;
                    int vbi1 = vbi0 + 1;
                    int vbi2 = vbi1 + kWidth;
                    int vbi3 = vbi2 - 1;

                    triangles[index    ] = vbi0;
                    triangles[index + 1] = vbi3;
                    triangles[index + 2] = vbi1;

                    triangles[index + 3] = vbi3;
                    triangles[index + 4] = vbi2;
                    triangles[index + 5] = vbi1;

                    index += 6;
                }
            }
        }

        // 頂点カラー
        m_colors = new Color[kWidth * kHeight];
        for (int i = 0; i < (kWidth * kHeight); ++i)
        {
            m_colors[i] = Color.white;
        }

        // 法線
        var normals = new Vector3[kWidth * kHeight];
        for (int i = 0; i < kWidth * kHeight; ++i)
        {
            normals[i] = Vector3.up;
        }

        m_mesh.vertices = m_vertices;
        m_mesh.triangles = triangles;
        m_mesh.colors = m_colors;
        m_mesh.normals = normals;

        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = m_mesh;


        // コライダーを合わせる
        var collider = GetComponent<BoxCollider>();

        Vector3 center = new Vector3(
            kWidth / 2f,
            0,
            kHeight / 2f);
        collider.center = center;

        Vector3 size = new Vector3(
            kWidth * kGap,
            0.1f,
            kHeight * kGap);
        collider.size = size;
    }

    // Update is called once per frame
    void Update()
    {

    }

    int XYtoI(int x, int y)
    {
        return (x + y * kWidth);
    }

    public void Set(int i, int j, float y, Color color)
    {
        m_vertices[XYtoI(i, j)].y = y;
        m_colors[XYtoI(i, j)] = color;
    }

    public void Apply()
    {
        m_mesh.vertices = m_vertices;
        m_mesh.colors = m_colors;
        m_mesh.RecalculateNormals();
    }
}
