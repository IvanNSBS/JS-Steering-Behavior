﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World instance;
    Noise noise;

    [Header("World Settings")]
    public int width;
    public int height;
    public WorldTile[,] tiles;
    public Material material;
    public GameObject WaterPrefab;

    [Header("Noise Settings")]
    public string seed;
    public bool randomSeed;
    public float frequency;
    public float amplitude;
    public float lacunarity;
    public float persistance;
    public int octaves;

    [Header("Heights Settings")]
    public float seaLevel;
    public float dirtStartHeight;
    public float dirtEndHeight;
    public float grassStartHeight;
    public float grassEndHeight;
    public float highGrassStartHeight;
    public float highGrassEndHeight;

    void Awake()
    {
        instance = this;

        if (randomSeed)
        {
            int value = Random.Range(-10000, 10000);
            seed = value.ToString();
        }

        noise = new Noise(seed.GetHashCode(), frequency, amplitude, lacunarity, persistance, octaves);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateTiles();
        SubdivideTilesArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateTiles()
    {
        tiles = new WorldTile[width, height];
        float[,] noiseValues = noise.GetNoiseValues(width, height);

        for(int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tiles[i, j] = MakeTileAtHeight(noiseValues[i, j]);
            }
        }

        SpawnWaterPrefab();
    }

    WorldTile MakeTileAtHeight (float currentHeight)
    {
        if (currentHeight <= seaLevel)
            return new WorldTile(WorldTile.Type.Water);

        if (currentHeight >= grassStartHeight && currentHeight <= grassEndHeight)
            return new WorldTile(WorldTile.Type.Grass);

        if (currentHeight >= highGrassStartHeight && currentHeight <= highGrassEndHeight)
            return new WorldTile(WorldTile.Type.HighGrass);

        if (currentHeight >= dirtStartHeight && currentHeight <= dirtEndHeight)
            return new WorldTile(WorldTile.Type.Dirt);

        return new WorldTile(WorldTile.Type.Void);
    }

    void SubdivideTilesArray (int index1 = 0, int index2 = 0)
    {
        if (index1 > tiles.GetLength(0) && index2 > tiles.GetLength(1))
            return;

        int sizeX, sizeY;

        if (tiles.GetLength(0) - index1 > 100)
            sizeX = 100;
        else
            sizeX = tiles.GetLength(0) - index1;

        if (tiles.GetLength(1) - index2 > 100)
            sizeY = 100;
        else
            sizeY = tiles.GetLength(1) - index2;

        GenerateMesh(index1, index2, sizeX, sizeY);

        if (tiles.GetLength(0) > index1 + 100)
        {
            SubdivideTilesArray(index1 + 100, index2);
            return;
        }

        if (tiles.GetLength(1) > index2 + 100)
        {
            SubdivideTilesArray(0, index2 + 100);
            return;
        }
    }

    void GenerateMesh(int x, int y, int width, int height)
    {
        MeshData data = new MeshData(x, y, width, height);

        GameObject meshGameObject = new GameObject("Chunk " + x + ", " + y);
        meshGameObject.transform.SetParent(this.transform);

        MeshFilter filter = meshGameObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = meshGameObject.AddComponent<MeshRenderer>();
        renderer.material = material;

        Mesh mesh = filter.mesh;
        mesh.vertices = data.vertices.ToArray();
        mesh.triangles = data.triangles.ToArray();
        mesh.uv = data.UVs.ToArray();
    }

    public WorldTile GetTileAt(int x, int y)
    {
        if(x < 0 || x >= width || y < 0 || y >= height)
        {
            return null;
        }
        return tiles[x, y];
    }

    void SpawnWaterPrefab()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tiles[i, j].type == WorldTile.Type.Water)
                {
                    Vector3 position = new Vector3(i + 0.5f, j + 0.5f, 0);
                    GameObject go = Instantiate(WaterPrefab, position,Quaternion.identity);
                    go.transform.SetParent(this.gameObject.transform);
                }
            }
        }
    }
}
