namespace Data
{
    public class MaterialProperties
    {
        public readonly float LowHeatContent;
        public readonly float TotalMineralContent;
        public readonly float EffectiveMineralContent;
        public readonly float OvenDryParticleDensity;
        
        public readonly float SurfaceAreaToVolumeRatio;
        public readonly float OvenDryFuelLoad;
        public readonly float FuelBedDepth;
        public readonly float DeadFuelMoistureOfExtinction;

        public MaterialProperties(float lowHeatContent, float totalMineralContent, float effectiveMineralContent,
            float ovenDryParticleDensity, float surfaceAreaToVolumeRatio, float ovenDryFuelLoad, float fuelBedDepth,
            float deadFuelMoistureOfExtinction)
        {
            LowHeatContent = lowHeatContent;
            TotalMineralContent = totalMineralContent;
            EffectiveMineralContent = effectiveMineralContent;
            OvenDryParticleDensity = ovenDryParticleDensity;
            
            SurfaceAreaToVolumeRatio = surfaceAreaToVolumeRatio;
            OvenDryFuelLoad = ovenDryFuelLoad;
            FuelBedDepth = fuelBedDepth;
            DeadFuelMoistureOfExtinction = deadFuelMoistureOfExtinction;
        }

        public MaterialProperties Clone()
        {
            return new MaterialProperties(LowHeatContent,
                TotalMineralContent,
                EffectiveMineralContent,
                OvenDryParticleDensity,
                SurfaceAreaToVolumeRatio,
                OvenDryFuelLoad,
                FuelBedDepth,
                DeadFuelMoistureOfExtinction);
        }
    }
}