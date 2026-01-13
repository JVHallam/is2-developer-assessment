using DataExporter.Dtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataExporter.Model;

namespace DataExporter.Services;

public class PolicyService : IPolicyService
{
    private readonly ExporterDbContext _dbContext;
    private readonly ILogger<PolicyService> _logger;
    private readonly IMapper _mapper;

    public PolicyService(
        ExporterDbContext dbContext,
        ILogger<PolicyService> logger,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new policy from the DTO.
    /// </summary>
    /// <param name="policy"></param>
    /// <returns>Returns a ReadPolicyDto representing the new policy, if succeded. Returns null, otherwise.</returns>
    public async Task<ReadPolicyDto?> CreatePolicyAsync(CreatePolicyDto createPolicyDto)
    {
        var policy = _mapper.Map<Policy>(createPolicyDto);

        _dbContext.Policies.Add(policy);

        await _dbContext.SaveChangesAsync();

        var asReadDto = _mapper.Map<ReadPolicyDto>(policy);

        return asReadDto;
    }

    /// <summary>
    /// Retrives all policies.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Returns a list of ReadPoliciesDto.</returns>
    public async Task<IList<ReadPolicyDto>> ReadPoliciesAsync()
    {
        return _dbContext
            .Policies
            .Select(_mapper.Map<ReadPolicyDto>)
            .ToList();
    }

    /// <summary>
    /// Retrieves a policy by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Returns a ReadPolicyDto.</returns>
    public async Task<ReadPolicyDto?> ReadPolicyAsync(int id)
    {
        var policy = await _dbContext.Policies.FirstOrDefaultAsync(x => x.Id == id);

        if (policy == null)
        {
            _logger.LogWarning("Policy with Id {id} does not exist", id);
            return null;
        }

        return _mapper.Map<ReadPolicyDto>(policy);
    }

    /// <summary>
    /// Get all policies and their notes.
    /// </summary>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns>Returns all policies and their notes within a particular range.</returns>
    public async Task<List<ExportDto>> GetExportDataAsync(DateTime fromDate, DateTime toDate)
    {
        //Validate the 2 dates aren't backward
        var exportDtos = new List<ExportDto>();

        var policies = await _dbContext
            .Policies
            .Where(p => fromDate <= p.StartDate && p.StartDate <= toDate)
            .ToListAsync();

        foreach(var policy in policies)
        {
            var exportDto = _mapper.Map<ExportDto>(policy);

            var notes = await _dbContext
                .Notes
                .Where(x => x.PolicyId == policy.Id)
                .Select(x => x.Text)
                .ToListAsync();

            exportDto.Notes = notes;

            exportDtos.Add(exportDto);
        }

        return exportDtos;
    }
}