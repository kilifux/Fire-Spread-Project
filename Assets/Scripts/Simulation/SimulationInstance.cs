using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Simulation;
using UnityEngine;

public class SimulationInstance : MonoBehaviour
{
    public int sizeX = 100;
    public int sizeY = 100;
    public float maxHeight = 2.0f;
    public int generatorFrequency = 10;
    
    public float windDirection = 0f;
    public float windSpeed = 20.0f;
    public float moistureContent = 0.7f;
    
    public TileGUIInfo tileGuiInfo;
    public TimeDisplayController timeDisplayController;
    
    private TileMap _tileMap;
    private TileMapData _tileMapData;
    
    private bool startedFire = false;
    private SimulationVariablesCalculator simulationVariablesCalculator;
    private int _simulationSpeed = 30;

    private float _elapsedTime = 0f;
    
    void Start()
    {
        RebuildSimulation();
        simulationVariablesCalculator = new SimulationVariablesCalculator();
    }

    private void FixedUpdate()
    {
        if(startedFire)
        {
            UpdateTimer();

            List<TileData> burningTiles = GetBurningTiles();
                
            foreach (TileData tile in burningTiles)
            {
                List<TileData> neighbours = GetUnburningNeighbours(tile);

                BurnNeighbours(tile, neighbours);
            }
        }
    }
    
    private void UpdateTimer()
    {
        float minutesSinceLastFrame = Time.deltaTime * _simulationSpeed / 60.0f;
        _elapsedTime += minutesSinceLastFrame;
            
        timeDisplayController.UpdateTimerDisplay(_elapsedTime);
    }
    
    public void RebuildSimulation()
    {
        GenerateData();
        _tileMap = gameObject.GetComponent<TileMap>();
        _tileMap.InitTileMap(sizeX, sizeY, _tileMapData);
    }
    
    public void SetSimulationSpeed(int newSimulationSpeed)
    {
        _simulationSpeed = newSimulationSpeed;
        Debug.Log("New simulation speed: " + newSimulationSpeed);
    }

    public void UpdateWindConfig(float speed, float direction)
    {
        windSpeed = speed;
        windDirection = direction;
    }
    
    private void GenerateData()
    {
        MapDataGenerator mapDataGenerator = new MapDataGenerator(sizeX, sizeY, maxHeight, generatorFrequency);
        _tileMapData = mapDataGenerator.GenerateMapData();
    }
public void StartFire()
        {
            TileData selectedTile = tileGuiInfo.CurrentlySelectedTile;
            BurnTile(selectedTile);
            startedFire = true;

        }

        private float CalculateSlopeSteepness(TileData selectedTile, TileData anotherTile)
        {
            float slopeSteepness = (anotherTile.TerrainData.Height - selectedTile.TerrainData.Height)
                  / _tileMap.tileSize;

            return slopeSteepness;
        }

        private float CalculateWholeDistance(TileData selectedTile, TileData anotherTile)
        {
            double xDistance = Math.Pow(selectedTile.PositionX - anotherTile.PositionX, 2);
            double yDistance = Math.Pow(selectedTile.PositionY - anotherTile.PositionY, 2);
            float distance = _tileMap.tileSize * (float) Math.Sqrt(xDistance + yDistance);

            return distance;
        }

        private List<TileData> GetUnburningNeighbours(TileData selectedTile)
        {
            List<TileData> neighbours = new List<TileData>();
            for (int x = selectedTile.PositionX - 1; x <= selectedTile.PositionX + 1; x++)
            {
                if (x < 0 || x >=_tileMap._tileMapData.GetTileData().Length)
                    continue;

                for (int y = selectedTile.PositionY - 1; y <= selectedTile.PositionY + 1; y++)
                {
                    if (y < 0 || y >= _tileMap._tileMapData.GetTileData()[x].Length)
                        continue;
                    if (x == selectedTile.PositionX && y == selectedTile.PositionY)
                        continue;

                    if(!_tileMapData.GetTileData(x, y).IsBurning)
                    {
                        neighbours.Add(_tileMapData.GetTileData(x, y));
                    }
                }
            }

            return neighbours;
        }

        private List<TileData> GetBurningTiles()
        {
            List<TileData> burning = new List<TileData>();

            foreach (TileData[] x in _tileMap._tileMapData.GetTileData())
            {
                foreach (TileData y in x)
                {
                    if(y.IsBurning)
                    {
                        burning.Add(y);
                    }
                }
            }

            return burning;
        }

        private void BurnTile(TileData tile)
        {
            tile.IsBurning = true;
            tile.TerrainData.Type = TerrainType.Burning;
            _tileMap.UpdateTexture(tile);
        }

        private void BurnNeighbours(TileData tile, List<TileData> neighbours)
        {
            foreach (TileData neighbour in neighbours)
            {
                float angle = CalculateAngle(tile, neighbour);
                float cosAngle = Mathf.Cos(angle - Mathf.Deg2Rad * windDirection);
                cosAngle = cosAngle < 0 ? 0 : cosAngle;

                float deltaDistance = CalculateBurnedDistance(tile, neighbour, cosAngle);
                float wholeDistance = CalculateWholeDistance(tile, neighbour);

                string neighbourKey = neighbour.PositionX.ToString() + neighbour.PositionY.ToString();

                if (tile.FireSpreadingDistance.ContainsKey(neighbourKey))
                {
                    tile.FireSpreadingDistance[neighbourKey] += deltaDistance;

                }
                else
                {
                    tile.FireSpreadingDistance.Add(neighbourKey, deltaDistance);
                }

                if (tile.FireSpreadingDistance[neighbourKey] >= wholeDistance)
                {
                    BurnTile(neighbour);
                }
            }
        }

        private float CalculateAngle(TileData tile, TileData neighbour)
        {

            float b_x = neighbour.PositionY - tile.PositionY;
            float b_y = neighbour.PositionX - tile.PositionX;

            float angle = 0.0f;

            if(b_x == 0.0f)
            {
                if(b_y == -1.0f)
                {
                    angle = Mathf.PI * 3.0f / 2.0f;
                }
                if(b_y == 1.0f)
                {
                    angle = Mathf.PI / 2.0f;
                }
            }
            if(b_x == -1.0f)
            {
                if (b_y == -1.0f)
                {
                    angle = Mathf.PI * 5.0f / 4.0f;
                }
                if (b_y == 0.0f)
                {
                    angle = Mathf.PI;
                }
                if (b_y == 1.0f)
                {
                    angle = Mathf.PI * 3.0f / 4.0f;
                }
            }
            if (b_x == 1.0f)
            {
                if (b_y == -1.0f)
                {
                    angle = Mathf.PI * 7.0f / 4.0f;
                }
                if (b_y == 0.0f)
                {
                    angle = 0.0f;
                }
                if (b_y == 1.0f)
                {
                    angle = Mathf.PI / 4.0f;
                }
            }

            return angle;
        }

        private float CalculateBurnedDistance(TileData tile, TileData neighbour, float cosAngle)
        {
            float slopeSteepness = CalculateSlopeSteepness(tile, neighbour);
            simulationVariablesCalculator.CalculateVariables(neighbour.TerrainData, slopeSteepness,
                moistureContent, windSpeed, cosAngle);

            float timeSinceLastFrame = Time.deltaTime / 60.0f;
            float burningSpeed = simulationVariablesCalculator.RateOfSpread;

            float deltaDistance = timeSinceLastFrame * burningSpeed * _simulationSpeed;

            return deltaDistance;
        }
}

