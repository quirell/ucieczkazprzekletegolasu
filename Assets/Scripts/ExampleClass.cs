using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    public int pixWidth;
    public int pixHeight;
    public float xOrg;
    public float yOrg;
    public float scale = 1.0F;
    public int resolution = 33;
    public int size = 32;
    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;
    private PerlinNoise perlin;
    public float freq;
    public float lacunarity;
    public float persistence;
    public int octaves;
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        noiseTex = new Texture2D(resolution,resolution);
        pix = new Color[resolution*resolution];
        rend.material.mainTexture = noiseTex;
        perlin = new PerlinNoise(new PerlinHash(256,1080).generate(),freq,octaves,lacunarity,persistence );
        Calc2();    
    }
    void CalcNoise()
    {

        float y = 0.0F;
        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)(y * noiseTex.width + x)] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    void Calc2()
    {
        float ratio = (float)size/(resolution);
        //        float ratio = 1f;
        var X = 0f;
        var Z = 0f;
        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                var posX = (X + (float)(x)  *ratio);
                var posZ = (Z + (float)(z)  *ratio);
                var noise = perlin.Perlin2D(new Vector3(posX, posZ, 0))+0.5f;
                noise /= 1.2f;
                pix[x*resolution + z] = new Color(noise, noise, noise);
//                Debug.LogFormat("{0} {1} {2}",posX,posZ ,pix[x*resolution+z]);
            }
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }
    void Update()
    {
        Calc2();
    }
}