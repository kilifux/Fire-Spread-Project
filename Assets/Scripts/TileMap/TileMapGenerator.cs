using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Data;
using Random = UnityEngine.Random;

public class TileMapGenerator
{
    private int _sizeX;
    private int _sizeZ;
    private float _tileSize;

    public TileMapGenerator(int size_X, int size_Z, float tileSize)
    {
        _sizeX = size_X;
        _sizeZ = size_Z;
        _tileSize = tileSize;
    }

    public Mesh GenerateMap()
    {
        int tilesCount = _sizeX * _sizeZ;

        int verticesSizeX = _sizeX + 1;
        int verticesSizeZ = _sizeZ + 1;

        int verticesAmount = verticesSizeX * verticesSizeZ;
        int trianglesAmount = 2 * tilesCount;
        
        Vector3[] mapVertices = new Vector3[verticesAmount];
        Vector3[] normalVertices = new Vector3[verticesAmount];
        Vector2[] uvVertices = new Vector2[verticesAmount];

        int[] triangleVertices = new int[trianglesAmount * 3];

        InitMapVertices(verticesSizeZ, verticesSizeX, mapVertices, normalVertices, uvVertices);
        InitTriangleVertices(verticesSizeX, triangleVertices);

        Mesh mesh = new Mesh
        {
            vertices = mapVertices, triangles = triangleVertices, normals = normalVertices, uv = uvVertices,
            name = "TileMapMesh"
        };

        return mesh;
    }

    private void InitMapVertices(int verticesSizeZ, int verticesSizeX, Vector3[] mapVertices, Vector3[] normalVertices,
        Vector2[] uvVertices)
    {
        for (int z = 0; z < verticesSizeZ; z++)
        {
            for (int x = 0; x < verticesSizeX; x++)
            {
                int currentVertexIndex = z * verticesSizeX + x;
                mapVertices[currentVertexIndex] = new Vector3(x * _tileSize, Random.Range(0.5f, 8f), z * _tileSize);
                normalVertices[currentVertexIndex] = Vector3.up;
                uvVertices[currentVertexIndex] = new Vector2((float) x / verticesSizeX, (float) z / verticesSizeZ);
            }
        }
    }
    
    private void InitTriangleVertices(int verticesSizeX, int[] triangleVertices)
    {
        for (int z = 0; z < _sizeZ; z++)
        {
            for (int x = 0; x < _sizeX; x++)
            {
                int triangleIndex = GetTriangleIndex(x, z);
                int triangleVertexOffset = z * verticesSizeX + x;

                triangleVertices[triangleIndex] = triangleVertexOffset;
                triangleVertices[triangleIndex + 1] = triangleVertexOffset + verticesSizeX;
                triangleVertices[triangleIndex + 2] = triangleVertexOffset + verticesSizeX + 1;

                triangleVertices[triangleIndex + 3] = triangleVertexOffset;
                triangleVertices[triangleIndex + 4] = triangleVertexOffset + verticesSizeX + 1;
                triangleVertices[triangleIndex + 5] = triangleVertexOffset + 1;
            }
        }
    }
    
    public Vector3[] GetTrianglesOfATile(Vector2Int coordinates, Vector3[] meshVertices)
    {
        Vector3[] result = new Vector3[4];
        result[0] = meshVertices[coordinates.y * (_sizeX + 1) + coordinates.x];
        result[1] = meshVertices[coordinates.y * (_sizeX + 1) + coordinates.x + 1];
        result[2] = meshVertices[(coordinates.y + 1) * (_sizeX + 1) + coordinates.x];
        result[3] = meshVertices[(coordinates.y + 1) * (_sizeX + 1) + coordinates.x + 1];

        return result;
    }
    
    private int GetTriangleIndex(int x, int y)
    {
        int squareIndex = y * _sizeX + x;
        int triangleIndex = squareIndex * 6;
        return triangleIndex;
    }
    
    private float GetMeanVertexHeight(int vertexX, int vertexY, TileMapData tileMapData)
    {
        List<TileData> tiles = new List<TileData>();
        
        tiles.Add(tileMapData.GetTileData(vertexX - 1, vertexY - 1));
        tiles.Add(tileMapData.GetTileData(vertexX - 1, vertexY));
        tiles.Add(tileMapData.GetTileData(vertexX, vertexY));
        tiles.Add(tileMapData.GetTileData(vertexX, vertexY - 1));
        
        tiles.RemoveAll(item => item == null);
        float sum = tiles.Sum(data => data.TerrainData.Height);
        
        return sum / tiles.Count;
    }
}