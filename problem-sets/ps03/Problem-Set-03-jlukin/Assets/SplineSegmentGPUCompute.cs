/*  CSCI-B481/B581 - Fall 2023 - Mitja Hmeljak
	Problem Set 03 starter C# program code
	This script needs to:
	prepare a meshRenderer and connect it to a Material.
	The Material will be implemented in a Vertex Shader,
	to calculate (on the GPU) the vertices on a single Spline Curve segment,
	to be displayed as a Mesh, using a Mesh Renderer.
	Original demo code by CSCI-B481 alumnus Rajin Shankar, IU Computer Science.
 */

using System.Net;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;


public class SplineSegmentGPUCompute : MonoBehaviour
{

    // specify the name of the Vertex Shader to be used:
    private const string shaderName = "SplineVertexShader";

    // control points for a single Spline Curve segment:
    [SerializeField] private Transform control0, control1, control2, control3;
    // choice of Spline Curve type:
    [SerializeField] private SplineParameters.SplineType splineType;
    // only one line renderer: the control polyline:
    [SerializeField] private LineRenderer controlPolyLine;

    // what color should the Spline Curve be?
    [SerializeField] private Color splineColor = new Color(255f / 255f, 255f / 255f, 0f / 255f);

    // how wide should the Spline Curve be?
    [SerializeField] private float splineWidth = 0.1f;

    // how many vertices on the spline curve?
    //   (the more vertices you set, the smoother the curve will be)
    [Range(8, 512)][SerializeField] private int verticesOnCurve = 64;


    // the Spline Curve will be rendered by a MeshRenderer,
    //   (and the vertices for the Mesh Renderer
    //   will be computed in our Vertex Shader)
    private MeshRenderer meshRenderer;

    // The Mesh Filter is meant to take a mesh from assets
    //    and pass it to the Mesh Renderer for rendering on the screen.
    // However, we create the mesh in this script,
    //    before the Mesh Filter passes it to the Mesh Renderer:
    private MeshFilter meshFilter;

    // the Vertex Shader will be considered a "Material" for rendering purposes:
    private Material material;

    // the Mesh to be rendered:
    private Mesh mesh;

    private bool _showDerivatives;

    [SerializeField, Range(5, 10)] private int derivativeLineLength;

    public void SetType(SplineParameters.SplineType type)
    {
        splineType = type;
    }

    public void UseBezier() => SetType(SplineParameters.SplineType.Bezier);

    public void UseCatmullRom() => SetType(SplineParameters.SplineType.CatmullRom);

    public void UseB() => SetType(SplineParameters.SplineType.Bspline);

    public void DerivativesToggle() => _showDerivatives = !_showDerivatives;

    [SerializeField] private LineRenderer control0First, control0Second, control1First, control1Second, control2First, control2Second, control3First, control3Second;

    // ---------------------------------------------------------
    // set up the renderer, the first time this object is instantiated in the scene:
    private void Awake()
    {

        _showDerivatives = true;

        // obtain Mesh Renderer and Mesh Filter components from Unity scene:
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        // find the vertex shader that will compute Spline curve vertices:
        material = new Material(Shader.Find(shaderName));

        // connect our MeshRenderer to the Vertex Shader:
        meshRenderer.material = material;

        // instantiate required vertices and triangles for the Mesh:
        Vector3[] vertices = new Vector3[verticesOnCurve * 2];
        int[] triangles = new int[verticesOnCurve * 6 - 6];

        for (int i = 0; i < verticesOnCurve; i++)
        {


            // parameter for vertices on "base spline curve":
            float t1 = (float)i / (float)(verticesOnCurve - 1);

            // parameter for vertices on "offset spline curve":
            float t2 = (float)i / (float)(verticesOnCurve - 1);

            // the "trick" is to pass the parameters t1 and t2
            //   (for Spline Curve computation in the Vertex Shader)
            // as .x components in the vertices.

            // we also use the .y components to pass another value
            //   used to compute the "offset spline curve" vertices (see below)

            // the Vertex Shader will receive the t1, t2 parameters
            // and use t1, t2 values to compute the position of each
            // vertex on the Spline Curve.

            // vertices on "base spline curve":
            vertices[2 * i].x = t1;
            vertices[2 * i].y = 0;

            // vertices on "offset spline curve":
            vertices[2 * i + 1].x = t2;
            vertices[2 * i + 1].y = splineWidth;

            if (i < verticesOnCurve - 1)
            {

                // triangle with last side on "base spline curve"
                // i.e. vertex 2 to vertex 0:
                triangles[6 * i] = 2 * i;
                triangles[6 * i + 1] = 2 * i + 1;
                triangles[6 * i + 2] = 2 * i + 2;

                // triangle with one side on "offset spline curve"
                // i.e. vertex 1 to vertex to vertex 3:
                triangles[6 * i + 3] = 2 * i + 1;
                triangles[6 * i + 4] = 2 * i + 3;
                triangles[6 * i + 5] = 2 * i + 2;
            }
        }
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        meshFilter.mesh = mesh;
        meshRenderer.sortingLayerName = "Default";
        meshRenderer.sortingOrder = 1;

    } // end of Awake()

    // ---------------------------------------------------------
    private void Update()
    {
        Matrix4x4 splineMatrix = SplineParameters.GetMatrix(splineType);

        // pass all necessary variables to the Vertex Shader:
        // (note: in GLSL these would be considered "uniform" variables)
        //
        // spline matrix in Hermite form:
        material.SetMatrix("_SplineMatrix", splineMatrix);
        // control points for spline curve rendering:
        material.SetVector("_Control0", control0.position);
        material.SetVector("_Control1", control1.position);
        material.SetVector("_Control2", control2.position);
        material.SetVector("_Control3", control3.position);
        // step between subsequent t parameter values for curve:
        float step = (float)1.0 / (float)(verticesOnCurve - 1);
        material.SetFloat("_Step", step);
        // color to be passed to the Fragment Shader:
        material.SetColor("_Color", splineColor);


        // to draw the enclosing polyLine, set control line points:
        //
        controlPolyLine.SetPosition(0, control0.position);
        controlPolyLine.SetPosition(1, control1.position);
        controlPolyLine.SetPosition(2, control2.position);
        controlPolyLine.SetPosition(3, control3.position);

        ShowDerivatives();

    } // end of Update()

    private void ShowDerivatives()
    {

        //float t = control.position.x; //according to the shader

        control0First.enabled = _showDerivatives;
        control0Second.enabled = _showDerivatives;
        control1First.enabled = _showDerivatives;
        control1Second.enabled = _showDerivatives;
        control2First.enabled = _showDerivatives;
        control2Second.enabled = _showDerivatives;
        control3First.enabled = _showDerivatives;
        control3Second.enabled = _showDerivatives;

        if (splineType is SplineParameters.SplineType.Bezier)
        {
            //FIRST CONTROL POINT

            float control0_t = 0;

            Vector3 control0firstD = 3f * ((control3.position - 3f * control2.position + 3f * control1.position - control0.position) * Mathf.Pow(control0_t, 2)
                + (2f * control2.position - 4f * control1.position + 2f * control0.position) * control0_t + control1.position - control0.position);

            control0First.SetPosition(0, control0.position);

            control0First.SetPosition(1, control0.position + control0firstD.normalized * derivativeLineLength);

            Vector3 control0SecondD = 6f * ((control3.position - 3f * control2.position + 3f * control1.position - control0.position) * control0_t
                + control2.position - 2f * control1.position + control0.position);

            control0Second.SetPosition(0, control0.position);

            control0Second.SetPosition(1, control0.position + control0SecondD.normalized * derivativeLineLength);

            //END FIRST CONTROL POINT

            //SECOND CONTROL POINT

            float control1_t = 1 / 3f;

            Vector3 control1firstD = 3f * ((control3.position - 3f * control2.position + 3f * control1.position - control0.position) * Mathf.Pow(control1_t, 2)
                + (2f * control2.position - 4f * control1.position + 2f * control0.position) * control1_t + control1.position - control0.position);

            control1First.SetPosition(0, control1.position);

            control1First.SetPosition(1, control1.position + control1firstD.normalized * derivativeLineLength);

            Vector3 control1SecondD = 6f * ((control3.position - 3f * control2.position + 3f * control1.position - control0.position) * control1_t
                + control2.position - 2f * control1.position + control0.position);

            control1Second.SetPosition(0, control1.position);

            control1Second.SetPosition(1, control1.position + control1SecondD.normalized * derivativeLineLength);

            //END SECOND CONTROL POINT

            //THIRD CONTROL POINT

            float control2_t = 2 / 3f;

            Vector3 control2firstD = 3f * ((control3.position - 3f * control2.position + 3f * control1.position - control0.position) * Mathf.Pow(control2_t, 2)
                + (2f * control2.position - 4f * control1.position + 2f * control0.position) * control2_t + control1.position - control0.position);

            control2First.SetPosition(0, control2.position);

            control2First.SetPosition(1, control2.position + control2firstD.normalized * derivativeLineLength);

            Vector3 control2SecondD = 6f * ((control3.position - 3f * control2.position + 3f * control1.position - control0.position) * control2_t
                + control2.position - 2f * control1.position + control0.position);

            control2Second.SetPosition(0, control2.position);

            control2Second.SetPosition(1, control2.position + control2SecondD.normalized * derivativeLineLength);

            //END THIRD CONTROL POINT

            //FOURTH CONTROL POINT

            float control3_t = 1;

            Vector3 control3firstD = 3f * ((control3.position - 3f * control2.position + 3f * control1.position - control0.position) * Mathf.Pow(control3_t, 2)
                + (2f * control2.position - 4f * control1.position + 2f * control0.position) * control3_t + control1.position - control0.position);

            control3First.SetPosition(0, control3.position);

            control3First.SetPosition(1, control3.position + control3firstD.normalized * derivativeLineLength);

            Vector3 control3SecondD = 6f * ((control3.position - 3f * control2.position + 3f * control1.position - control0.position) * control3_t
                + control2.position - 2f * control1.position + control0.position);

            control3Second.SetPosition(0, control3.position);

            control3Second.SetPosition(1, control3.position + control3SecondD.normalized * derivativeLineLength);

            //END FOURTH CONTROL POINT

        }

        else if (splineType is SplineParameters.SplineType.CatmullRom) 
        {

            //FIRST CONTROL POINT - NO DERIVATIVES HERE (NOT PART OF CURVE)

            control0First.enabled = false;

            control0Second.enabled = false;

            //END FIRST CONTROL POINT


            //SECOND CONTROL POINT

            float control1_t = 0;

            Vector3 control1firstD = 0.5f * (3f * Mathf.Pow(control1_t, 2) * (control3.position - 3f * control2.position + 3f * control1.position - control0.position)
                + (2f * control1_t * (-control3.position + 4f * control2.position - 5f * control1.position + 2f * control0.position))
                + control2.position - control0.position);

            control1First.SetPosition(0, control1.position);

            control1First.SetPosition(1, control1.position + control1firstD.normalized * derivativeLineLength);

            Vector3 control1SecondD = (3f * control1_t * (control3.position - 9f * control2.position + 9f * control1.position - 3f * control0.position)
                - (control3.position + 4f * control2.position - 5f * control1.position + 2f * control0.position));

            control1Second.SetPosition(0, control1.position);

            control1Second.SetPosition(1, control1.position + control1SecondD.normalized * derivativeLineLength);

            //END SECOND CONTROL POINT

            //THIRD CONTROL POINT

            float control2_t = 1;

            Vector3 control2firstD = 0.5f * (3f * Mathf.Pow(control2_t, 2) * (control3.position - 3f * control2.position + 3f * control1.position - control0.position)
                + (2f * control2_t * (-control3.position + 4f * control2.position - 5f * control1.position + 2f * control0.position))
                + control2.position - control0.position);

            control2First.SetPosition(0, control2.position);

            control2First.SetPosition(1, control2.position + control2firstD.normalized * derivativeLineLength);

            Vector3 control2SecondD = (control2_t * ((3f * control3.position) - (9f * control2.position) + (9f * control1.position) - (3f * control0.position))
                + (-control3.position + (4f * control2.position) - (5f * control1.position) + (2f * control0.position)));

            control2Second.SetPosition(0, control2.position);

            control2Second.SetPosition(1, control2.position + control2SecondD.normalized * derivativeLineLength);

            //END THIRD CONTROL POINT

            //FOURTH CONTROL POINT - NO DERIVATIVES HERE (NOT PART OF CURVE)

            control3First.enabled = false;

            control3Second.enabled = false;

            //END FOURTH CONTROL POINT


        }

        else if (splineType is SplineParameters.SplineType.Bspline) 
        {

            //FIRST CONTROL POINT

            control0First.enabled = false;

            control0Second.enabled = false;

            //END FIRST CONTROL POINT

            //FIRST POINT DERIVATIVES - USES CONTROL POINT 1 LINE RENDERERS

            float splineStart_t = 0;

            Vector3 startPosFirstD = 0.5f * (3f * Mathf.Pow(splineStart_t, 2) * (control3.position - 3f * control2.position + 3f * control1.position - control0.position)
                + (2f * splineStart_t * (-control3.position + 4f * control2.position - 5f * control1.position + 2f * control0.position))
                + control2.position - control0.position);

            control1First.SetPosition(0, control1.position);

            control1First.SetPosition(1, control1.position + startPosFirstD.normalized * derivativeLineLength);

            Vector3 startPosSecondD = (3f * splineStart_t * (control3.position - 9f * control2.position + 9f * control1.position - 3f * control0.position)
                + (-control3.position + 4f * control2.position - 5f * control1.position + 2f * control0.position));

            control1Second.SetPosition(0, control1.position);

            control1Second.SetPosition(1, control1.position + startPosSecondD.normalized * derivativeLineLength);

            //END FIRST POINT DERIVATIVES

            //LAST POINT DERIVATIVES - USES CONTROL POINT 2 LINE RENDERERS

            float splineEnd_t = 1;

            Vector3 endPosFirstD = 0.5f * (3f * Mathf.Pow(splineEnd_t, 2) * (control3.position - 3f * control2.position + 3f * control1.position - control0.position)
                + (2f * splineEnd_t * (-control3.position + 4f * control2.position - 5f * control1.position + 2f * control0.position))
                + control2.position - control0.position);

            control2First.SetPosition(0, control2.position);

            control2First.SetPosition(1, control2.position + endPosFirstD.normalized * derivativeLineLength);

            Vector3 endPosSecondD = (3f * splineEnd_t * (control3.position - 9f * control2.position + 9f * control1.position - 3f * control0.position)
                - (control3.position + 4f * control2.position - 5f * control1.position + 2f * control0.position));

            control2Second.SetPosition(0, control2.position);

            control2Second.SetPosition(1, control2.position + endPosSecondD.normalized * derivativeLineLength);

            //END LAST POINT DERIVATIVES

            //FOURTH CONTROL POINT

            control3First.enabled = false;

            control3Second.enabled = false;

            //END FOURTH CONTROL POINT

        }

    }

} // end of SplineSegmentGPUCompute

