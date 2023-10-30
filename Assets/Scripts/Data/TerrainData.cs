namespace Data
{
    public class TerrainData
    {
    
        public TerrainType Type { get; set; }
        public MaterialData MaterialData { get; set; }
        public float Height { get; }

        public TerrainData(TerrainType type, float height, MaterialData materialData)
        {
            Type = type;
            Height = height;
            MaterialData = materialData;
        }
    }
}
