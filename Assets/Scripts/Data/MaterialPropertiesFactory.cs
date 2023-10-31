using System;
namespace Data
{
    public class MaterialPropertiesFactory
    {
        public static MaterialProperties GetProperties(TerrainType type) {
            switch (type) {

                case TerrainType.Grass:
                    return new MaterialProperties(18608f, 0.0555f, 0.01f, 512.576f, 114.8293f, 0.1659f, 0.3048f, 0.12f); // trawa

                case TerrainType.HighGrass:
                    return new MaterialProperties(18608f, 0.0555f, 0.01f, 512.576f, 49.2125f, 0.67251f, 0.762f,0.25f); // wysoka trawa

                case TerrainType.Tree:
                    return new MaterialProperties(18608f, 0.0555f, 0.01f, 512.576f, 65.6167f, 1.8157f, 0.1219f, 0.25f); // drzewo

                case TerrainType.Bush:
                    return new MaterialProperties(18608f, 0.0555f, 0.01f, 512.576f, 52.4934f, 0.67251f, 0.3048f, 0.40f); // krzaki

                case TerrainType.Burning:
                    return new MaterialProperties(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f); // wypalone


                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);

            }
        }
        public static TerrainType[] GetAllowedInitialTerrainTypes()
        {
            return new[] {TerrainType.Grass, TerrainType.Tree, TerrainType.Bush, TerrainType.HighGrass};
        }
    }
}
