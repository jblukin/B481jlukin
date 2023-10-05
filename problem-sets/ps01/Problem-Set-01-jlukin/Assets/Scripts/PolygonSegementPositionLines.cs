/*  CSCI-B481/B581 - Fall 2023 - Mitja Hmeljak
    This script will position the start and end points for two line renderers.
    Positioning is done by using GameObject Transforms.
    Used to show closest point on line segment.
    Original demo code by CSCI-B481 alumnus Rajin Shankar, IU Computer Science.
 */

using UnityEngine;

namespace PS01 {

    public class PolygonSegementPositionLines : MonoBehaviour {

        // fields to connect to Unity objects:

        [Header("Points")]
        [SerializeField] private Transform[] PolygonPoints = new Transform[6];
        [SerializeField] private Transform subjectPoint;

        [Header("Lines")]
        [SerializeField] private LineRenderer[] PolygonLines = new LineRenderer[6];
        [SerializeField] private LineRenderer subjectLine;

        private int lineCount = 0;

        // Update() is called once per frame:
        private void Update() {

            lineCount = 0;
            // set positions for subject line vertices:
            foreach (LineRenderer line in PolygonLines)
            {

                line.SetPosition(0, PolygonPoints[lineCount++].position);
                if (lineCount == PolygonPoints.Length) lineCount = 0;
                line.SetPosition(1, PolygonPoints[lineCount].position);

            }

            // if debug is necessary, uncomment these lines:
            // Debug.Log("subjectLineStartTransform.position = " + subjectLineStartTransform.position);
            // Debug.Log("subjectLineEndTransform.position = " + subjectLineEndTransform.position);
            // Debug.Log("subjectLineRenderer.GetPosition(0) = " + subjectLineRenderer.GetPosition(0));
            // Debug.Log("subjectLineRenderer.GetPosition(1) = " + subjectLineRenderer.GetPosition(1));

            // set positions for connecting line vertices:

            // TODO - uncomment when .ClosestPointOnSegment is implemented:
            Vector2 lClosestPoint = PS01.LineUtility.ClosestPointOnPolygon(
            PolygonPoints,
            subjectPoint.position);

            // TODO: remove next line when .ClosestPointOnSegment is implemented and lClosestPoint can be computed:
            //Vector2 lClosestPoint = Vector2.one; // temporarily set to Vector2.one, until we compute lClosestPoint correctly!
            
            subjectLine.SetPosition(0, subjectPoint.position);
            subjectLine.SetPosition(1, lClosestPoint);
        } // end of Update()

    } // end of class SingleSegmentPositionLines

} // end of namespace PS01