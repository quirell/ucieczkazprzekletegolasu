using System;
using System.Collections.Generic;
using UnityEngine;
public class SurfaceCreator : MonoBehaviour
{
    public Transform playerCreature;

    private SurfaceVisualSettings settings;
    private const int ChunksNumber = 3;
    private PerlinNoise perlin;
    private List<SurfaceChunk> surfaceChunks = new List<SurfaceChunk>();
    private SurfaceChunk currentChunk;
    private float diagSize;
    private Queue<Vector3> toCreate = new Queue<Vector3>(5);

    void Start()
    {
        settings = GetComponent<SurfaceVisualSettings>();
		//playerCreature = (Transform)Instantiate(playerCreature, new Vector3(ChunksNumber*settings.size/2, 10, ChunksNumber*settings.size/2), Quaternion.identity);
		playerCreature.position = new Vector3(ChunksNumber*settings.size/2, 10, ChunksNumber*settings.size/2);
		diagSize = settings.size*Mathf.Sqrt(2);
        GenerateTerrain();
    }

    void Update()
    {
        if (!currentChunk.Rect.Contains(OriginPos2D))
        {
            currentChunk = surfaceChunks.Find(c => c.Rect.Contains(OriginPos2D));
            var toRemove = surfaceChunks.FindAll(c => Vector3.Distance(currentChunk.Position, c.Position) - diagSize > 1);
            Vector3 middlePos = new Vector3();
            if (toRemove.Count == 5)
                middlePos = toRemove.Find(c => Mathf.Abs(Vector3.Distance(currentChunk.Position, c.Position) - diagSize*2f) < 1).Position;
            foreach (var chunk in toRemove)
            {
                Destroy(chunk.Terrain);
                surfaceChunks.Remove(chunk);
                var newPosition = -chunk.Position + 2*currentChunk.Position; //odbicie lustrzane względem układu współrzędnych o środku w punkcie currentChunk
                if (toRemove.Count == 3)
                {
                    if (Mathf.Abs(newPosition.x - currentChunk.Position.x) -  settings.size < 1)
                        //przysunięcie współrzędnej oddalonej od 2*size tak by stykała się ze starą powierzchnią
                        newPosition = new Vector3(Mathf.Floor(newPosition.x), 0, Mathf.Floor(newPosition.z + (newPosition.z > currentChunk.Position.z ? - settings.size :  settings.size)));
                    else
                        newPosition =
                            new Vector3(
                                Mathf.Floor(newPosition.x + (newPosition.x > currentChunk.Position.x ? - settings.size :  settings.size)), 0,Mathf.Floor(newPosition.z));
                }
                else
                {
                    int xSize = ComputeTranslation(newPosition.x, currentChunk.Position.x, middlePos.x);
                    int zSize = ComputeTranslation(newPosition.z,currentChunk.Position.z,middlePos.z);

                    newPosition = new Vector3(Mathf.Floor(newPosition.x + xSize), 0,Mathf.Floor(newPosition.z + zSize));

                }

                toCreate.Enqueue(newPosition);
            }
        }else if (toCreate.Count > 0)
        {
            var newChunk = new SurfaceChunk(toCreate.Dequeue(),settings);
            surfaceChunks.Add(newChunk);
            newChunk.Draw();
        }
    }

    private void GenerateTerrain()
    {
        for (int x = 0; x < ChunksNumber* settings.size; x +=  settings.size)
        {
            for (int z = 0; z < ChunksNumber* settings.size; z +=  settings.size)
            {
                var chunk = new SurfaceChunk(new Vector3(x,0,z),settings);
                chunk.Draw();
                surfaceChunks.Add(chunk);
            }
        }
        currentChunk = surfaceChunks[4];
    }

    private int ComputeTranslation(float chunk,float currentChunk,float middleChunk)
    {
        if (Mathf.Abs(chunk - currentChunk) < 1f)
        {
            if (middleChunk > chunk)
            {
                return settings.size;
            }
            else
            {
                return - settings.size;
            }
        }
        else if (chunk > currentChunk)
            return - settings.size;
        else
            return  settings.size;
    }

    private Vector2 OriginPos2D
    {
        get
        {
            var pos = playerCreature.transform.position;
            return new Vector2(pos.x, pos.z);
        }

    }

}
