using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderToText : MonoBehaviour
{
    public string unitSuffix;
    private TextMeshProUGUI textComponent;
 
    void Awake() {
        textComponent = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
 
    public void SetSliderValue(float sliderValue)
    {
        string text = sliderValue.ToString("0.00") + " " + unitSuffix;
        textComponent.text = text;
    }
}