using AutoMapper;
using DataExporter.Model;
using DataExporter.Dtos;

namespace DataExporter.Profiles;

public class PolicyProfile : Profile 
{
    public PolicyProfile()
    {
        CreateMap<Policy, ReadPolicyDto>();
        CreateMap<CreatePolicyDto, Policy>();
        CreateMap<Policy, ExportDto>();
    }
}