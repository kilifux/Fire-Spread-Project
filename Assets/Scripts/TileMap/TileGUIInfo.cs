using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileGUIInfo : MonoBehaviour
{
    public GameObject tileInfoPanel;
    public TextMeshProUGUI tileTypeName;

    public TileData CurrentlySelectedTile { get; private set; }

    private void Start()
    {
        tileInfoPanel.SetActive(false);
    }

    public void HandleMouseButtonClick(TileData tileUnderMouse)
    {
        if (Input.GetMouseButtonDown(0) && tileUnderMouse != null)
        {
            FillPanelInfo(tileUnderMouse);
            CurrentlySelectedTile = tileUnderMouse;
            tileInfoPanel.SetActive(true);
        }
    }
    
    public void ClosePanel()
    {
        tileInfoPanel.SetActive(false);
    }
    
    private void FillPanelInfo(TileData tileUnderMouse)
    {
        tileTypeName.text = tileUnderMouse.TerrainData.Type.ToString();
    }
}
