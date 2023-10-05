/*  CSCI-B481/B581 - Fall 2022 - Mitja Hmeljak
    This script provides a library of "utility" methods,
    that may be useful to solve Problem Set 01.
    
    However, you may have to complete the parts marked as TODO ...
    Original demo code by CSCI-B481 alumnus Rajin Shankar, IU Computer Science.
 */

using UnityEngine;

namespace PS01 {

    public static class LineUtility {
    
        // DirectionNormal() --- returns the normal to a given direction vector:
        public static Vector2 DirectionNormal(Vector2 direction) {
            // TODO: compute 
              Vector2 normal = Vector2.Perpendicular(direction);
              return normal;
        } // end of DirectionNormal() 

        // LineSegmentNormal() --- returns the normal to a line segment:
        public static Vector2 LineSegmentNormal(Vector2 start, Vector2 end) {
            // TODO: compute 
              Vector2 direction =  new Vector2(-(end.y - start.y), (end.x - start.x));
              Vector2 normal = Vector2.Perpendicular(direction);
              return normal;
        } // end of LineSegmentNormal()


        // ClosestPointOnLine() --- returns the closest point on a line to a given query point:
        public static Vector2 ClosestPointOnLine(Vector2 pointOnLine, Vector2 direction, Vector2 point) {
            // TODO: compute 
            //
            // ERRATA CORRIGE:
            //  Vector2 localX = ...  <- incorrect, it should be a float
            //  float localX = ...    <- correct type definition

            Vector2 primaryLine = point - direction;

            Vector2 testLine =  point - pointOnLine;
            float dProduct = Vector2.Dot(testLine, primaryLine);

            Vector2 resultingLine = primaryLine*(dProduct) + pointOnLine;

            return resultingLine;
            
            //  you may find useful the 2D Point-Line Geometry expressions shown at lecture time
            //  
            
        } // end of ClosestPointOnLine()


        // ClosestPointOnSegment() --- returns the closest point (on a line segment)
        //                             to a given subject point:
        public static Vector2 ClosestPointOnSegment(Vector2 start, Vector2 end, Vector2 point) {
            // TODO: 
            //  you may find the above methods useful, once you complete them...

            Vector2 direction = end - start;

            float magnitude = direction.sqrMagnitude;

            float length = direction.magnitude;

            float dProduct = Vector2.Dot((point - start), direction);

            dProduct = Mathf.Clamp(dProduct, 0f, magnitude);

            float localX = dProduct / magnitude;


             if (localX < 0) {
                 return start;
             } else if (localX > length) {
                 return end;
             } else {
                 return start + (end - start) * localX;
             }
        } // end of ClosestPointOnSegement()



        // NOTE: look at the two method implementations below, and 
        //       decide whether you prefer to use:
        //       either the ClosestPointOnPolygon() method using Vector2[] polygonPoints
        //       or the ClosestPointOnPolygon() method using Transform[] polygonVertices,
        //       but not both!
        
        
        // ClosestPointOnPolygon() --- returns the closest point (on a polygon)
        //                             to a given query point.
        //
        // Note:
        //   Polygon given as array of vertices with vertex[n-1] connecting back to vertex[0]
       /* public static Vector2 ClosestPointOnPolygon(Vector2[] polygonPoints, Vector2 point) {
            Vector2 result = Vector2.zero;
            float minSqrDistance = float.PositiveInfinity;
            for (int i = 0; i < polygonPoints.Length; i++) {
                int j = (i + 1) % polygonPoints.Length;
                Vector2 side = polygonPoints[j] - polygonPoints[i];
                float sideLength = side.magnitude;
                Vector2 sideDirection = side / sideLength;

            // TODO: 
            //  you may find useful the utility methods at the top of this file, once you complete them...

                Vector2 pointOnPolygon = Vector2.positiveInfinity;
                Vector2 pointB = polygonPoints[polygonPoints.Length - 1];
                Vector2 pointA = pointB;
                pointB = polygonPoints[i];
                float localX = Vector2.Dot((point - pointA), polygonPoints[i]);
               if (localX < 0) {
                   pointOnPolygon = pointA;
               } else if (localX > sideLength) {
                   pointOnPolygon = (1 - localX) * (pointA) + localX * pointB;
               } else {
                   pointOnPolygon = pointB;
               }

            // TODO:
            //  the following code works, as long as you computed pointOnPolygon correctly.
            //  It will be useful to understand what the following lines do:
                Vector2 delta = point - pointOnPolygon;
                float sqrDistance = delta.sqrMagnitude;

                if (sqrDistance < minSqrDistance) {
                    result = pointOnPolygon;
                    minSqrDistance = sqrDistance;
                }
            }
            return result;
        } // end of ClosestPointOnPolygon()*/


        // // ClosestPointOnPolygon() --- returns the closest point (on a polygon)
        // //                             to a given subject point.
        // //  Note:
        // //      the polygon is given as array of transforms
        // //      with vertex[n-1] connecting back to vertex[0]
        // //
         public static Vector2 ClosestPointOnPolygon(Transform[] polygonPoints, Vector2 subjectPoint) {
        
             Vector2 result = Vector2.zero;
             float minSqrDistance = float.PositiveInfinity;
            
             for (int i = 0; i < polygonPoints.Length; i++) {
                 
                int j = (i + 1) % polygonPoints.Length;
                Vector2 side = polygonPoints[j].position - polygonPoints[i].position;
                float sideLength = side.magnitude;
                Vector2 sideDirection = side / sideLength;

             // TODO: 
             //  you may find useful the utility methods at the top of this file, once you complete them...

                Vector2 pointOnPolygon;
                Vector2 pointA = polygonPoints[i].position;
                Vector2 pointB = polygonPoints[j].position;

                float localX = Mathf.Clamp(Vector2.Dot(subjectPoint - pointA, sideDirection), 0f, sideLength);

                if (localX <= 0) {
                    pointOnPolygon = pointA;
                 } else if (localX >= sideLength) {
                    pointOnPolygon = pointB;
                 } else {
                    pointOnPolygon = ClosestPointOnSegment(pointA, pointB, subjectPoint);
                }

             // TODO:
             //  the following code works, as long as you computed pointOnPolygon correctly.
             //  It will be useful to understand what the following lines do:
                 Vector2 delta = subjectPoint - pointOnPolygon;
                 float sqrDistance = delta.sqrMagnitude;

                 if (sqrDistance < minSqrDistance) {
                     result = pointOnPolygon;
                     minSqrDistance = sqrDistance;
                 }
             }
             return result;
         } // end of ClosestPointOnPolygon()


    } // end of static class LineUtility

} // end of namespace PS01