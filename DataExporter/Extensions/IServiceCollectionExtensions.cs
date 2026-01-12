using DataExporter.Services;
using DataExporter;

namespace DataExporter.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDataExporterServices(this IServiceCollection services)
    {
        services.AddDbContext<ExporterDbContext>();
        services.AddScoped<IPolicyService, PolicyService>();
        services.AddScoped<IMappingService, MappingService>();

        return services;
    }
}