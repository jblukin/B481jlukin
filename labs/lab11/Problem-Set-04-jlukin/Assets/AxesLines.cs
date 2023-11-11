using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxesLines : MonoBehaviour
{

    public Material lineMaterial;

    public Transform xAxis, yAxis, zAxis;

    // Start is called before the first frame update
    void Start()
    {
        
        //Unit Lines -- X Axis
        for (int x = 1; x <= 10; x++) 
        {

            GameObject go = new GameObject();

            LineRenderer line = go.AddComponent<LineRenderer>();

            line.startWidth = 0.1f;

            line.endWidth = 0.1f;

            line.SetPosition(0, new(x, 0, 0));

            line.SetPosition(1, new(x, 1, 0));

            line.startColor = Color.red;

            line.endColor = Color.red;

            line.material = lineMaterial;

            Instantiate(go, xAxis);

            Destroy(go);

        }

        for (int x = 1; x <= 10; x++)
        {

            GameObject go = new GameObject();

            LineRenderer line = go.AddComponent<LineRenderer>();

            line.startWidth = 0.1f;

            line.endWidth = 0.1f;

            line.SetPosition(0, new(x, 0, 0));

            line.SetPosition(1, new(x, 0, 1));

            line.startColor = Color.red;

            line.endColor = Color.red;

            line.material = lineMaterial;

            Instantiate(go, xAxis);

            Destroy(go);

        }
        //


        //Unit Lines -- Y Axis
        for (int y = 1; y <= 10; y++)
        {

            GameObject go = new GameObject();

            LineRenderer line = go.AddComponent<LineRenderer>();

            line.startWidth = 0.1f;

            line.endWidth = 0.1f;

            line.SetPosition(0, new(0, y, 0));

            line.SetPosition(1, new(1, y, 0));

            line.startColor = Color.green;

            line.endColor = Color.green;

            line.material = lineMaterial;

            Instantiate(go, yAxis);

            Destroy(go);

        }

        for (int y = 1; y <= 10; y++)
        {

            GameObject go = new GameObject();

            LineRenderer line = go.AddComponent<LineRenderer>();

            line.startWidth = 0.1f;

            line.endWidth = 0.1f;

            line.SetPosition(0, new(0, y, 0));

            line.SetPosition(1, new(0, y, 1));

            line.startColor = Color.green;

            line.endColor = Color.green;

            line.material = lineMaterial;

            Instantiate(go, yAxis);

            Destroy(go);

        }
        //


        //Unit Lines -- Z Axis
        for (int z = 1; z <= 10; z++)
        {

            GameObject go = new GameObject();

            LineRenderer line = go.AddComponent<LineRenderer>();

            line.startWidth = 0.1f;

            line.endWidth = 0.1f;

            line.SetPosition(0, new(0, 0, z));

            line.SetPosition(1, new(1, 0, z));

            line.startColor = Color.blue;

            line.endColor = Color.blue;

            line.material = lineMaterial;

            Instantiate(go, zAxis);

            Destroy(go);

        }

        for (int z = 1; z <= 10; z++)
        {

            GameObject go = new GameObject();

            LineRenderer line = go.AddComponent<LineRenderer>();

            line.startWidth = 0.1f;

            line.endWidth = 0.1f;

            line.SetPosition(0, new(0, 0, z));

            line.SetPosition(1, new(0, 1, z));

            line.startColor = Color.blue;

            line.endColor = Color.blue;

            line.material = lineMaterial;

            Instantiate(go, zAxis);

            Destroy(go);

        }
        //

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
