namespace Data
{
    public class TerrainData
    {
    
        public TerrainType Type { get; set; }
        public MaterialProperties MaterialProperties { get; set; }
        public float Height { get; }

        public TerrainData(TerrainType type, float height, MaterialProperties materialProperties)
        {
            Type = type;
            Height = height;
            MaterialProperties = materialProperties;
        }
    }
}
