using UnityEngine;
using System;

public class PerlinNoise
{
    private  int[] hash;
    private float frequency;
    private readonly int octaves;
    private readonly float lacunarity;
    private readonly float persistence;

    private const int hashMask = 255;

    public PerlinNoise(int[] hash,float frequency, int octaves, float lacunarity, float persistence)
    {
        this.hash = hash;
        this.frequency = frequency;
        this.octaves = octaves;
        this.lacunarity = lacunarity;
        this.persistence = persistence;
    }

    private  float Smooth(float t)
    {
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }

    private  float sqr2 = Mathf.Sqrt(2f);

    public  float Perlin1D(Vector3 point)
    {
        point *= frequency;
        int i0 = Mathf.FloorToInt(point.x);
        float t0 = point.x - i0;
        float t1 = t0 - 1f;
        i0 &= hashMask;
        int i1 = i0 + 1;

        float g0 = PerlinGradients.oneD[hash[i0] & PerlinGradients.mask1D];
        float g1 = PerlinGradients.oneD[hash[i1] & PerlinGradients.mask1D];

        float v0 = g0 * t0;
        float v1 = g1 * t1;

        float t = Smooth(t0);
        return Mathf.Lerp(v0, v1, t) * 2f;
    }

    public float Perlin2D(Vector3 point)
    {
        point *= frequency;
        int ix0 = Mathf.FloorToInt(point.x);
        int iy0 = Mathf.FloorToInt(point.y);
        float tx0 = point.x - ix0;
        float ty0 = point.y - iy0;
        float tx1 = tx0 - 1f;
        float ty1 = ty0 - 1f;
        ix0 &= hashMask;
        iy0 &= hashMask;
        int ix1 = ix0 + 1;
        int iy1 = iy0 + 1;

        int h0 = hash[ix0];
        int h1 = hash[ix1];
        Vector2 g00 = PerlinGradients.twoD[hash[h0 + iy0] & PerlinGradients.mask2D];
        Vector2 g10 = PerlinGradients.twoD[hash[h1 + iy0] & PerlinGradients.mask2D];
        Vector2 g01 = PerlinGradients.twoD[hash[h0 + iy1] & PerlinGradients.mask2D];
        Vector2 g11 = PerlinGradients.twoD[hash[h1 + iy1] & PerlinGradients.mask2D];

        float v00 = Vector2.Dot(g00, new Vector2( tx0, ty0 ));
        float v10 = Vector2.Dot(g10, new Vector2( tx1, ty0 ));
        float v01 = Vector2.Dot(g01, new Vector2(tx0, ty1));
        float v11 = Vector2.Dot(g11, new Vector2(tx1, ty1));    

        float tx = Smooth(tx0);
        float ty = Smooth(ty0);
        return Mathf.Lerp(
            Mathf.Lerp(v00, v10, tx),
            Mathf.Lerp(v01, v11, tx),
            ty) * sqr2;
    }

    public  float Perlin3D(Vector3 point)
    {
        point *= frequency;
        int ix0 = Mathf.FloorToInt(point.x);
        int iy0 = Mathf.FloorToInt(point.y);
        int iz0 = Mathf.FloorToInt(point.z);
        float tx0 = point.x - ix0;
        float ty0 = point.y - iy0;
        float tz0 = point.z - iz0;
        float tx1 = tx0 - 1f;
        float ty1 = ty0 - 1f;
        float tz1 = tz0 - 1f;
        ix0 &= hashMask;
        iy0 &= hashMask;
        iz0 &= hashMask;
        int ix1 = ix0 + 1;
        int iy1 = iy0 + 1;
        int iz1 = iz0 + 1;

        int h0 = hash[ix0];
        int h1 = hash[ix1];
        int h00 = hash[h0 + iy0];
        int h10 = hash[h1 + iy0];
        int h01 = hash[h0 + iy1];
        int h11 = hash[h1 + iy1];
        Vector3 g000 = PerlinGradients.threeD[hash[h00 + iz0] & PerlinGradients.mask3D];
        Vector3 g100 = PerlinGradients.threeD[hash[h10 + iz0] & PerlinGradients.mask3D];
        Vector3 g010 = PerlinGradients.threeD[hash[h01 + iz0] & PerlinGradients.mask3D];
        Vector3 g110 = PerlinGradients.threeD[hash[h11 + iz0] & PerlinGradients.mask3D];
        Vector3 g001 = PerlinGradients.threeD[hash[h00 + iz1] & PerlinGradients.mask3D];
        Vector3 g101 = PerlinGradients.threeD[hash[h10 + iz1] & PerlinGradients.mask3D];
        Vector3 g011 = PerlinGradients.threeD[hash[h01 + iz1] & PerlinGradients.mask3D];
        Vector3 g111 = PerlinGradients.threeD[hash[h11 + iz1] & PerlinGradients.mask3D];

        float v000 = Vector3.Dot(g000, new Vector3(tx0, ty0, tz0));
        float v100 = Vector3.Dot(g100, new Vector3(tx1, ty0, tz0));
        float v010 = Vector3.Dot(g010, new Vector3(tx0, ty1, tz0));
        float v110 = Vector3.Dot(g110, new Vector3(tx1, ty1, tz0));
        float v001 = Vector3.Dot(g001, new Vector3(tx0, ty0, tz1));
        float v101 = Vector3.Dot(g101, new Vector3(tx1, ty0, tz1));
        float v011 = Vector3.Dot(g011, new Vector3(tx0, ty1, tz1));
        float v111 = Vector3.Dot(g111, new Vector3(tx1, ty1, tz1));

        float tx = Smooth(tx0);
        float ty = Smooth(ty0);
        float tz = Smooth(tz0);
        return Mathf.Lerp(
            Mathf.Lerp(Mathf.Lerp(v000, v100, tx), Mathf.Lerp(v010, v110, tx), ty),
            Mathf.Lerp(Mathf.Lerp(v001, v101, tx), Mathf.Lerp(v011, v111, tx), ty),
            tz);
    }

    public  float Sum(Func<Vector3,float> method, Vector3 point)
    {
        float sum = method(point);
        float amplitude = 1f;
        float range = 1f;
        var frequencyCopy = frequency;
        for (int o = 1; o < octaves; o++)
        {
            frequency *= lacunarity;
            amplitude *= persistence;
            range += amplitude;
            sum += method(point) * amplitude;
        }
        frequency = frequencyCopy;
        return sum / range;
    }

    public float Sum2D(float x,float z)
    {
        return Sum(Perlin2D, new Vector3(x, z, 0));
    }

}

