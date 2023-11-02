using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using TerrainData = Data.TerrainData;

public class MapDataGenerator : MonoBehaviour
{
    private readonly int _sizeX;
    private readonly int _sizeY;
    private readonly float _maxHeight;
    private readonly float _generatorFrequency;
    
    public MapDataGenerator(int sizeX, int sizeY, float maxHeight, float generatorFrequency)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        _maxHeight = maxHeight;
        _generatorFrequency = generatorFrequency;
    }
    
    public TileMapData GenerateMapData()
    {
        TileMapData tileMapData = new TileMapData(_sizeX, _sizeY);
        InitTiles(tileMapData);

        return tileMapData;
    }
    
    private void InitTiles(TileMapData tileMapData)
    {
        TerrainType[] terrainTypes = MaterialPropertiesFactory.GetAllowedInitialTerrainTypes();
            
        float terrainSeed = Random.Range(0.0f, 200.0f);
        float heightSeed = Random.Range(0.0f, 200.0f);

        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                int generatedType = GenerateTerrainType(x, y, terrainSeed, terrainTypes.Length);
                float generatedHeight = GenerateTerrainHeight(x, y, heightSeed);

                TerrainType terrainType = terrainTypes[generatedType];
                MaterialProperties materialProperties = MaterialPropertiesFactory.GetProperties(terrainType);
                    
                TerrainData terrainData = new TerrainData(terrainType, generatedHeight, materialProperties);
                tileMapData.SetTileData(x, y, new TileData(x, y, terrainData));
            }
        }

    }
    
    private int GenerateTerrainType(int x, int y, float terrainSeed, int terrainTypes)
    {
        float perlinNoise = Mathf.PerlinNoise((x + terrainSeed) / _generatorFrequency, (y + terrainSeed) / _generatorFrequency);
        float step = 1.1f / terrainTypes;

        return (int) (perlinNoise / step);
    }
        
    private float GenerateTerrainHeight(int x, int y, float heightSeed)
    {
        float perlinNoise = Mathf.PerlinNoise((x + heightSeed) / _generatorFrequency, (y + heightSeed) / _generatorFrequency);
        return perlinNoise * _maxHeight;
    }
}
