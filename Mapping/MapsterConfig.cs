using Mapster;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(ApiEcommerce.Mapping.CategoryProfile).Assembly);
    }
}
