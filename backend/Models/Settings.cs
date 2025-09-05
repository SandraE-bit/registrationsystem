namespace backend.Models;

public sealed class Settings
{
    public class Configuration
    {
        public AzureCosmos CosmosDB { get; set; } = new();
    }

    public class AzureCosmos
    {
        public string DatabaseName { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
    }
}
