using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug : MonoBehaviour
{

    void Start()
    {
        Vector3[] V = GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i < V.Length; i++)
        {
            UnityEngine.Debug.Log("Vertex " + i + ": " + V[i]);
        }

        int[] T = GetComponent<MeshFilter>().mesh.triangles;
        for (int i = 0; i < T.Length; i++)
        {
            UnityEngine.Debug.Log("Triangle " + Mathf.CeilToInt(i/3) + ": " + T[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
