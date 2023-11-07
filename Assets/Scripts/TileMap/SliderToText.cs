using UnityEngine;
using UnityEngine.UI;

public class SliderToText : MonoBehaviour
{
    public string unitSuffix;
    private Text textComponent;
 
    void Awake() {
        textComponent = gameObject.GetComponentInChildren<Text>();
    }
 
    public void SetSliderValue(float sliderValue)
    {
        string text = sliderValue.ToString("0.00") + " " + unitSuffix;
        textComponent.text = text;
    }
}