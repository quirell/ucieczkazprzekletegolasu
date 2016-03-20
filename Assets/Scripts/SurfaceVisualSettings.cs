using UnityEngine;
using System.Collections;
using System.Linq;

public class SurfaceVisualSettings : MonoBehaviour {

    public GameObject[] trees;
    public GameObject[] bushes;
    public GameObject[] grasses;
    public GameObject[] boulders;
    public Texture2D green;
    public Texture2D dust;

    void Awake()
    {
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


        
    }


    public SplatPrototype GreenSplat { get; private set; }
    public SplatPrototype DustSplat { get; private set; }

    public TreePrototype[] TreePrototypes { get; private set; }
    public TreePrototype[] BushPrototypes { get; private set; }

    public DetailPrototype[] GrassPrototypes { get; private set; }

    public DetailPrototype[] BoulderPrototypes { get; private set; }

    public TreePrototype[] PlantPrototypes { get; private set; }
}
