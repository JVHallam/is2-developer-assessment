using DataExporter.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataExporter.Services;

public interface IPolicyService
{
    Task<ReadPolicyDto?> CreatePolicyAsync(CreatePolicyDto createPolicyDto);
    Task<IList<ReadPolicyDto>> ReadPoliciesAsync();
    Task<ReadPolicyDto?> ReadPolicyAsync(int id);
    Task<List<ExportDto>> GetExportDataAsync(DateTime fromDate, DateTime toDate);
}