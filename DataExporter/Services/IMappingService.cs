using DataExporter.Dtos;
using DataExporter.Model;

namespace DataExporter.Services;

public interface IMappingService
{
    ReadPolicyDto MapToDto(Policy policy);
}