namespace Domain.Reports;

public interface IReportsService
{
    public Task<IEnumerable<Report>> GetAllAsync();
    public Task<Report> GetAsync(Guid id);
    public Task<Report> CreateAsync(Report Report);
    public Report Update(Report Report);
    public Task<bool> DeleteAsync(Guid id);
    public Task<IEnumerable<Note>> GetNotesByReportIdAsync(Guid ReportId);

}
