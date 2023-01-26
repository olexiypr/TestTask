namespace LegiosoftTestTask.Extension;

public static class ConfigurationBuilderExtension
{
    public static ConfigurationManager AddConfiguration(this ConfigurationManager manager)
    {
        manager.AddJsonFile("appsettings.json", optional: false);
        return manager;
    }
}