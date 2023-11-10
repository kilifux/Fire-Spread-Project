using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class TileMapGenerator
{
    private int size_x;
    private int size_z;
    private float tileSize;

    public TileMapGenerator(int sizeX, int sizeZ, float tileSize)
    {
        size_x = sizeX;
        size_z = sizeZ;
        this.tileSize = tileSize;
    }
    
    public Mesh GenerateMap(TileMapData tileMapData)
    {
        int tilesCount = size_x * size_z;

        int verticesSizeX = size_x + 1;
        int verticesSizeZ = size_z + 1;
        
        int verticesCount = verticesSizeX * verticesSizeZ;
        int trianglesCount = 2 * tilesCount;
        Vector3[] mapVertices = new Vector3[verticesCount];
        Vector3[] normalVertices = new Vector3[verticesCount];
        Vector2[] uvVertices = new Vector2[verticesCount];
        
        int[] triangleVertices = new int[trianglesCount * 3];

        InitMapVertices(verticesSizeZ, verticesSizeX, mapVertices, normalVertices, uvVertices, tileMapData);
        InitTriangleVertices(verticesSizeX, triangleVertices);

        Mesh mesh = new Mesh
        {
            vertices = mapVertices, triangles = triangleVertices, normals = normalVertices, uv = uvVertices,
            name = "TileMapMesh"
        };

        return mesh;
    }

    private void InitTriangleVertices(int verticesSizeX, int[] triangleVertices)
    {
        for (int z = 0; z < size_z; z++)
        {
            for (int x = 0; x < size_x; x++)
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
        result[0] = meshVertices[coordinates.y * (size_x + 1) + coordinates.x];
        result[1] = meshVertices[coordinates.y * (size_x + 1) + coordinates.x + 1];
        result[2] = meshVertices[(coordinates.y + 1) * (size_x + 1) + coordinates.x];
        result[3] = meshVertices[(coordinates.y + 1) * (size_x + 1) + coordinates.x + 1];

        return result;
    }
    
    private int GetTriangleIndex(int x, int y)
    {
        int squareIndex = y * size_x + x;
        int triangleIndex = squareIndex * 6;
        return triangleIndex;
    }

    private void InitMapVertices(int verticesSizeZ, int verticesSizeX, Vector3[] mapVertices, Vector3[] normalVertices,
        Vector2[] uvVertices, TileMapData tileMapData)
    {
        for (int z = 0; z < verticesSizeZ; z++)
        {
            for (int x = 0; x < verticesSizeX; x++)
            {
                float vertexHeight = GetMeanVertexHeight(x, z, tileMapData);
                int currentVertexIndex = z * verticesSizeX + x;
                mapVertices[currentVertexIndex] = new Vector3(x * tileSize, vertexHeight, z * tileSize);
                normalVertices[currentVertexIndex] = Vector3.up;
                uvVertices[currentVertexIndex] = new Vector2((float) x / (verticesSizeX - 1), (float) z / (verticesSizeZ - 1));
            }
        }
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