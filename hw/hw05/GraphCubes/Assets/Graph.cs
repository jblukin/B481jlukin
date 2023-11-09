using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Graph : MonoBehaviour
{

    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(1, 100)]
    int resolution = 10;

    Transform[] points;

    void Awake()
    {

        Vector3 position = Vector3.zero;
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        points = new Transform[resolution];


        for (int i = 0; i < points.Length; i++)
        {

            Transform point = points[i] = Instantiate(pointPrefab);
            position.x = (i + 0.5f) * step - 1f;
            point.localPosition = position;

            Vector3[] vertices = point.GetComponent<MeshFilter>().mesh.vertices;

            Debug.Log(vertices[3] - vertices[0]);


            Transform parent = Instantiate(point, point.position, Quaternion.Euler(-45f, -45f, 0f));

            point.rotation = Quaternion.Euler(45f, 45f, 0f);

            point.localScale = scale;

            point.SetParent(parent, false);

            parent.gameObject.GetComponent<MeshRenderer>().enabled = false;

            parent.localScale = new(2f, 1f, 1f);


        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float time = Time.time;
        for (int i = 0; i < points.Length; i++)
        {

            Transform point = points[i];
            Vector3 position = point.localPosition;
            position.y = Mathf.Sin(Mathf.PI * (position.x /*+ time*/));
            point.localPosition = position;

            //point.Rotate(point.forward, position.y);

        }

    }
}
