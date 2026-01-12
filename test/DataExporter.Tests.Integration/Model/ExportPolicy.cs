namespace DataExporter.Tests.Integration.Models;

public class ExportDto
{
    public string? PolicyNumber { get; set; }
    public decimal Premium { get; set; }
    public DateTime StartDate { get; set; }
    public List<string> Notes { get; set; }
}