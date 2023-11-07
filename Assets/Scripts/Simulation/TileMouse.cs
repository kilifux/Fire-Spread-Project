using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMouse : MonoBehaviour
{
    [SerializeField] private TileMap tileMap;
    
    private Collider _collider;
    private MeshFilter _meshFilter;
    
    private Camera _mainCamera;
    private Renderer _renderer;
    private bool _isTileMapNull;
    
    void Start()
    {
        if (tileMap)
        {
            _collider =  tileMap.GetComponent<Collider>();
        }
        
        _mainCamera = Camera.main;
        _meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = false;
    }


    void Update()
    {
        if (!tileMap)
        {
            return;
        }

        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        
        bool isTileHitWithRay = _collider.Raycast(ray, out hitInfo, Mathf.Infinity);
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && isTileHitWithRay)
        {
            Vector3[] tileRectVertices = GetTileVerticesPositions(hitInfo);
            if (tileRectVertices.Length < 4)
            {
                return;
            }

            Vector3 startPosition = PositionIndicatorAboveTile(tileRectVertices);

            transform.position = startPosition;
            _meshFilter.mesh.vertices = tileRectVertices;
            
            _renderer.enabled = true;
        }
        else
        {
            _renderer.enabled = false;
        }
    }
    
    
    private Vector3 PositionIndicatorAboveTile(Vector3[] rectVertices)
    {
        Vector3 startPosition = tileMap.gameObject.transform.TransformPoint(rectVertices[0]);
        for (int i = 0; i < rectVertices.Length; i++)
        {
            rectVertices[i] =
                tileMap.gameObject.transform.TransformPoint(rectVertices[i].x, rectVertices[i].y + 0.01f,
                    rectVertices[i].z);
            rectVertices[i] = new Vector3(rectVertices[i].x - startPosition.x,
                rectVertices[i].y - startPosition.y, rectVertices[i].z - startPosition.z);
        }

        return startPosition;
    }

    private Vector3[] GetTileVerticesPositions(RaycastHit hitInfo)
    {
        Vector3 hitPointPositionVector = tileMap.gameObject.transform.InverseTransformPoint(hitInfo.point);
        Vector3[] rectVertices = tileMap.GetRectVerticesByPoint(hitPointPositionVector);
        return rectVertices;
    }
}
