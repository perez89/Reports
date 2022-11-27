namespace Domain.Reports;

public class Report : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreationDate { get; set; }
}