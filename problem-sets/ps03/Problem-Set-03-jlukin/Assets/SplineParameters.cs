/*  CSCI-B481/B581 - Fall 2023 - Mitja Hmeljak

    Problem Set 02 starter C# program code

    You have to complete the parts marked as TODO.

    This script should:
    provide the correct parameters in the spline matrices,
    as from the spline Matrix Form.
    
    (defined as in Lecture 10 notes,
     albeit in Lecture 10 the parameter is named 't',
     and here we're naming the parameter 'u',
     following the nomenclature used in the textbook)
                         
    However, keep in mind that Unity Matrix4x4 are "column major",
    as detailed in assignment instructions.

    Original demo code by CSCI-B481 alumnus Rajin Shankar, IU Computer Science.
 */

using UnityEngine;

    public static class SplineParameters {
    
        public enum SplineType { Bezier, CatmullRom, Bspline }

        public static Matrix4x4 GetMatrix(SplineType type) {
        
            switch (type) {
                // TODO: generate Bezier spline matrix,
                //   with constants as per Lecture 10 notes:
                case SplineType.Bezier:
                    return new Matrix4x4( // COLUMN MAJOR!
                        new Vector4(-1, 3, -3, 1), // TODO
                        new Vector4(3, -6, 3, 0), // TODO
                        new Vector4(-3, 3, 0, 0), // TODO
                        new Vector4(1, 0, 0, 0) // TODO
                    );
                // TODO: generate Catmull-Rom spline matrix,
                //   with constants as per Lecture 10 notes:
                case SplineType.CatmullRom:
                    return new Matrix4x4( // COLUMN MAJOR!
                        new Vector4(-0.5f, 1.5f, -1.5f, 0.5f), // TODO
                        new Vector4(1f, -2.5f, 2f, -0.5f), // TODO
                        new Vector4(-0.5f, 0f, 0.5f, 0f), // TODO
                        new Vector4(0f, 1f, 0f, 0f) // TODO
                    );
                // TODO: generate B-spline matrix,
                //   with constants as per Lecture 10 notes:
                case SplineType.Bspline:
                    return new Matrix4x4( // COLUMN MAJOR!
                        new Vector4((-1/6f), (0.5f), (-0.5f), (1/6f)), // TODO
                        new Vector4((0.5f), (-1f), (0.5f), (0f)), // TODO
                        new Vector4((-0.5f), (0f), (0.5f), (0f)), // TODO
                        new Vector4((1/6f), (2/3f), (1/6f), (0f)) // TODO
                    );
                default:
                    return Matrix4x4.identity;
            }
        } // end of GetMatrix()

        // this could be useful for multi-segment spline curves:
        public static bool UsesConnectedEndpoints(SplineType type) {
            switch (type) {
                case SplineType.Bezier: return true;
                case SplineType.CatmullRom: return false;
                case SplineType.Bspline: return false;
                default: return false;
            }
        } // end of UsesConnectedEndpoints()
        
    } // end of class SplineParameters
