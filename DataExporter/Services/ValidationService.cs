namespace DataExporter.Services;

public class ValidationService : IValidationService
{
    private readonly ILogger<ValidationService> _logger;

    public ValidationService(ILogger<ValidationService> logger)
    {
        _logger = logger;
    }

    public void ThrowIfStartDateAfterToDate(DateTime fromDate, DateTime toDate)
    {
        if(fromDate > toDate)
        {
            var errorMessage = "Dates are invalid, from date '{0}' needs to be before the to date {1}";
            _logger.LogError(errorMessage, fromDate, toDate);
            throw new ArgumentException(String.Format(errorMessage, fromDate, toDate));
        }
    }
}