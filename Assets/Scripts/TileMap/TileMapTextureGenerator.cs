using System.Collections;
using Data;
using System.Collections.Generic;
using UnityEngine;

public class TileMapTextureGenerator : MonoBehaviour
{
    private readonly Texture2D _textureAtlas;
    private readonly List<Color[]> _extractedTextures;
    private readonly int _tileResolution;
    private readonly int _tileSizeX;
    private readonly int _tileSizeZ;
    
    public TileMapTextureGenerator(Texture2D textureAtlas, int tileResolution, int tileSizeX, int tileSizeZ)
    {
        _textureAtlas = textureAtlas;
        _tileResolution = tileResolution;
        _tileSizeX = tileSizeX;
        _tileSizeZ = tileSizeZ;
        
        _extractedTextures = ExtractTexturesFromAtlas();
    }
    
    public Texture2D GenerateTexture(TileMapData tileMapData)
    {
        return GenerateTextureData(tileMapData);
    }
    
    public List<Color[]> ExtractTexturesFromAtlas()
    {
        int tilesInARow = _textureAtlas.width / _tileResolution;
        int tileRowsCount = _textureAtlas.height / _tileResolution;

        List<Color[]> extractedTextures = new List<Color[]>(tileRowsCount * tilesInARow);
        for (int row = 0; row < tileRowsCount; row++)
        {
            for (int tileInARow = 0; tileInARow < tilesInARow; tileInARow++)
            {
                int x = tileInARow * _tileResolution;
                int y = row * _tileResolution;
                extractedTextures.Add(_textureAtlas.GetPixels(x, y, _tileResolution, _tileResolution));
            }
        }

        return extractedTextures;
    }
    
    private Texture2D GenerateTextureData(TileMapData tileMapData)
    {
        int textureWidth = _tileResolution * _tileSizeX;
        int textureHeight = _tileResolution * _tileSizeZ;
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        
        for (int y = 0; y < _tileSizeZ; y++)
        {
            for (int x = 0; x < _tileSizeX; x++)
            {
                TileData tileData = tileMapData.GetTileData(x, y);
                int textureIndex = MapTerrainToTexture(tileData.TerrainData.Type);
                Color[] extractedTexture = _extractedTextures[textureIndex];
                texture.SetPixels(x * _tileResolution, y * _tileResolution, _tileResolution, _tileResolution, extractedTexture);
            }
        }

        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    }
    
    public int MapTerrainToTexture(TerrainType type)
    {
        switch (type)
        {
            case TerrainType.Grass:
                return 0;
            case TerrainType.Tree:
                return 1;
            case TerrainType.Bush:
                return 2;
            case TerrainType.HighGrass:
                return 3;
            case TerrainType.Burning:
                return 4;
            default:
                return 0;
        }
    }
    
    public void UpdateTexture(Texture2D sharedMaterialMainTexture, TileData tileUnderMouse)
    {
        Color[] extractedTexture = GetExtractedTexture(tileUnderMouse);
        sharedMaterialMainTexture.SetPixels(tileUnderMouse.PositionX * _tileResolution,
            tileUnderMouse.PositionY * _tileResolution, _tileResolution, _tileResolution, extractedTexture);
        sharedMaterialMainTexture.Apply();
    }

    private Color[] GetExtractedTexture(TileData tileUnderMouse)
    {
        int textureIndex = MapTerrainToTexture(tileUnderMouse.TerrainData.Type);
        Color[] extractedTexture = _extractedTextures[textureIndex];
        return extractedTexture;
    }
}
