using UnityEngine;
using System.Collections;
class PerlinHash
{
    int[] hash; 


    public PerlinHash(int size, int seed)
    {
        hash = new int[size * 2];
        Random.seed = seed;

    }
    public int[] generate()
    {
        var size = hash.Length/2;
        for(int i = 0;i< size; i++)
        {
            hash[i] = i;
        }
        for(int i = 0;i < size; i++)
        {
            swapHash(i, Random.Range(0, size));
        }
        System.Array.Copy(hash, 0, hash, size, size);
        return hash;
    }

    private void swapHash(int i, int j)
    {
        int ih = hash[i];
        hash[i] = hash[j];
        hash[j] = ih;
    }
}

