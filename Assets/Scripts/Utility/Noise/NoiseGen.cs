
using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Internal;

[Serializable]
public struct NoiseGenStruct
{
    [HideInInspector]
    public FastNoiseLiteStruct noise;

    public NoiseGenStruct(int _DontUse_CompilerRequired = 0)
    {
        noise = new();
    }

    [ShowInInspector]
    public int Seed
    {
        get { return noise.GetSeed(); }
        set { noise.SetSeed(value); }
    }

    [ShowInInspector]
    public float Frequency
    {
        get { return noise.GetFrequency(); }
        set { noise.SetFrequency(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.NoiseType NoiseType
    {
        get { return noise.GetNoiseType(); }
        set { noise.SetNoiseType(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.RotationType3D RotationType3D
    {
        get { return noise.GetRotationType3D(); }
        set { noise.SetRotationType3D(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.FractalType FractalType
    {
        get { return noise.GetFractalType(); }
        set { noise.SetFractalType(value); }
    }

    [ShowInInspector]
    public int FractalOctaves
    {
        get { return noise.GetFractalOctaves(); }
        set { noise.SetFractalOctaves(value); }
    }

    [ShowInInspector]
    public float FractalLacunarity
    {
        get { return noise.GetFractalLacunarity(); }
        set { noise.SetFractalLacunarity(value); }
    }

    [ShowInInspector]
    public float FractialGain
    {
        get { return noise.GetFractalGain(); }
        set { noise.SetFractalGain(value); }
    }

    [ShowInInspector]
    public float FractalWeightedStrength
    {
        get { return noise.GetFractalWeightedStrength(); }
        set { noise.SetFractalWeightedStrength(value); }
    }

    [ShowInInspector]
    public float FractalPingPongStrength
    {
        get { return noise.GetFractalPingPongStrength(); }
        set { noise.SetFractalPingPongStrength(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.CellularDistanceFunction CellularDistanceFunction
    {
        get { return noise.GetCellularDistanceFunction(); }
        set { noise.SetCellularDistanceFunction(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.CellularReturnType CellularReturnType
    {
        get { return noise.GetCellularReturnType(); }
        set { noise.SetCellularReturnType(value); }
    }

    [ShowInInspector]
    public float CellularJitter
    {
        get { return noise.GetCellularJitter(); }
        set { noise.SetCellularJitter(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.DomainWarpType DomainWarpType
    {
        get { return noise.GetDomainWarpType(); }
        set { noise.SetDomainWarpType(value); }
    }

    [ShowInInspector]
    public float DomainWarpAmp
    {
        get { return noise.GetDomainWarpAmp(); }
        set { noise.SetDomainWarpAmp(value); }
    }

    [Button]
    public float Sample(float3 p)
    {
        return noise.GetNoise(p.x, p.y, p.z);
    }

    [Button]
    public float Sample(float2 p)
    {
        return noise.GetNoise(p.x, p.y);
    }

    public float Sample(float x, float y) 
    {
        return noise.GetNoise(x, y);
    }
}



[Serializable]
public class NoiseGen
{
    
    [HideInInspector]
    public FastNoiseLite noise = new();

    [ShowInInspector]
    public int Seed
    {
        get { return noise.GetSeed(); }
        set { noise.SetSeed(value); }
    }

    [ShowInInspector]
    public float Frequency
    {
        get { return noise.GetFrequency(); }
        set { noise.SetFrequency(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.NoiseType NoiseType
    {
        get { return noise.GetNoiseType(); }
        set { noise.SetNoiseType(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.RotationType3D RotationType3D
    {
        get { return noise.GetRotationType3D(); }
        set { noise.SetRotationType3D(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.FractalType FractalType
    {
        get { return noise.GetFractalType(); }
        set { noise.SetFractalType(value); }
    }

    [ShowInInspector]
    public int FractalOctaves
    {
        get { return noise.GetFractalOctaves(); }
        set { noise.SetFractalOctaves(value); }
    }

    [ShowInInspector]
    public float FractalLacunarity
    {
        get { return noise.GetFractalLacunarity(); }
        set { noise.SetFractalLacunarity(value); }
    }

    [ShowInInspector]
    public float FractialGain
    {
        get { return noise.GetFractalGain(); }
        set { noise.SetFractalGain(value); }
    }

    [ShowInInspector]
    public float FractalWeightedStrength
    {
        get { return noise.GetFractalWeightedStrength(); }
        set { noise.SetFractalWeightedStrength(value); }
    }

    [ShowInInspector]
    public float FractalPingPongStrength
    {
        get { return noise.GetFractalPingPongStrength(); }
        set { noise.SetFractalPingPongStrength(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.CellularDistanceFunction CellularDistanceFunction
    {
        get { return noise.GetCellularDistanceFunction(); }
        set { noise.SetCellularDistanceFunction(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.CellularReturnType CellularReturnType
    {
        get { return noise.GetCellularReturnType(); }
        set { noise.SetCellularReturnType(value); }
    }

    [ShowInInspector]
    public float CellularJitter
    {
        get { return noise.GetCellularJitter(); }
        set { noise.SetCellularJitter(value); }
    }

    [ShowInInspector]
    public FastNoiseLite.DomainWarpType DomainWarpType
    {
        get { return noise.GetDomainWarpType(); }
        set { noise.SetDomainWarpType(value); }
    }

    [ShowInInspector]
    public float DomainWarpAmp
    {
        get { return noise.GetDomainWarpAmp(); }
        set { noise.SetDomainWarpAmp(value); }
    }

    [Button]
    public float Sample(float3 p)
    {
        return noise.GetNoise(p.x, p.y, p.z);
    }

    [Button]
    public float Sample(float2 p)
    {
        return noise.GetNoise(p.x, p.y);
    }

    public float Sample(float x, float y) 
    {
        return noise.GetNoise(x, y);
    }

    public NoiseGenStruct ToStruct()
    {
        NoiseGenStruct n = new();
        n.Seed = Seed;
        n.Frequency = Frequency;
        n.NoiseType = NoiseType;
        n.RotationType3D = RotationType3D;
        n.FractalType = FractalType;
        n.FractalOctaves = FractalOctaves;
        n.FractalLacunarity = FractalLacunarity;
        n.FractialGain = FractialGain;
        n.FractalWeightedStrength = FractalWeightedStrength;
        n.FractalPingPongStrength = FractalPingPongStrength;
        n.CellularDistanceFunction = CellularDistanceFunction;
        n.CellularReturnType = CellularReturnType;
        n.CellularJitter = CellularJitter;
        n.DomainWarpType = DomainWarpType;
        n.DomainWarpAmp = DomainWarpAmp;
        return n;
    }
}