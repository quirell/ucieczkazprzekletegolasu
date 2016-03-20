using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

class SurfaceChunk
{
    public Terrain Terrain { get; private set; }
    private TerrainData terrainData;
    public Vector3 Position { get; private set; }
    private int resolution;
    private readonly int alphaResolution;
    private readonly PerlinNoise perlin;
    private readonly SurfaceVisualSettings settings;
    private int height;
    private int size;

    private const float sizeMultipler = 0.3f;

    public Rect Rect { get; private set; }

    public SurfaceChunk(Vector3 position, int size, int height, int resolution,int alphaResolution, PerlinNoise perlin, SurfaceVisualSettings settings)
    {
        this.Position = position;
        this.resolution = resolution;
        this.alphaResolution = alphaResolution;
        this.perlin = perlin;
        this.settings = settings;
        this.height = height;
        this.size = size;
        Prepare();
    }

    private void Prepare()
    {
        terrainData = new TerrainData();
        SetVisualSettings();
        terrainData.heightmapResolution = resolution;
        terrainData.alphamapResolution = alphaResolution;
        terrainData.size = new Vector3(size, height, size);
        terrainData.SetAlphamaps(0,0,GenerateAlphaMap());
        terrainData.SetHeights(0, 0, GenerateHeightMap());
        var threshold = size*sizeMultipler;
        Rect = new Rect(Position.x-threshold,Position.z - threshold,size+threshold,size + threshold);
    }

    private void SetVisualSettings()
    {
        terrainData.splatPrototypes = new SplatPrototype[] {settings.GreenSplat,settings.DustSplat};
        terrainData.treePrototypes = settings.PlantPrototypes;
    }

    public void Draw()
    {
        Terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
        Terrain.transform.position = Position;
        Terrain.castShadows = false;
        Terrain.basemapDistance = 1000;
        AddTrees();
    }

    private float[,] GenerateHeightMap()
    {

        float[,] heightMap = new float[resolution, resolution];
        for (int x = 0; x < resolution; x++)
        {
            for (int z = 0; z < resolution; z++)
            {
                var posX = (Position.x/size + (float) (x)/(resolution-1));
                var posZ = (Position.z/size+ (float) (z)/(resolution-1));
        
                heightMap[z, x] = perlin.Perlin2D(new Vector3(posX,posZ)) + 0.5f;

            }       
        }
        return heightMap;
    }

    private float[,,] GenerateAlphaMap()
    {
        float[,,] alphaMap = new float[alphaResolution,alphaResolution,2];
        for (int x = 0; x < alphaResolution; x++)
        {
            for (int z = 0; z < alphaResolution; z++)
            {
                var posX = (Position.x / size + (float)(x) / (alphaResolution - 1));
                var posZ = (Position.z / size + (float)(z) / (alphaResolution - 1));
                //                float steepness = terrainData.GetSteepness(nX, nZ)/90f;
                var noise = Mathf.Min(perlin.Perlin2D(new Vector3(posX,posZ)) + 0.8f,1f);
                alphaMap[z, x, 0] = noise;
                alphaMap[z, x, 1] = 1 - noise;

            }
        }
        return alphaMap;
    }

    private void AddTrees()
    {
//        Terrain.treeDistance = 20f;
        Terrain.treeBillboardDistance = 50f;
        Terrain.treeCrossFadeLength = 10f;
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                var nX = x/(float) size;
                var nZ = z/(float) size;
                var noise = perlin.Perlin2D(new Vector3(nX, nZ))+0.5f;
                if (noise < 0.3f && Random.value < 0.015f)
                {
                    AddTree(nX, terrainData.GetInterpolatedHeight(nX, nZ), nZ, false);
                }
                else if(Random.value < 0.015f)
                {
                    AddTree(nX, terrainData.GetInterpolatedHeight(nX, nZ), nZ, true);
                }
            }
        }
    }

    private void AddTree(float x,float y,float z,bool tree)
    {
        TreeInstance plant = new TreeInstance();
        plant.position = new Vector3(x,y,z);
        if (tree)
        {
            plant.prototypeIndex = UnityEngine.Random.Range(0, settings.TreePrototypes.Length);
        }
        else
        {
            plant.prototypeIndex = UnityEngine.Random.Range(settings.TreePrototypes.Length,
                settings.PlantPrototypes.Length);
        }
        plant.widthScale = 2;
        plant.heightScale = 2;
        plant.color = Color.white;
        plant.lightmapColor = Color.white;
        Terrain.AddTreeInstance(plant);
//        Debug.Log("tree placed");
    }
}

