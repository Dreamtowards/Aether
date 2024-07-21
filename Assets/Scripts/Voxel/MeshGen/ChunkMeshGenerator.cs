using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;

namespace Aether
{
public static class ChunkMeshGenerator
{
    static readonly ProfilerMarker s_Pm = new("Aether.JobChunkMeshGen");

    public static void GenerateMesh(VertexBuffer vbuf, Chunk chunk)
    {
        using var _p = s_Pm.Auto();
        using var _t = new BenchmarkTimer("Managed ChunkMeshGen at "+chunk.chunkpos+" in {0}");
        
        chunk.ForVoxels((int3 localpos, ref Vox vox) =>
        {
            if (vox.IsNil())
                return;

            PutCube(vbuf, localpos, vox, chunk);

        });
    }



    #region Blocky Cube

    static float[] CUBE_POS = {
        0, 0, 1, 0, 1, 1, 0, 1, 0,  // Left -X
        0, 0, 1, 0, 1, 0, 0, 0, 0,
        1, 0, 0, 1, 1, 0, 1, 1, 1,  // Right +X
        1, 0, 0, 1, 1, 1, 1, 0, 1,
        0, 0, 1, 0, 0, 0, 1, 0, 0,  // Bottom -Y
        0, 0, 1, 1, 0, 0, 1, 0, 1,
        0, 1, 1, 1, 1, 1, 1, 1, 0,  // Bottom +Y
        0, 1, 1, 1, 1, 0, 0, 1, 0,
        0, 0, 0, 0, 1, 0, 1, 1, 0,  // Front -Z
        0, 0, 0, 1, 1, 0, 1, 0, 0,
        1, 0, 1, 1, 1, 1, 0, 1, 1,  // Back +Z
        1, 0, 1, 0, 1, 1, 0, 0, 1,
    };
    static float[] CUBE_UV = {
        1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0,  // One Face.
        1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0,
        1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0,
        1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0,
        1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0,
        1, 0, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0,
    };
    static int[] CUBE_NORM = {
        -1, 0, 0,-1, 0, 0,-1, 0, 0,-1, 0, 0,-1, 0, 0,-1, 0, 0,
        1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0,
        0,-1, 0, 0,-1, 0, 0,-1, 0, 0,-1, 0, 0,-1, 0, 0,-1, 0,
        0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0,
        0, 0,-1, 0, 0,-1, 0, 0,-1, 0, 0,-1, 0, 0,-1, 0, 0,-1,
        0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1
    };

    static void PutCube(VertexBuffer vbuf, int3 localpos, Vox vox, Chunk chunk)
    {
        for (int faceIdx = 0; faceIdx < 6; ++faceIdx)
        {
            int3 faceDir = Maths.IVec3(CUBE_NORM, faceIdx * 18);   // 18: 3 scalar * 3 vertex * 2 triangle

            chunk.GetVoxel(localpos + faceDir, out Vox neibVox);
            if (neibVox.IsObaque())
                continue;

            for (int vertIdx = 0; vertIdx < 6; ++vertIdx)
            {
                vbuf.PushVertex(
                    Maths.Vec3(CUBE_POS, faceIdx * 18 + vertIdx * 3) + localpos,
                    new(vox.texId, 0), //Maths.vec2(CUBE_UV,  faceIdx * 12 + vertIdx * 2),
                    Maths.IVec3(CUBE_NORM, faceIdx * 18 + vertIdx * 3)
                );
            }
        }
    }




    #endregion


    #region Misc, Foliages

    public static void PutFace(VertexBuffer vbuf, int3 p)
    {

    }

    #endregion


    #region The Gread SurfaceNets, Isosurface


    public static int3[] SN_VERT = {
        new(0, 0, 0),  // 0
        new(0, 0, 1),
        new(0, 1, 0),  // 2
        new(0, 1, 1),
        new(1, 0, 0),  // 4
        new(1, 0, 1),
        new(1, 1, 0),  // 6
        new(1, 1, 1)
};
    // from min to max in each Edge.  axis order x y z.
    // Diagonal Edge in Cell is in-axis-flip-index edge.  i.e. diag of edge[axis*4 +i] is edge[axis*4 +(3-i)]
    /*     +--2--+    +-----+    +-----+
        *    /|    /|   /7    /6  11|    10
        *   +--3--+ |  +-----+ |  +-----+ |
        *   | +--0|-+  5 +---4-+  | +---|-+
        *   |/    |/   |/    |/   |9    |8
        *   +--1--+    +-----+    +-----+
        *   |3  2| winding. for each axis.
        *   |1  0|
        */
    static int[,] EDGE = {  // [12,2]
        {0,4}, {1,5}, {2,6}, {3,7},  // X
        {5,7}, {1,3}, {4,6}, {0,2},  // Y
        {4,5}, {0,1}, {6,7}, {2,3}   // Z
    };
    static int3[] AXES = {
        new(1, 0, 0),
        new(0, 1, 0),
        new(0, 0, 1)
    };
    static int3[,] ADJACENT = {
        {new(0,0,0), new(0,-1,0), new(0,-1,-1), new(0,-1,-1), new(0,0,-1), new(0,0,0)},
        {new(0,0,0), new(0,0,-1), new(-1,0,-1), new(-1,0,-1), new(-1,0,0), new(0,0,0)},
        {new(0,0,0), new(-1,0,0), new(-1,-1,0), new(-1,-1,0), new(0,-1,0), new(0,0,0)}
    };/*

    static void SN_GenerateMesh(Chunk chunk, VertexData vts)
    {

        for (int rx = 0; rx < 16; ++rx)
        {
            for (int ry = 0; ry < 16; ++ry)
            {
                for (int rz = 0; rz < 16; ++rz)
                {
                    float3 rp = new(rx, ry, rz);
                    Cell c0 = chunk.LocalCell(rx, ry, rz);

                    // for 3 axes edges, if sign-changed, connect adjacent 4 cells' vertices
                    for (int axis_i = 0; axis_i < 3; ++axis_i)
                    {
                        Cell c1 = chunk.LocalCell(rp + AXES[axis_i], true);

                        // Skip the face constructing with a Nil Cell. since the FeaturePoint and Normal(SDF Grad) cannot be evaluated.
                        // ?but Outside should be Enclosed? or the Shadow Casting will be a problem. if a Ceil in TopNil
                        if (c1.IsNil())
                            continue;

                        if (SN_SignChanged(c0, c1))
                        {
                            for (int quadv_i = 0; quadv_i < 6; ++quadv_i)
                            {
                                int winded_vi = c0.IsSolid() ? quadv_i : 5 - quadv_i;
                                float3 quadp = rp + ADJACENT[axis_i, winded_vi];

                                ref Cell c = ref chunk.LocalCell(quadp, true);

                                bool badQuad = c.IsNil();
                                //if (badQuad)
                                //{
                                //    vts.RemoveVertex((int)Maths.Floor(vts.VertexCount(), 6), quadv_i);
                                //    break;
                                //}

                                //if (c.FeaturePoint.x == Mathf.Infinity)
                                {
                                    c.FeaturePoint = SN_FeaturePoint(quadp, chunk);
                                    c.Normal = -SN_Grad(quadp, chunk);  // Need Neg. since its not SDF but DensityField.

                                    if (!Maths.IsFinite(c.FeaturePoint))
                                    {
                                        c.FeaturePoint = new(0, -99, 0); ;
                                        badQuad = true;
                                    }
                                    if (!Maths.IsFinite(c.Normal) || math.lengthsq(c.Normal) < 0.1f)
                                    {
                                        c.Normal = new(0, 1, 0);
                                    }
                                }
                                if (badQuad)
                                {
                                    vts.RemoveVertex((int)Maths.Floor(vts.VertexCount(), 6), quadv_i);
                                    break;
                                }

                                float3 p = quadp + c.FeaturePoint;


                                // Select Material of 8 Corners. (Max Density Value)
                                Material mtl = null;
                                float min_dist = float.PositiveInfinity;
                                foreach (float3 vp in SN_VERT)
                                {
                                    ref Cell vc = ref chunk.LocalCell(quadp + vp, true);
                                    if (vc.Mtl != null && vc.Value > 0 && vc.Value < min_dist)
                                    {
                                        min_dist = vc.Value;
                                        mtl = vc.Mtl;
                                    }
                                }
#if DEBUG
                                Log.assert(mtl != null, "MeshGen Error: Vertex MtlId == 0.");
                                Log.assert(Maths.IsFinite(p), () => $"MeshGen Error: Non-Finite Vertex Position Value. {p}");

                                float3 n = c.Normal;
                                Log.assert(Maths.IsFinite(n), () => $"MeshGen Error: Non-Finite Vertex Normal Value. {n}");
                                Log.assert(Mathf.Abs(math.lengthsq(n) - 1.0f) < 0.2f, () => $"MeshGen Error: Vertex Normal LengthSq != 1.0. {n}");
#endif

                                vts.AddVertex(p, new(mtl == null ? 0 : mtl.Id, -1), c.Normal);
                            }
                        }
                    }
                }
            }
        }
    }

    static bool SN_SignChanged(in Cell c0, in Cell c1)
    {
        return (c0.Value > 0) != (c1.Value > 0);
    }

    // Evaluate Normal of a Cell FeaturePoint
    // via Approxiate Differental Gradient  
    // WARN: may produce NaN Normal Value if the Cell's value is NaN (Nil Cell in the Context)
    static float3 SN_Grad(float3 rp, Chunk chunk)
    {
        float E = 1.0f;  // epsilon
        float denom = 1.0f / (2.0f * E);
        return Vector3.Normalize(denom * new float3(
            chunk.LocalCell(rp + new float3(E, 0, 0), true).Value - chunk.LocalCell(rp - new float3(E, 0, 0), true).Value,
            chunk.LocalCell(rp + new float3(0, E, 0), true).Value - chunk.LocalCell(rp - new float3(0, E, 0), true).Value,
            chunk.LocalCell(rp + new float3(0, 0, E), true).Value - chunk.LocalCell(rp - new float3(0, 0, E), true).Value));
    }

    // Evaluate FeaturePoint
    // returns cell-local point.
    static float3 SN_FeaturePoint(int3 relpos, Chunk chunk)
    {
        int signchanges = 0;
        float3 sumFp = new(0, 0, 0);

        for (int edge_i = 0; edge_i < 12; ++edge_i)
        {
            int3 p0 = SN_VERT[EDGE[edge_i, 0]];
            int3 p1 = SN_VERT[EDGE[edge_i, 1]];
            Vox v0 = chunk.GetVoxelOr(relpos + p0);
            Vox v1 = chunk.GetVoxelOr(relpos + p1);

            if (SN_SignChanged(v0, v1))
            {
                float t = Mathf.InverseLerp(v0.density, v1.density, 0);
                if (!float.IsFinite(t)) t = 0;  // t maybe NaN if accessing a Nil Cell.

                float3 p = math.lerp(p0, p1, t);

                sumFp += p;
                ++signchanges;
            }
        }

        Assert.NotZero(signchanges, "FpEval Error: No SignChange.");
        Assert.IsTrue(float.IsFinite(sumFp.x), "FpEval Error: Non-Finite Fp Value.");

        return sumFp / signchanges;
    }*/

    #endregion


}
}
