using System;
using Data;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TileMap : MonoBehaviour
{
    [SerializeField] private int sizeX = 24;
    [SerializeField] private int sizeY = 24;
    [SerializeField] public float tileSize = 8f;
    public int tileResolution = 16;

    [SerializeField] private TileGUIInfo tileGUIInfo;
    [SerializeField] public TileMapData _tileMapData;
    [SerializeField] private TileTypesPanel _tileTypesPanel;
    
    public Texture2D textureAtlas;
    
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private MeshCollider _meshCollider;
    private Camera _camera;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshCollider = GetComponent<MeshCollider>();
        _camera = Camera.main;
    }

    public void InitTileMap(int _sizeX, int _sizeY, TileMapData tileMapData)
    {
        sizeX = _sizeX;
        sizeY = _sizeY;
        _tileMapData = tileMapData;
        
        BuildMap();
        BuildTexture();
    }

    private void Start()
    {
        BuildMap();
        BuildTexture();
    }


    void BuildMap()
    {
        TileMapGenerator tileMapGenerator = new TileMapGenerator(sizeX, sizeY, tileSize);

        Mesh mesh = tileMapGenerator.GenerateMap();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }

    private void OnMouseDown()
    {
        TileData tileUnderMouse = GetTileUnderMouse();
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && _tileTypesPanel.IsButtonSelected())
        {
            TerrainType currentlySelectedTerrainType = _tileTypesPanel.CurrentlySelectedTerrainType;
            tileUnderMouse.TerrainData.Type = currentlySelectedTerrainType;
            tileUnderMouse.TerrainData.MaterialProperties = _tileTypesPanel.GetPropertiesForCurrentSelection();

            ResetBurningDistance(tileUnderMouse);
            UpdateTexture(tileUnderMouse);
            tileUnderMouse.IsBurning = false;
        }
        else
        {
            tileGUIInfo.HandleMouseButtonClick(tileUnderMouse);
        }
    }
    
    private void ResetBurningDistance(TileData tileData)
    {
        string tileKey = tileData.PositionX.ToString() + tileData.PositionY.ToString();

        for (int x = tileData.PositionX - 1; x <= tileData.PositionX + 1; x++)
        {
            if (x < 0 || x >= _tileMapData.GetTileData().Length)
                continue;

            for (int y = tileData.PositionY - 1; y <= tileData.PositionY + 1; y++)
            {
                if (y < 0 || y >= _tileMapData.GetTileData()[x].Length)
                    continue;
                if (x == tileData.PositionX && y == tileData.PositionY)
                    continue;

                if (_tileMapData.GetTileData(x, y).FireSpreadingDistance.ContainsKey(tileKey))
                {
                    _tileMapData.GetTileData(x, y).FireSpreadingDistance[tileKey] = 0.0f;
                }
            }
            
        }
    }
    
    private TileData GetTileUnderMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();

        bool isTileHitWithRay = _meshCollider.Raycast(ray, out hitInfo, Mathf.Infinity);
        
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && isTileHitWithRay)
        {
            Vector3 hitPointPositionVector = gameObject.transform.InverseTransformPoint(hitInfo.point);
            Vector2Int tileByPoint = GetTileByPoint(hitPointPositionVector);
            return _tileMapData.GetTileData(tileByPoint.x, tileByPoint.y);
        }

        return null;
    }

    public void UpdateTexture(TileData tileUnderMouse)
    {
        TileMapTextureGenerator tileMapTextureGenerator = GetTileMapTextureGenerator();
        tileMapTextureGenerator.UpdateTexture(_meshRenderer.sharedMaterial.mainTexture as Texture2D, tileUnderMouse);
    }
    
    public Vector3[] GetRectVerticesByPoint(Vector3 positionVector)
    {
        Vector2Int tileByPoint = GetTileByPoint(positionVector);
        TileMapGenerator tileMapGenerator = new TileMapGenerator(sizeX, sizeY, tileSize);
        Mesh sharedMesh = _meshFilter.sharedMesh;
        return tileMapGenerator.GetTrianglesOfATile(tileByPoint, sharedMesh.vertices);
    }
    
    public Vector2Int GetTileByPoint(Vector3 positionVector)
    {
        return new Vector2Int(Mathf.FloorToInt(positionVector.x / tileSize),
            Mathf.FloorToInt(positionVector.z / tileSize));
    }
    
    private void BuildTexture()
    {
        TileMapTextureGenerator tileMapTextureGenerator = GetTileMapTextureGenerator();
        Texture2D texture = tileMapTextureGenerator.GenerateTexture(_tileMapData);
        _meshRenderer.sharedMaterial.mainTexture = texture;
        
        Debug.Log("Texture built!");
    }
    
    public TileMapTextureGenerator GetTileMapTextureGenerator()
    {
        TileMapTextureGenerator tileMapTextureGenerator =
            new TileMapTextureGenerator(textureAtlas, tileResolution, sizeX, sizeY);
        return tileMapTextureGenerator;
    }
    
}
