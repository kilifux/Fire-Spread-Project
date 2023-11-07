using System;
using System.Collections;
using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileTypesPanel : MonoBehaviour
{
    public GameObject TerrainTypesPanel;
    public TileTypeDataEditor TileTypeDataEditor;
    public GameObject buttonPrefab;

    public TileMap TileMap;

    private List<GameObject> _terrainButtons = new List<GameObject>();

    private GameObject _currentlySelectedButton;
    public TerrainType CurrentlySelectedTerrainType { get; private set; }
    private Dictionary<TerrainType, MaterialProperties> _materialProperties = new Dictionary<TerrainType,MaterialProperties>();

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
        MaterialProperties materialProperties = TileTypeDataEditor.FinishEditing();
        _materialProperties[CurrentlySelectedTerrainType] = materialProperties;

        CleanupEditingState();
    }

    public void OnCancelTypeEditing()
    {
        TileTypeDataEditor.CancelEditing();
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
        
        TileMapTextureGenerator tileMapTextureGenerator = TileMap.GetTileMapTextureGenerator();
        List<Color[]> texturesFromAtlas = tileMapTextureGenerator.ExtractTexturesFromAtlas();

        RectTransform parentRectTransform = TerrainTypesPanel.transform.GetComponent<RectTransform>();
        foreach (TerrainType terrainTypesValue in terrainTypesValues)
        {
            Texture2D texture2D = GetTextureForTerrainType(tileMapTextureGenerator, terrainTypesValue, texturesFromAtlas);
            GameObject button = CreateButton(terrainTypesValue, texture2D);
            _terrainButtons.Add(button);

            RectTransform buttonRectTransform = button.transform.GetComponent<RectTransform>();
            float rectWidth = buttonRectTransform.rect.width / terrainTypesValues.Length;
            int buttonOffset = Array.IndexOf(terrainTypesValues, terrainTypesValue);
            button.transform.localPosition = new Vector3(rectWidth * 2 + (rectWidth * 8 * buttonOffset),
                (-parentRectTransform.rect.height / 2), 0);
        }

        SetPanelWidth(terrainTypesValues, parentRectTransform);
    }

    private void InitTerrainTypeMaterialProperties(TerrainType[] terrainTypesValues)
    {
        foreach (TerrainType terrainType in terrainTypesValues)
        {
            MaterialProperties materialProperties = MaterialPropertiesFactory.GetProperties(terrainType);
            _materialProperties.Add(terrainType, materialProperties);
        }
    }

    private void SetPanelWidth(TerrainType[] terrainTypesValues, RectTransform parentRectTransform)
    {
        GameObject terrainButton = _terrainButtons[0];
        RectTransform buttonRect = terrainButton.transform.GetComponent<RectTransform>();
        Rect rect = buttonRect.rect;
        float maxWidth = (rect.width * 2 * terrainTypesValues.Length);
        Rect parentRect = parentRectTransform.rect;
        parentRectTransform.sizeDelta = new Vector2(maxWidth, parentRect.height);
    }

    private GameObject CreateButton(TerrainType terrainTypesValue, Texture2D texture2D)
    {
        GameObject button = Instantiate(buttonPrefab, TerrainTypesPanel.transform, false);
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

        Texture2D texture2D = new Texture2D(TileMap.tileResolution, TileMap.tileResolution);
        texture2D.SetPixels(texturePixels);
        texture2D.Apply();
        return texture2D;
    }

    private void HandleLeftButtonClick(GameObject clickedButton, TerrainType terrainTypesValue)
    {
        if (clickedButton != _currentlySelectedButton && !_isInEditMode)
        {
            Button button = clickedButton.GetComponent<Button>();
            button.Select();
            _currentlySelectedButton = clickedButton;
            CurrentlySelectedTerrainType = terrainTypesValue;
            AddBackgroundHighlightToButton(clickedButton);
            SetPanelColor(new Color(1f, 1f, 1f, 0.8f));
            Debug.Log("Button clicked");
        }
        else
        {
            _currentlySelectedButton = null;
            ClearTypesPanelHighlight();
            Debug.Log("Button unselected");
        }
    }

    private void ClearTypesPanelHighlight()
    {
        RemoveHighlight();
        SetPanelColor(new Color(1f, 1f, 1f, 0.4f));
    }

    private void HandleRightButtonClick(GameObject clickedButton, TerrainType terrainTypesValue)
    {
        if (!_isInEditMode || _currentlySelectedButton != clickedButton)
        {
            _isInEditMode = true;
            ClearTypesPanelHighlight();
            _currentlySelectedButton = clickedButton;

            CurrentlySelectedTerrainType = terrainTypesValue;
            TileTypeDataEditor.StartEditing(_materialProperties[terrainTypesValue]);
        }
        else
        {
            CleanupEditingState();
            TileTypeDataEditor.CancelEditing();
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
        image.color = new Color(0.2f, 0.0f, 1.0f, 0.3f);

        Vector3 pos = new Vector3(rect.size.x / 2, 0, -1);
        imageGameObject.transform.localPosition = pos;
        imageGameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.size.x / 1.5f, rect.size.y / 1.5f);

        RemoveHighlight();
        _buttonHighlight = imageGameObject;
    }

    private void SetPanelColor(Color color)
    {
        Image component = TerrainTypesPanel.GetComponent<Image>();
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
