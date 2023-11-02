using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class SimulationInstance : MonoBehaviour
{
    public int sizeX = 100;
    public int sizeY = 100;
    public float maxHeight = 2.0f;
    public int generatorFrequency = 10;
    
    private TileMap _tileMap;
    private TileMapData _tileMapData;
    
    void Start()
    {
        RebuildSimulation();
    }


    public void RebuildSimulation()
    {
        GenerateData();
        _tileMap = gameObject.GetComponent<TileMap>();
        _tileMap.InitTileMap(sizeX, sizeY, _tileMapData);
    }
    
    private void GenerateData()
    {
        MapDataGenerator mapDataGenerator = new MapDataGenerator(sizeX, sizeY, maxHeight, generatorFrequency);
        _tileMapData = mapDataGenerator.GenerateMapData();
    }

}
