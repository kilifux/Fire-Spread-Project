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
    [SerializeField] private int sizeZ = 24;
    [SerializeField] private float tileSize = 8f;

    [SerializeField] private TileGUIInfo tileGUIInfo;
    [SerializeField] private TileMapData _tileMapData;
    
    
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

    void Start()
    {
        BuildMap();
    }
    
    void BuildMap()
    {
        TileMapGenerator tileMapGenerator = new TileMapGenerator(sizeX, sizeZ, tileSize);

        Mesh mesh = tileMapGenerator.GenerateMap();
        _meshFilter.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }

    private void OnMouseDown()
    {
        TileData tileUnderMouse = GetTileUnderMouse();
        Debug.Log(tileUnderMouse.TerrainData.Type.ToString());
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
            Debug.Log(hitInfo.point);
            return _tileMapData.GetTileData(tileByPoint.x, tileByPoint.y);
        }

        return null;
    }

    public Vector2Int GetTileByPoint(Vector3 positionVector)
    {
        return new Vector2Int(Mathf.FloorToInt(positionVector.x / tileSize),
            Mathf.FloorToInt(positionVector.z / tileSize));
    }
    
}
