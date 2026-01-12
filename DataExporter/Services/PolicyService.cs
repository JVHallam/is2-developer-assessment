using DataExporter.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DataExporter.Services;

public class PolicyService
{
    private readonly ExporterDbContext _dbContext;
    private readonly ILogger<PolicyService> _logger;
    private readonly IMappingService _mappingService;

    public PolicyService(
        ExporterDbContext dbContext,
        ILogger<PolicyService> logger,
        IMappingService mappingService)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
        _logger = logger;
        _mappingService = mappingService;
    }

    /// <summary>
    /// Creates a new policy from the DTO.
    /// </summary>
    /// <param name="policy"></param>
    /// <returns>Returns a ReadPolicyDto representing the new policy, if succeded. Returns null, otherwise.</returns>
    public async Task<ReadPolicyDto?> CreatePolicyAsync(CreatePolicyDto createPolicyDto)
    {
        var policy = _mappingService.MapToEntity(createPolicyDto);

        _dbContext.Policies.Add(policy);

        await _dbContext.SaveChangesAsync();

        //Then map to a read dto
        var asReadDto = _mappingService.MapToDto(policy);

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
            .Select(_mappingService.MapToDto)
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

        return _mappingService.MapToDto(policy);
    }
}