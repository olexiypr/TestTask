using LegiosoftTestTask.DataContext;
using Microsoft.EntityFrameworkCore;

namespace LegiosoftTestTask.Extension;

public static class ApplicationDbContextExtension
{
    public static IServiceCollection AddDbContext (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.AddDbContext<ApplicationEfContext>(options => options
            .EnableSensitiveDataLogging()
            .UseNpgsql(connectionString));
        return services;
    }
}