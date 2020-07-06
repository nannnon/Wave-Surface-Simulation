using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class WaveSurfaceController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var mesh = new Mesh();
        mesh.vertices = new Vector3[]
        {
            new Vector3(0f, 1f),
            new Vector3(1f, -1f),
            new Vector3(-1f, -1f)
        };

        mesh.triangles = new int[]
        {
            0, 1, 2
        };

        mesh.colors = new Color[]
        {
            Color.white,
            Color.red,
            Color.green
        };

        //mesh.RecalculateNormals();

        var filter = GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
