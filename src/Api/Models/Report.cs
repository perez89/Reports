namespace Api.Models;

public record Report
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime CreationDate { get; set; }
}