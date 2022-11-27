namespace Api.Models;

public record Note
{
    public Guid? Id { get; set; }
    public Guid ReportId { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public DateTime CreationDate { get; set; }
}