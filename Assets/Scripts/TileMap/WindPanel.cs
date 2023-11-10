using Simulation;
using UnityEngine;
using UnityEngine.UI;

public class WindPanel : MonoBehaviour
{
    public SimulationInstance simulationInstance;
    public Slider windSpeedSlider;
    public Slider windDirectionSlider;

    public void UpdateWindConfig(float sliderValue)
    {
        simulationInstance.UpdateWindConfig(windSpeedSlider.value, windDirectionSlider.value);
    }
}