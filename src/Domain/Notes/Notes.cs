namespace Domain.Notes;

public class Note : BaseEntity
{
    public Guid ReportId { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public DateTime CreationDate { get; set; }
}