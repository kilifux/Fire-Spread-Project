using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    
    
}
