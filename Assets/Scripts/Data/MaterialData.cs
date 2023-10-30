namespace Data
{

    public class MaterialProperties
    {
        public float LowHeatContent; // zawarto�� energetyczna
        public float MineralContent;
        public float EffectiveMineralContent; // zawarto�� minera��w
        public float BurnedParticleDensity; // gestosc czasteczek po spaleniu
        public float SurfaceAreaToVolumeRatio;
        public float BurnedFuelLoad;
        public float FuelBedDepth;
        public float BurnedFuelMoisture;

        public MaterialProperties(float lowHeatContent, float mineralContent, float effectiveMineralContent,
               float burnedParticleDensity, float surfaceAreaToVolumeRatio, float burnedFuelLoad, float fuelBedDepth,
               float burnedFuelMoisture)
        {
            LowHeatContent = lowHeatContent;
            MineralContent = mineralContent;
            EffectiveMineralContent = effectiveMineralContent;
            BurnedParticleDensity = burnedParticleDensity;
            SurfaceAreaToVolumeRatio = surfaceAreaToVolumeRatio;
            BurnedFuelLoad = burnedFuelLoad;
            FuelBedDepth = fuelBedDepth;
            BurnedFuelMoisture = burnedFuelMoisture;
        }

        public MaterialProperties Clone()
        {
            return new MaterialProperties(LowHeatContent, MineralContent, EffectiveMineralContent, BurnedParticleDensity, SurfaceAreaToVolumeRatio, BurnedFuelLoad, FuelBedDepth, BurnedFuelMoisture);
        }
    }
}
