using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileGUIInfo : MonoBehaviour
{
    [SerializeField] private GameObject tileInfoPanel;
    [SerializeField] private TextMeshProUGUI tileTypeName;

    public TileData CurrentlySelectedTile { get; private set; }

    private void Start()
    {
        tileInfoPanel.SetActive(false);
    }

    public void HandleMouseButtonClick(TileData tileUnderMouse)
    {
        if (Input.GetMouseButtonDown(0) && tileUnderMouse != null)
        {
            tileTypeName.text = tileUnderMouse.TerrainData.Type.ToString();
            CurrentlySelectedTile = tileUnderMouse;
            tileInfoPanel.SetActive(true);
        }
    }
}
