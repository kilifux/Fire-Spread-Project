using Data;
using System;
using UnityEngine;

namespace Simulation
{
    public class SimulationVariablesCalculator : MonoBehaviour
    {
        private MaterialProperties materialProperties;

        private float reactionIntensity;
        private float optimumReactionVelocity;
        private float maximumReactionVelocity;
        private float optimumPackingRatio;
        private float packingRatio;
        private float ovenDryBulkDensity;
        private float netFuelLoad;

        private float moistureDampingCoefficient;
        private float mineralDampingCoefficient;

        private float propagatingFluxRatio;
        private float windFactor;
        private float slopeFactor;
        private float effectiveHeatingNumber;
        private float heatOfPreignition;

        public float RateOfSpread { get; set; }

        public SimulationVariablesCalculator()
        {
        }

        public void CalculateVariables(Data.TerrainData terrainData, float slopeSteepness, float moistureContent, 
            float windSpeed, float cosAngle)
        {
            this.materialProperties = terrainData.MaterialProperties;

            netFuelLoad = CalculateNetFuelLoad();
            ovenDryBulkDensity = CalculateOvenDryBulkDensity();
            packingRatio = CalculatePackingRatio();
            optimumPackingRatio = CalculateOptimumPackingRatio();
            maximumReactionVelocity = CalculateMaximumReactionVelocity();
            optimumReactionVelocity = CalculateOptimumReactionVelocity();

            moistureDampingCoefficient = CalculateMoistureDampingCoefficient(moistureContent);
            mineralDampingCoefficient = CalculateMineralDampingCoefficient();

            reactionIntensity = CalculateReactionIntensity();
            propagatingFluxRatio = CalculatePropagatingFluxRatio();
            windFactor = CalculateWindFactor(windSpeed, cosAngle);
            slopeFactor = CalculateSlopeFactor(slopeSteepness);
            effectiveHeatingNumber = CalculateEffectiveHeatingNumber();
            heatOfPreignition = CalculateHeatOfPreignition(moistureContent);

            RateOfSpread = CalculateRateOfSpread();
        }


        private float CalculateRateOfSpread()
        {
            return reactionIntensity * propagatingFluxRatio * (1.0f + windFactor + slopeFactor)
                /(ovenDryBulkDensity * effectiveHeatingNumber * heatOfPreignition);
        }

        private float CalculateReactionIntensity()
        {
            return optimumReactionVelocity * netFuelLoad * materialProperties.LowHeatContent 
                * moistureDampingCoefficient * mineralDampingCoefficient;
        }

        private float CalculateNetFuelLoad()
        {
            return materialProperties.OvenDryFuelLoad * (1.0f - materialProperties.TotalMineralContent);
        }

        private float CalculateOvenDryBulkDensity()
        {
            return materialProperties.OvenDryFuelLoad / materialProperties.FuelBedDepth;
        }

        private float CalculatePackingRatio()
        {
            return ovenDryBulkDensity / materialProperties.OvenDryParticleDensity;
        }

        private float CalculateOptimumPackingRatio()
        {
            return 0.20395f * Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, -0.8189f);
        }

        private float CalculateMaximumReactionVelocity()
        {
            float ratioPower = Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, -1.5f);
            return Mathf.Pow((0.0591f + 2.926f * ratioPower), -1.0f);
        }

        private float CalculateOptimumReactionVelocity()
        {
            float a = 8.9033f * Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, -0.7913f);
            return maximumReactionVelocity * Mathf.Pow(packingRatio / optimumPackingRatio, a)
                * Mathf.Exp(a * (1 - packingRatio / optimumPackingRatio));
        }

        private float CalculateMoistureDampingCoefficient(float moistureContent)
        {
            float rm = moistureContent / materialProperties.DeadFuelMoistureOfExtinction;
            rm = rm > 1.0f ? 1.0f : rm;

            return 1.0f - 2.59f * rm + 5.11f * Mathf.Pow(rm, 2.0f) - 3.52f * Mathf.Pow(rm, 3.0f);
        }

        private float CalculateMineralDampingCoefficient()
        {
            float ns = 0.174f * Mathf.Pow(materialProperties.EffectiveMineralContent, -0.19f);

            return ns > 1.0f ? 1.0f : ns;
        }

        private float CalculatePropagatingFluxRatio()
        {
            return Mathf.Pow(192.0f + 7.9095f * materialProperties.SurfaceAreaToVolumeRatio, -1.0f)
                * Mathf.Exp((0.792f + 3.7597f * Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, 0.5f))
                * (packingRatio + 0.1f));
        }

        private float CalculateWindFactor(float windVelocity, float cosAngle)
        {
            float c = 7.47f * Mathf.Exp(-0.8711f * Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, 0.55f));
            float b = 0.15988f * Mathf.Pow(materialProperties.SurfaceAreaToVolumeRatio, 0.54f);
            float e = 0.715f * Mathf.Exp(-0.01094f * materialProperties.SurfaceAreaToVolumeRatio);

            return cosAngle * c * Mathf.Pow(3.281f * windVelocity, b) 
                * Mathf.Pow(packingRatio / optimumPackingRatio, -1.0f * e);
        }

        private float CalculateSlopeFactor(float slopeSteepness)
        {
            return 5.275f * Mathf.Pow(packingRatio, -0.3f) * Mathf.Pow(slopeSteepness, 2.0f);
        }

        private float CalculateEffectiveHeatingNumber()
        {
            return Mathf.Exp(-4.528f / materialProperties.SurfaceAreaToVolumeRatio);
        }

        private float CalculateHeatOfPreignition(float moistureContent)
        {
            return 581.0f + 2594.0f * moistureContent;
        }
    }
}
