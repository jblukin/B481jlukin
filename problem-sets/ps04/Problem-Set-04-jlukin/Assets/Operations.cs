using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public enum ObjectMode
{

    Cam,
    Obj1

}

public enum Operation
{

    TranslateXY,
    TranslateXZ,
    Rotate,
    Scale

}
public class Operations : MonoBehaviour
{

    ObjectMode ObjectMode = ObjectMode.Obj1;

    Operation Operation = Operation.TranslateXZ;

    GameObject Obj1;

    Vector3 prevCamCoords, CamCoords, prevObj1Coords, Obj1Coords;

    Vector3 startMousePos, deltaMousePos;

    Matrix4x4 ViewingMatrix, ModelMatrix;

    [SerializeField] Camera cam;

    [SerializeField, Range(0.01f, 0.1f)] float mouseSensitivity;

    // Start is called before the first frame update
    void Start()
    {

        Obj1 = FindObjectOfType<CreateCube>().gameObject;

        prevObj1Coords = Obj1.transform.position;

        Obj1Coords = prevObj1Coords;

        prevCamCoords = cam.transform.position;

        CamCoords = prevCamCoords;

        ViewingMatrix = Matrix4x4.identity;

        ModelMatrix = Matrix4x4.identity;

    }

    #region Button Methods
    public void OperatateCam() => SetMode(ObjectMode.Cam);

    public void OperateObject1() => SetMode(ObjectMode.Obj1);

    void SetMode(ObjectMode mode)
    {

        ObjectMode = mode;

    }

    public void TranslateXY() => SetOperation(Operation.TranslateXY);

    public void TranslateXZ() => SetOperation(Operation.TranslateXZ);

    public void Rotate() => SetOperation(Operation.Rotate);

    public void Scale() => SetOperation(Operation.Scale);

    void SetOperation(Operation op)
    {

        Operation = op;

    }
#endregion


    void GetMouseDeltas()
    {

        if (Input.GetMouseButtonDown(0))
        {

            startMousePos = Input.mousePosition;

            if (ObjectMode is ObjectMode.Cam) prevCamCoords = CamCoords;

            else if (ObjectMode is ObjectMode.Obj1) prevObj1Coords = Obj1.transform.position;

        }
        if (Input.GetMouseButton(0))
        {

            deltaMousePos = mouseSensitivity * (Input.mousePosition - startMousePos);

            if (ObjectMode is ObjectMode.Cam) CamCoords = prevCamCoords + deltaMousePos;

            else if (ObjectMode is ObjectMode.Obj1) Obj1Coords = prevObj1Coords + deltaMousePos;

        }


    }

    Quaternion Rotation()
    {

        float radius = (Obj1.GetComponent<MeshFilter>().mesh.vertices[2] - Obj1.GetComponent<MeshFilter>().mesh.vertices[3]).magnitude/2;

        float dr = deltaMousePos.magnitude;

        Vector3 rotationAxis = new Vector3(-deltaMousePos.y / dr, deltaMousePos.x / dr, 0f).normalized;

        float angle = dr / radius;

        return Quaternion.AngleAxis(angle, rotationAxis);

    }

    void RotateCam()
    {

        cam.transform.rotation = Rotation();

    }

    void Operate()
    {

        Vector3 newCamCoords = CamCoords;

        if (ObjectMode is ObjectMode.Cam)
        {

            if (Operation is Operation.TranslateXY) newCamCoords = CamCoords;

            else if (Operation is Operation.TranslateXZ) newCamCoords = new(CamCoords.x, prevCamCoords.y, CamCoords.y);

            else if (Operation is Operation.Scale) newCamCoords = CamCoords;

            else if (Operation is Operation.Rotate) RotateCam(); //Couldn't get fully working

        }
        if (ObjectMode is ObjectMode.Obj1)
        {

            if (Operation is Operation.TranslateXY) ModelMatrix = Matrix4x4.Translate(new(Obj1Coords.x, Obj1Coords.y, prevObj1Coords.z));

            else if (Operation is Operation.TranslateXZ) ModelMatrix = Matrix4x4.Translate(new(Obj1Coords.x, prevObj1Coords.y, Obj1Coords.y));

            else if (Operation is Operation.Rotate) ModelMatrix = Matrix4x4.Rotate(Rotation());

            else if (Operation is Operation.Scale) ModelMatrix = Matrix4x4.Scale(Vector3.one * Mathf.Max(1f, deltaMousePos.magnitude));

        }

        if(ObjectMode is not ObjectMode.Cam || (ObjectMode is ObjectMode.Cam && Operation is not Operation.Rotate)) Camera.main.transform.position = CamCoords = newCamCoords;

        ViewingMatrix = Camera.main.worldToCameraMatrix;

        Obj1.GetComponent<CreateCube>().material.SetMatrix("_ViewingMatrix", ViewingMatrix);

        Obj1.GetComponent<CreateCube>().material.SetMatrix("_ModelMatrix", ModelMatrix);

    }

    // Update is called once per frame
    void Update()
    {

        GetMouseDeltas();

        Operate();

    }
}
