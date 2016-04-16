using UnityEngine;
using System.Collections;
using System.Linq;

public class SurfaceVisualSettings : MonoBehaviour {

    public GameObject[] trees;
    public GameObject[] bushes;
    public Texture2D[] grasses;
    public GameObject[] boulders;
    public Texture2D green;
    public Texture2D dust;
    public Color grassHealthy;
    public Color grassWithered;
    public int detailResolution = 32;
    public int height = 10;
    public int alphaResolution = 32;
    public int size = 128;
    public float frequency = 2f;
    public int seed = 1080;
    [Range(1, 200)]
    public int resolution = 65;
    [Range(1, 8)]
    public int octaves = 1;
    [Range(1f, 4f)]
    public float lacunarity = 2f;
    [Range(0f, 1f)]
    public float persistence = 0.5f;

    public PerlinNoise Perlin { get; private set; }

    void Awake()
    {
        Perlin = new PerlinNoise(new PerlinHash(256, seed).generate(), frequency, octaves, lacunarity, persistence);
        GreenSplat = new SplatPrototype();
        GreenSplat.texture = green;
        GreenSplat.tileSize = new Vector2(20,20);
        DustSplat = new SplatPrototype();
        DustSplat.texture = dust;
        DustSplat.tileSize = new Vector2(2,3);

        PlantPrototypes = new TreePrototype[trees.Length + bushes.Length];

        TreePrototypes = new TreePrototype[trees.Length];
        for (int i = 0; i < trees.Length; i++)
        {
            var tree = trees[i];
            var prototype = new TreePrototype();
            prototype.prefab = tree;
            TreePrototypes[i] = prototype;
            PlantPrototypes[i] = prototype;
        }

        BushPrototypes = new TreePrototype[bushes.Length];
        for (int i = 0; i < bushes.Length; i++)
        {
            var bush = bushes[i];
            var prototype = new TreePrototype();
            prototype.prefab = bush;
            BushPrototypes[i] = prototype;
            PlantPrototypes[i + trees.Length] = prototype;
        }

        DetailPrototypes = new DetailPrototype[grasses.Length + boulders.Length];
        GrassPrototypes =new DetailPrototype[grasses.Length];
        for (var i = 0; i < grasses.Length; i++)
        {
            var prototype = new DetailPrototype();
            prototype.prototypeTexture = grasses[i];
            prototype.healthyColor = grassHealthy;
            prototype.dryColor = grassWithered;
            GrassPrototypes[i] = prototype;
            DetailPrototypes[i] = prototype;
        }
        BoulderPrototypes = new DetailPrototype[boulders.Length];
        for (var i = 0; i < boulders.Length; i++)
        {
            var prototype = new DetailPrototype();
            prototype.usePrototypeMesh = true;
            prototype.healthyColor = Color.white;
            prototype.dryColor = Color.white;
            prototype.renderMode = DetailRenderMode.VertexLit;
            prototype.prototype = boulders[i];
            BoulderPrototypes[i] = prototype;
            DetailPrototypes[GrassPrototypes.Length + i] = prototype;
        }
    }


    public SplatPrototype GreenSplat { get; private set; }
    public SplatPrototype DustSplat { get; private set; }

    public TreePrototype[] TreePrototypes { get; private set; }
    public TreePrototype[] BushPrototypes { get; private set; }

    public DetailPrototype[] GrassPrototypes { get; private set; }

    public DetailPrototype[] BoulderPrototypes { get; private set; }

    public TreePrototype[] PlantPrototypes { get; private set; }

    public DetailPrototype[] DetailPrototypes { get; private set; }
}
