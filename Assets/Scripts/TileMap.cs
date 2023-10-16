using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour
{
    [SerializeField] private int sizeX = 24;
    [SerializeField] private int sizeZ = 24;
    [SerializeField] private float tileSize = 8f;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    void Start()
    {
        BuildMap();
    }
    
    void BuildMap()
    {
        TileMapGenerator tileMapGenerator = new TileMapGenerator(sizeX, sizeZ, tileSize);

        Mesh mesh = tileMapGenerator.GenerateMap();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }
}
