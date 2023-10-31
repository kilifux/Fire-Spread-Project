using System.Collections;
using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileTypesPanel : MonoBehaviour
{
    [SerializeField] private GameObject terrainTypesPanel;
    [SerializeField] private TileTypeDataEditor tileTypeDataEditor;
    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private TileMap tileMap;

    private List<GameObject> _terrainButtons = new List<GameObject>();

    private GameObject _currentlySelectedButton;
    
    public TerrainType CurrentlySelectedTerrainType { get; private set; }

    private Dictionary<TerrainType, MaterialProperties> _materialProperties = new Dictionary<TerrainType, MaterialProperties>();

    private GameObject _buttonHighlight;
    private bool _isInEditMode;





    private void GenerateButtonsForTileTypes()
    {
        TerrainType[] terrainTypesValues = MaterialPropertiesFactory.GetAllowedInitialTerrainTypes();
        
    }

    
    private void InitTerrainTypeMaterialProperties(TerrainType[] terrainTypesValues)
    {
        foreach (TerrainType terrainType in terrainTypesValues)
        {
            MaterialProperties materialProperties = MaterialPropertiesFactory.GetProperties(terrainType);
            _materialProperties.Add(terrainType, materialProperties);
        }
    }
}