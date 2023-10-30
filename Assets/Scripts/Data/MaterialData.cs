namespace Data
{

    public class MaterialData
    {
        public float LowHeatContent; // zawartoœæ energetyczna
        public float MineralContent;
        public float EffectiveMineralContent; // zawartoœæ minera³ów
        public float BurnedParticleDensity; // gestosc czasteczek po spaleniu
        public float SurfaceAreaToVolumeRatio;
        public float BurnedFuelLoad;
        public float FuelBedDepth;
        public float BurnedFuelMoisture;

        public MaterialData(float lowHeatContent, float mineralContent, float effectiveMineralContent,
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

        public MaterialData Clone()
        {
            return new MaterialData(LowHeatContent, MineralContent, EffectiveMineralContent, BurnedParticleDensity, SurfaceAreaToVolumeRatio, BurnedFuelLoad, FuelBedDepth, BurnedFuelMoisture);
        }
    }
}
