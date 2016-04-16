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
    public Vector3 Position { get; private set; }
    public Rect Rect { get; private set; }
    private TerrainData terrainData;
    private readonly SurfaceVisualSettings settings;
    private const float sizeMultipler = 0.3f;

    public SurfaceChunk(Vector3 position, SurfaceVisualSettings settings)
    {
        this.Position = position;
        this.settings = settings;
        PrepareTerrainData();
    }

    private void PrepareTerrainData()
    {
        terrainData = new TerrainData();
        SetPrototypesSettings();
        terrainData.heightmapResolution = settings.resolution;
        terrainData.alphamapResolution = settings.alphaResolution;
        terrainData.size = new Vector3( settings.size, settings.height,  settings.size);
        terrainData.SetAlphamaps(0,0,GenerateAlphaMap());
        terrainData.SetHeights(0, 0, GenerateHeightMap());
        terrainData.SetDetailResolution(settings.detailResolution,16);
        AddDetails();
        var threshold =  settings.size*sizeMultipler;
        Rect = new Rect(Position.x-threshold,Position.z - threshold, settings.size+threshold,settings.size + threshold);
    }

    private void SetPrototypesSettings()
    {
        terrainData.splatPrototypes = new SplatPrototype[] {settings.GreenSplat,settings.DustSplat};
        terrainData.treePrototypes = settings.PlantPrototypes;
        terrainData.detailPrototypes = settings.DetailPrototypes;
    }

    public void Draw()
    {
        Terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
        Terrain.transform.position = Position;
        Terrain.castShadows = false;
        Terrain.basemapDistance = 1000;
        Terrain.detailObjectDistance = 30f;
        AddTrees();
    }

    private float[,] GenerateHeightMap()
    {

        float[,] heightMap = new float[settings.resolution, settings.resolution];
        for (int x = 0; x < settings.resolution; x++)
        {
            for (int z = 0; z < settings.resolution; z++)
            {
                var posX = (Position.x/ settings.size + (float) (x)/(settings.resolution-1));
                var posZ = (Position.z/ settings.size+ (float) (z)/(settings.resolution-1));
        
                heightMap[z, x] = settings.Perlin.Perlin2D(new Vector3(posX,posZ)) + 0.5f;
            }       
        }
        return heightMap;
    }

    private float[,,] GenerateAlphaMap()
    {
        float[,,] alphaMap = new float[ settings.alphaResolution, settings.alphaResolution,2];
        for (int x = 0; x <  settings.alphaResolution; x++)
        {
            for (int z = 0; z <  settings.alphaResolution; z++)
            {
                var posX = (Position.x /  settings.size + (float)(x) / ( settings.alphaResolution - 1));
                var posZ = (Position.z /  settings.size + (float)(z) / ( settings.alphaResolution - 1));
                var noise = Mathf.Min(settings.Perlin.Perlin2D(new Vector3(posX,posZ)) + 0.8f,1f);
                alphaMap[z, x, 0] = noise;
                alphaMap[z, x, 1] = 1 - noise;

            }
        }
        return alphaMap;
    }

    private void AddTrees()
    {
        Terrain.treeBillboardDistance = 50f;
        Terrain.treeCrossFadeLength = 10f;
        for (int x = 0; x <  settings.size; x++)
        {
            for (int z = 0; z <  settings.size; z++)
            {
                var nX = x/(float)  settings.size;
                var nZ = z/(float)  settings.size;
                var noise = settings.Perlin.Perlin2D(new Vector3(nX, nZ))+0.5f;
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

    private void AddDetails()
    {
        var random = new System.Random();
        int[][,] detailMaps = new int[settings.DetailPrototypes.Length][,];
        for (var i = 0; i < detailMaps.Length; i++)
        {
            detailMaps[i] = new int[settings.detailResolution, settings.detailResolution];
        }
        for (int x = 0; x < settings.detailResolution; x++)
        {
            for (int z = 0; z < settings.detailResolution; z++)
            {
                if(Random.value < 0.7f)
                    detailMaps[random.Next(settings.GrassPrototypes.Length)][z,x] = 1;
                else if(Random.value < 0.01f)
                    detailMaps[random.Next(settings.GrassPrototypes.Length,detailMaps.Length)][z, x] = 1;
            }
        }
        for (int i = 0; i < detailMaps.Length; i++)
        {
            terrainData.SetDetailLayer(0,0,i,detailMaps[i]);
        }
    }

    private void AddTree(float x,float y,float z,bool tree)
    {
        TreeInstance plant = new TreeInstance();
        plant.position = new Vector3(x,y,z);
        if (tree)
        {
            plant.prototypeIndex = Random.Range(0, settings.TreePrototypes.Length);
        }
        else
        {
            plant.prototypeIndex = Random.Range(settings.TreePrototypes.Length,
                settings.PlantPrototypes.Length);
        }
        plant.widthScale = 2;
        plant.heightScale = 2;
        plant.color = Color.white;
        plant.lightmapColor = Color.white;
        Terrain.AddTreeInstance(plant);
    }

}

