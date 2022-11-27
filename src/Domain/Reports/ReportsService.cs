namespace Domain.Reports;

public sealed class ReportsService : IReportsService
{
    private readonly IReportsRepository<Report> _reportRepository;
    private readonly INotesRepository<Note> _notesRepository;
    public ReportsService(IReportsRepository<Report> reportRepository, INotesRepository<Note> notesRepository)
    {
        _reportRepository = reportRepository;
        _notesRepository = notesRepository;
    }

    public async Task<Report> CreateAsync(Report Report)
    {
        if (Report.Id.HasValue)
            return null;

        var result = await _reportRepository.CreateAsync(Report);
        await _reportRepository.SaveAsync();
        return result;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _reportRepository.DeleteAsync(id);
        await _reportRepository.SaveAsync();
        return result;
    }

    public async Task<IEnumerable<Report>> GetAllAsync()
    {
        var result = await _reportRepository.GetAllAsync();

        return result.ToList();
    }

    public async Task<Report> GetAsync(Guid id)
    {
        return await _reportRepository.GetAsync(id);
    }

    public async Task<IEnumerable<Note>> GetNotesByReportIdAsync(Guid ReportId)
    {
        var result = await _notesRepository.GetNotesByReportIdAsync(ReportId);
        return result.ToList();
    }

    public async Task<Report> UpdateAsync(Report Report)
    {
        var result = await _reportRepository.UpdateAsync(Report);
        await _reportRepository.SaveAsync();
        return result;
    }
}
