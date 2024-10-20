
using System;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Assertions;
using Vector3 = UnityEngine.Vector3;

namespace Aether
{
public static class ChunkMeshGenerator
{
    static readonly ProfilerMarker s_Pm = new("Aether.JobChunkMeshGen");

    // For Solid Blocks
    public static void GenerateMesh(VertexBuffer vbuf, Chunk chunk)
    {
        using var _p = s_Pm.Auto();
        using var _t = new BenchmarkTimer("Managed ChunkMeshGen at "+chunk.chunkpos+" in {0}");
        
        SN_GenerateMesh(vbuf, chunk);
        
        return;
        chunk.ForVoxels((int3 localpos, ref Vox vox) =>
        {
            if (vox.IsTexNil())
                return;

            PutCube(vbuf, localpos, vox, chunk);

        });
    }

    public static void GenerateMeshFoliage(VertexBuffer vbuf, Chunk chunk)
    {
        chunk.ForVoxels((int3 lp, ref Vox vox) =>
        {
            // 陷入悖论了
            // TexId 本该是 only for Solid Tex 的，因为 Solid Tex 是一个单独的 Atlas, 单独的 DrawCall

            if (vox.IsIsoNil() || vox.IsTexNil())
                return;
            
            var texId = vox.texId;
            if (texId == VoxTex.grass.NumId)
                PutGrass(vbuf, VoxTex.grass.NumId, lp);
            else if (texId == VoxTex.grass_moss.NumId)
                PutLeaves(vbuf, VoxTex.grass_moss.NumId, lp);
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
            if (!neibVox.IsTexNil())
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

    public static void PutLeaves(VertexBuffer vbuf, UInt16 texId, float3 pos) {
        var deg45 = 180 / 4.0f;
        var siz = 1.4f;

        PutFace(vbuf, texId, pos + 0.5f, Quaternion.AngleAxis(deg45, Vector3.up), new Vector2(1.4f, 1.0f) * siz);
        PutFace(vbuf, texId, pos + 0.5f, Quaternion.AngleAxis(deg45*3.0f, Vector3.up), new Vector2(1.4f, 1.0f) * siz);
        PutFace(vbuf, texId, pos + 0.5f, Quaternion.AngleAxis(-deg45, Vector3.up), new Vector2(1.0f, 1.4f) * siz);
        PutFace(vbuf, texId, pos + 0.5f, Quaternion.AngleAxis(-deg45*3.0f, Vector3.up), new Vector2(1.0f, 1.4f) * siz);
    }
    
    public static void PutGrass(VertexBuffer vbuf, UInt16 texId, float3 pos) {
        var ang = 180 / 3.0f;
        var siz = 1.4f;

        PutFace(vbuf, texId, pos + 0.5f, Quaternion.AngleAxis(ang, Vector3.up), Vector2.one * siz);
        PutFace(vbuf, texId, pos + 0.5f, Quaternion.AngleAxis(ang*2, Vector3.up), Vector2.one * siz);
        PutFace(vbuf, texId, pos + 0.5f, Quaternion.AngleAxis(ang*3, Vector3.up), Vector2.one * siz);
    }
    
    // put a -X face in middle of pos. for foliages.
    public static void PutFace(VertexBuffer vbuf, UInt16 texId, Vector3 pos, Quaternion rot, Vector2 scale)
    {
        // -X Face
        for (int i = 0;i < 6;i++) {
            // 6 verts
            var p = Maths.Vec3(CUBE_POS, i*3) - new float3(0.0f, 0.5f, 0.5f); // -0.5: centerized for proper rotation
            p = (rot * (p * new float3(1.0f, scale.y, scale.x))) + pos;

            var n = new float3(Maths.IVec3(CUBE_NORM, i * 3));
            n = rot * n;

            var uv = Maths.Vec2(CUBE_UV, i * 2);
            uv = VoxTex.MapUV(uv, texId);
            // uv.x += tex_id;
            // uv.y += light;

            vbuf.PushVertex(p, uv, n);
        }
    }

    #endregion


    #region SurfaceNets, Isosurface


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
    };

    static void SN_GenerateMesh(VertexBuffer vbuf, Chunk chunk)
    {

        for (int lx = 0; lx < 16; ++lx)
        for (int ly = 0; ly < 16; ++ly)
        for (int lz = 0; lz < 16; ++lz) 
        {
            int3 lp = new(lx, ly, lz);
            var c0 = chunk.AtVoxel(lp);

            // for 3 axes edges, if sign-changed, connect adjacent 4 cells' vertices
            for (int axis_i = 0; axis_i < 3; ++axis_i) 
            {
                if (!chunk.GetVoxel(lp + AXES[axis_i], out var c1))
                    continue;

                // Skip the face constructing with a Nil Cell. since the FeaturePoint and Normal(SDF Grad) cannot be evaluated.
                // ?but Outside should be Enclosed? or the Shadow Casting will be a problem. if a Ceil in TopNil
                // if (c1.IsNil())
                //     continue;

                if (!SN_SignChanged(c0, c1))
                    continue;
                
                for (int quadv_i = 0; quadv_i < 6; ++quadv_i)
                {
                    int winded_vi = !c0.IsIsoNil() ? quadv_i : 5 - quadv_i;
                    int3 quadp = lp + ADJACENT[axis_i, winded_vi];
                    
                    var c = chunk.GetVoxelOr(quadp);

                    bool badQuad = c.IsTexNil();
                    //if (badQuad)
                    //{
                    //    vts.RemoveVertex((int)Maths.Floor(vts.VertexCount(), 6), quadv_i);
                    //    break;
                    //}

                    //if (c.FeaturePoint.x == Mathf.Infinity)
                    {
                        c.CachedFp = SN_FeaturePoint(quadp, chunk);
                        c.CachedNorm = -SN_Grad(quadp, chunk);  // Need Neg. since its not SDF but DensityField.

                        if (!math.all(math.isfinite(c.CachedFp))) {
                            c.CachedFp = new(0, -99, 0); ;
                            badQuad = true;
                        }
                        // if (!math.all(math.isfinite(c.CachedNorm)) || math.lengthsq(c.CachedNorm) < 0.1f) {
                        //     c.CachedNorm = new(0, 1, 0);
                        // }
                        //
                        // chunk.SetVoxel(quadp, c);
                    }
                    // if (badQuad)
                    // {
                    //     vts.RemoveVertex((int)Maths.Floor(vts.VertexCount(), 6), quadv_i);
                    //     break;
                    // }

                    float3 p = quadp + c.CachedFp;


                    // Select Material of 8 Corners. (Max Density Value)
                    int texId = 0;
                    float min_dist = float.PositiveInfinity;
                    foreach (var vp in SN_VERT) {
                        if (!chunk.GetVoxel(quadp + vp, out var vc))
                            continue;
                        if (!vc.IsTexNil() && !vc.IsIsoNil() && vc.density < min_dist)
                        {
                            min_dist = vc.density;
                            texId = vc.texId;
                        }
                    }
#if DEBUG
                    Assert.IsTrue(texId != 0, "MeshGen Error: Vertex MtlId == 0.");
                    // Assert.IsTrue(math.all(math.isfinite(p)), () => $"MeshGen Error: Non-Finite Vertex Position Value. {p}");

                    // float3 n = c.Normal;
                    // Log.assert(Maths.IsFinite(n), () => $"MeshGen Error: Non-Finite Vertex Normal Value. {n}");
                    // Log.assert(Mathf.Abs(math.lengthsq(n) - 1.0f) < 0.2f, () => $"MeshGen Error: Vertex Normal LengthSq != 1.0. {n}");
#endif

                    vbuf.PushVertex(p, new(texId, -1), c.CachedNorm);
                }
            }
        }
    }

    static bool SN_SignChanged(in Vox c0, in Vox c1)
    {
        return (c0.density > 0) != (c1.density > 0);
    }

    // Evaluate Normal of a Cell FeaturePoint
    // via Approxiate Differental Gradient  
    // WARN: may produce NaN Normal Value if the Cell's value is NaN (Nil Cell in the Context)
    static float3 SN_Grad(int3 rp, Chunk chunk)
    {
        var E = 1;  // epsilon
        float denom = 1.0f / (2.0f * E);
        return math.normalize(denom * new float3(
            chunk.GetVoxelOr(rp + new int3(E, 0, 0)).density - chunk.GetVoxelOr(rp - new int3(E, 0, 0)).density,
            chunk.GetVoxelOr(rp + new int3(0, E, 0)).density - chunk.GetVoxelOr(rp - new int3(0, E, 0)).density,
            chunk.GetVoxelOr(rp + new int3(0, 0, E)).density - chunk.GetVoxelOr(rp - new int3(0, 0, E)).density));
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
                float t = math.unlerp(v0.density, v1.density, 0);
                if (!float.IsFinite(t)) t = 0;  // t maybe NaN if accessing a Nil Cell.

                float3 p = math.lerp(p0, p1, t);

                sumFp += p;
                ++signchanges;
            }
        }

        Assert.AreNotEqual(signchanges, 0, "FpEval Error: No SignChange.");
        Assert.IsTrue(float.IsFinite(sumFp.x), "FpEval Error: Non-Finite Fp Value.");

        return sumFp / signchanges;
    }

    #endregion


}
}
