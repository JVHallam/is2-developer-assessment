using DataExporter.Services;
using DataExporter;

namespace DataExporter.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDataExporterServices(this IServiceCollection services)
    {
        services.AddDbContext<ExporterDbContext>();
        services.AddScoped<PolicyService>();
        services.AddSingleton<IMappingService, MappingService>();

        return services;
    }
}