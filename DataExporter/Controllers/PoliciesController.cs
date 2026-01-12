using DataExporter.Dtos;
using DataExporter.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataExporter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PoliciesController : ControllerBase
    {
        private IPolicyService _policyService;

        public PoliciesController(IPolicyService policyService) 
        { 
            _policyService = policyService;
        }

        [HttpPost]
        public async Task<IActionResult> PostPolicies([FromBody]CreatePolicyDto createPolicyDto)
        {         
            var result = await _policyService.CreatePolicyAsync(createPolicyDto);
            var location = $"/Policies/{result.Id}";
            return Created(location, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPolicies()
        {
            var results = await _policyService.ReadPoliciesAsync();
            return Ok(results);
        }

        [HttpGet("{policyId}")]
        public async Task<IActionResult> GetPolicy(int policyId)
        {
            var result = await _policyService.ReadPolicyAsync(policyId);
            return Ok(result);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportData([FromQuery]DateTime startDate, [FromQuery] DateTime endDate)
        {
            var a = new List<ExportDto>()
            {
                new ExportDto()
            };

            return Ok(a);
        }
    }
}