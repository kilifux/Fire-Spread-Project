using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class TileTypeDataEditor : MonoBehaviour
{
    [SerializeField] private GameObject tileTypeDataEditorPanel;
    
    private void Start()
    {
        HidePanel();
    }

    public void StartEditing(MaterialProperties properties)
    {
        SetMaterialProperties(properties);
        ShowPanel();
    }

    public void CancelEditing()
    {
        HidePanel();
    }

    public MaterialProperties FinishEditing()
    {
        HidePanel();
        return ReadMaterialPropertiesValues();
    }

    private void ShowPanel()
    {
        tileTypeDataEditorPanel.SetActive(true);
    }

    private void HidePanel()
    {
        tileTypeDataEditorPanel.SetActive(false);
    }
    
    private void SetMaterialProperties(MaterialProperties properties)
    {
        List<Slider> componentsInChildren = GetComponentsInChildren<Slider>().ToList();
        SetValue(componentsInChildren, "LowHeatContentSlider", properties.LowHeatContent);
        SetValue(componentsInChildren, "TotalMineralContentSlider", properties.TotalMineralContent);
        SetValue(componentsInChildren, "EffectiveMineralContentSlider", properties.EffectiveMineralContent);
        SetValue(componentsInChildren, "OvenDryParticleDensitySlider", properties.OvenDryParticleDensity);
        SetValue(componentsInChildren, "SurfaceAreaToVolumeRatioSlider", properties.SurfaceAreaToVolumeRatio);
        SetValue(componentsInChildren, "OvenDryFuelLoadSlider", properties.OvenDryFuelLoad);
        SetValue(componentsInChildren, "FuelBedDepthSlider", properties.FuelBedDepth);
        SetValue(componentsInChildren, "DeadFuelMoistureOfExtinctionSlider", properties.DeadFuelMoistureOfExtinction);
    }

    private MaterialProperties ReadMaterialPropertiesValues()
    {
        List<Slider> componentsInChildren = GetComponentsInChildren<Slider>().ToList();

        float lowHeatContent = GetValue(componentsInChildren, "LowHeatContentSlider");
        float totalMineralContent = GetValue(componentsInChildren, "TotalMineralContentSlider");
        float effectiveMineralContent = GetValue(componentsInChildren, "EffectiveMineralContentSlider");
        float ovenDryParticleDensity = GetValue(componentsInChildren, "OvenDryParticleDensitySlider");
        float surfaceAreaToVolumeRatio = GetValue(componentsInChildren, "SurfaceAreaToVolumeRatioSlider");
        float ovenDryFuelLoad = GetValue(componentsInChildren, "OvenDryFuelLoadSlider");
        float fuelBedDepth = GetValue(componentsInChildren, "FuelBedDepthSlider");
        float deadFuelMoistureOfExtinction = GetValue(componentsInChildren, "DeadFuelMoistureOfExtinctionSlider");

        return new MaterialProperties(lowHeatContent,
            totalMineralContent,
            effectiveMineralContent,
            ovenDryParticleDensity,
            surfaceAreaToVolumeRatio,
            ovenDryFuelLoad,
            fuelBedDepth,
            deadFuelMoistureOfExtinction);
    }

    private void SetValue(List<Slider> components, string componentName, float value)
    {
        Slider foundComponent = components.Find(gameComponent => gameComponent.name == componentName);
        foundComponent.value = value;
    }    
    
    private float GetValue(List<Slider> components, string componentName)
    {
        Slider foundComponent = components.Find(gameComponent => gameComponent.name == componentName);
        return foundComponent.value;
    }

}
