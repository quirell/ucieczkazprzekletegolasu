using System;
using System.Collections.Generic;
using UnityEngine;
public class SurfaceCreator : MonoBehaviour
{
    [Range(1, 200)]
    public int resolution = 17;
    public int size = 256;

    public float frequency = 1f;

    [Range(1, 8)]
    public int octaves = 1;

    [Range(1f, 4f)]
    public float lacunarity = 2f;

    [Range(0f, 1f)]
    public float persistence = 0.5f;

    public int seed = 1000;
    public int height = 1;
    private const int ChunksNumber = 3;
    public Transform prefab;

    private PerlinNoise perlin;
    private List<SurfaceChunk> surfaceChunks = new List<SurfaceChunk>();
    private SurfaceChunk currentChunk;
    private float diagSize;
    private Perlin2 perlin2 = new Perlin2(1080);
//    public void Redraw()
//    {
//        CreateGrid();
//    }

    void Start()
    {
        prefab = (Transform)Instantiate(prefab, new Vector3(ChunksNumber*size/2, 10, ChunksNumber*size/2), Quaternion.identity);
        diagSize = size*Mathf.Sqrt(2);
        perlin = new PerlinNoise(new PerlinHash(256,seed).generate(),frequency,octaves,lacunarity,persistence);
        GenerateTerrain();
    }

    void Update()
    {
        if (!currentChunk.Rect.Contains(OriginPos2D))
        {
            currentChunk = surfaceChunks.Find(c => c.Rect.Contains(OriginPos2D));
            Debug.LogFormat("new chunk pos {0} {1}",currentChunk.Position.x,currentChunk.Position.z);
            var toRemove = surfaceChunks.FindAll(c => Vector3.Distance(currentChunk.Position, c.Position) - diagSize > 1);
            foreach (var chunk in toRemove)
            {
                Destroy(chunk.Terrain);
                surfaceChunks.Remove(chunk);
                var newPosition = -chunk.Position + 2*currentChunk.Position; //odbicie lustrzane względem układu współrzędnych o środku w punkcie currentChunk
                if (toRemove.Count == 3)
                {
                    if (Mathf.Abs(newPosition.x - currentChunk.Position.x) - size < 1)
                        //przysunięcie współrzędnej oddalonej od 2*size tak by stykała się ze starą powierzchnią
                        newPosition = new Vector3(Mathf.Floor(newPosition.x), 0,
                            Mathf.Floor(newPosition.z + (newPosition.z > currentChunk.Position.z ? -size : size)));
                    else
                        newPosition =
                            new Vector3(
                                Mathf.Floor(newPosition.x + (newPosition.x > currentChunk.Position.x ? -size : size)), 0,
                                Mathf.Floor(newPosition.z));
                }
                else
                {
                    int empty;
                    if (newPosition.x > currentChunk.Position.x || newPosition.z > currentChunk.Position.z)
                        empty = -size;
                    else
                        empty = size;
                        newPosition =
                            new Vector3(Mathf.Floor(newPosition.x + empty), 0,Mathf.Floor(newPosition.z + empty));
                }

                Debug.LogFormat("new {0} old {1}",newPosition,chunk.Position);
                var newChunk = new SurfaceChunk(newPosition, size, height, resolution,perlin,perlin2);
                surfaceChunks.Add(newChunk);
                newChunk.Draw();
            }
        }
    }

    private void GenerateTerrain()
    {
        for (int x = 0; x < ChunksNumber*size; x += size)
        {
            for (int z = 0; z < ChunksNumber*size; z += size)
            {
                var chunk = new SurfaceChunk(new Vector3(x,0,z),size,height,resolution,perlin,perlin2);
                chunk.Draw();
                surfaceChunks.Add(chunk);
            }
        }
        currentChunk = surfaceChunks[4];
    }

    //    private void OnEnable()
    //    {
    //        Redraw();
    //    }

    

    private void UpdateTerrain()
    {

    }


    private Vector2 OriginPos2D
    {
        get
        {
            var pos = prefab.Find("monster1AnimsNormal").transform.position;
            return new Vector2(pos.x, pos.z);
        }

    }

}
