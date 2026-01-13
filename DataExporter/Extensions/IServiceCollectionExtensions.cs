using DataExporter.Services;
using DataExporter;
using AutoMapper;
using DataExporter.Profiles;

namespace DataExporter.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDataExporterServices(this IServiceCollection services)
    {
        services.AddDbContext<ExporterDbContext>();
        services.AddScoped<IPolicyService, PolicyService>();
        services.AddScoped<IValidationService, ValidationService>();
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<PolicyProfile>();
        });

        return services;
    }
}