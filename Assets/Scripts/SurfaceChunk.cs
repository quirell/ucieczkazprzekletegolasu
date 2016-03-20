using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class SurfaceChunk
{
    public Terrain Terrain { get; private set; }
    private TerrainData terrainData;
    public Vector3 Position { get; private set; }
    private int resolution;
    private readonly PerlinNoise perlin;
    private int height;
    private int size;
    private Perlin2 perlin2;
    private const float sizeMultipler = 0.3f;

    public Rect Rect { get; private set; }

    public SurfaceChunk(Vector3 position, int size, int height, int resolution, PerlinNoise perlin,Perlin2 perlin2)
    {
        this.Position = position;
        this.resolution = resolution;
        this.perlin = perlin;
        this.height = height;
        this.size = size;
        this.perlin2 = perlin2;
        Prepare();
    }

    private void Prepare()
    {
        terrainData = new TerrainData();
        terrainData.heightmapResolution = resolution;
        terrainData.size = new Vector3(size, height, size);
        terrainData.SetHeights(0, 0, GenerateHeightMap());
        var threshold = size*sizeMultipler;
        Rect = new Rect(Position.x-threshold,Position.z - threshold,size+threshold,size + threshold);
    }

    public void Draw()
    {
        Terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
        Terrain.transform.position = Position;
        Terrain.castShadows = false;
        Terrain.basemapDistance = 1000;
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
        
                heightMap[z, x] = perlin.Perlin2D(new Vector3(posX, posZ, 0)) + 0.5f;

            }       
        }
        return heightMap;
    }
}

