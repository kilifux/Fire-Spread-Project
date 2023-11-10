using System;
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

    private List<GameObject> _terrainButtons = new();

    private GameObject _currentlySelectedButton;
    public TerrainType CurrentlySelectedTerrainType { get; private set; }
    private Dictionary<TerrainType, MaterialProperties> _materialProperties = new();

    private GameObject _buttonHighlight;
    private bool _isInEditMode;
    
    void Start()
    {
        GenerateButtonsForTileTypes();
    }
    
    public bool IsButtonSelected()
    {
        return _currentlySelectedButton != null;
    }

    public void OnSaveTypeEditing()
    {
        MaterialProperties materialProperties = tileTypeDataEditor.FinishEditing();
        _materialProperties[CurrentlySelectedTerrainType] = materialProperties;

        CleanupEditingState();
    }

    public void OnCancelTypeEditing()
    {
        tileTypeDataEditor.CancelEditing();
        CleanupEditingState();
    }

    public MaterialProperties GetPropertiesForCurrentSelection()
    {
        return _materialProperties[CurrentlySelectedTerrainType].Clone();
    }

    private void GenerateButtonsForTileTypes()
    {
        TerrainType[] terrainTypesValues = MaterialPropertiesFactory.GetAllowedInitialTerrainTypes();
        InitTerrainTypeMaterialProperties(terrainTypesValues);
        
        TileMapTextureGenerator tileMapTextureGenerator = tileMap.GetTileMapTextureGenerator();
        List<Color[]> texturesFromAtlas = tileMapTextureGenerator.ExtractTexturesFromAtlas();

        RectTransform parentRectTransform = terrainTypesPanel.transform.GetComponent<RectTransform>();
        foreach (TerrainType terrainTypesValue in terrainTypesValues)
        {
            Texture2D texture2D = GetTextureForTerrainType(tileMapTextureGenerator, terrainTypesValue, texturesFromAtlas);
            GameObject button = CreateButton(terrainTypesValue, texture2D);
            _terrainButtons.Add(button);

            RectTransform buttonRectTransform = button.transform.GetComponent<RectTransform>();
            float rectWidth = buttonRectTransform.rect.width / terrainTypesValues.Length;
            float rectHeight = buttonRectTransform.rect.height / terrainTypesValues.Length;
            int buttonOffset = Array.IndexOf(terrainTypesValues, terrainTypesValue);
            button.transform.localPosition = new Vector3(rectWidth * 2,
                (-rectHeight * 5 - rectHeight * buttonOffset * 8), 0);
        }
    }

    private void InitTerrainTypeMaterialProperties(TerrainType[] terrainTypesValues)
    {
        foreach (TerrainType terrainType in terrainTypesValues)
        {
            MaterialProperties materialProperties = MaterialPropertiesFactory.GetProperties(terrainType);
            _materialProperties.Add(terrainType, materialProperties);
        }
    }

    private GameObject CreateButton(TerrainType terrainTypesValue, Texture2D texture2D)
    {
        GameObject button = Instantiate(buttonPrefab, terrainTypesPanel.transform, false);
        button.GetComponent<CustomClickHandler>().onLeft.AddListener(() => HandleLeftButtonClick(button, terrainTypesValue));
        button.GetComponent<CustomClickHandler>().onRight.AddListener(() => HandleRightButtonClick(button, terrainTypesValue));

        Image component = button.transform.GetChild(0).GetComponent<Image>();
        component.sprite = Sprite.Create(texture2D,
            new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        return button;
    }

    private Texture2D GetTextureForTerrainType(TileMapTextureGenerator tileMapTextureGenerator,
        TerrainType terrainTypesValue, List<Color[]> texturesFromAtlas)
    {
        int terrainIndex = tileMapTextureGenerator.MapTerrainToTexture(terrainTypesValue);
        Color[] texturePixels = texturesFromAtlas[terrainIndex];

        Texture2D texture2D = new Texture2D(tileMap.tileResolution, tileMap.tileResolution);
        texture2D.SetPixels(texturePixels);
        texture2D.Apply();
        return texture2D;
    }

    private void HandleLeftButtonClick(GameObject clickedButton, TerrainType terrainTypesValue)
    {
        if (clickedButton != _currentlySelectedButton && _isInEditMode)
        {
            tileTypeDataEditor.CancelEditing();
            Button button = clickedButton.GetComponent<Button>();
            button.Select();
            _currentlySelectedButton = clickedButton;
            CurrentlySelectedTerrainType = terrainTypesValue;
            AddBackgroundHighlightToButton(clickedButton);
            SetPanelColor(new Color(0.1f, 0.1f, 0.1f, 0.5f));
        }
        else if (clickedButton != _currentlySelectedButton && !_isInEditMode)
        {
            Button button = clickedButton.GetComponent<Button>();
            button.Select();
            _currentlySelectedButton = clickedButton;
            CurrentlySelectedTerrainType = terrainTypesValue;
            AddBackgroundHighlightToButton(clickedButton);
            SetPanelColor(new Color(0.1f, 0.1f, 0.1f, 0.5f));
        }
        else
        {
            _currentlySelectedButton = null;
            tileTypeDataEditor.CancelEditing();
            ClearTypesPanelHighlight();
        }
    }

    private void ClearTypesPanelHighlight()
    {
        RemoveHighlight();
        SetPanelColor(new Color(0f, 0f, 0f, 0.4f));
    }

    private void HandleRightButtonClick(GameObject clickedButton, TerrainType terrainTypesValue)
    {
        if (!_isInEditMode || _currentlySelectedButton != clickedButton)
        {
            _isInEditMode = true;
            ClearTypesPanelHighlight();
            _currentlySelectedButton = clickedButton;
            AddBackgroundHighlightToButton(clickedButton);
            CurrentlySelectedTerrainType = terrainTypesValue;
            tileTypeDataEditor.StartEditing(_materialProperties[terrainTypesValue]);
            
        }
        else
        {
            RemoveHighlight();
            CleanupEditingState();
            tileTypeDataEditor.CancelEditing();
        }
    }

    private void CleanupEditingState()
    {
        _isInEditMode = false;
        _currentlySelectedButton = null;
        CurrentlySelectedTerrainType = TerrainType.Bush;
    }

    private void AddBackgroundHighlightToButton(GameObject parentButton)
    {
        RectTransform rectTransform = parentButton.transform.GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;

        GameObject imageGameObject = new GameObject();
        imageGameObject.transform.SetParent(parentButton.transform);

        Image image = imageGameObject.AddComponent<Image>();
        image.color = new Color(1f, 1f, 1f, 0.8f);

        Vector3 pos = new Vector3(rect.size.x / 2, rect.size.y - 130, -1);
        imageGameObject.transform.localPosition = pos;
        imageGameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 10);

        RemoveHighlight();
        _buttonHighlight = imageGameObject;
    }

    private void SetPanelColor(Color color)
    {
        Image component = terrainTypesPanel.GetComponent<Image>();
        component.color = color;
    }

    private void RemoveHighlight()
    {
        if (_buttonHighlight != null)
        {
            Destroy(_buttonHighlight);
            _buttonHighlight = null;
        }
    }
}
