using DataExporter.Dtos;
using DataExporter.Model;

namespace DataExporter.Services;

public class MappingService : IMappingService
{
    public ReadPolicyDto MapToDto(Policy policy)
    {
        return new ReadPolicyDto()
        {
            Id = policy.Id,
            PolicyNumber = policy.PolicyNumber,
            Premium = policy.Premium,
            StartDate = policy.StartDate
        };
    }

    public Policy MapToEntity(CreatePolicyDto policy)
    {
        return new Policy
        {
            PolicyNumber = policy.PolicyNumber,
            Premium = policy.Premium,
            StartDate = policy.StartDate
        };
    }
}