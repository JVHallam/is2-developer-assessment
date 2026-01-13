namespace DataExporter.Services;

public interface IValidationService
{
    void ThrowIfStartDateAfterToDate(DateTime fromDaste, DateTime toDate);
}