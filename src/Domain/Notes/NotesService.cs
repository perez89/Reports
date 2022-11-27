namespace Domain.Notes;

public sealed class NotesService : INotesService
{
    private readonly INotesRepository<Note> _noteRepository;
    private readonly IReportsRepository<Report> _reportsRepository;

    public NotesService(INotesRepository<Note> noteRepository, IReportsRepository<Report> reportsRepository)
    {
        _noteRepository = noteRepository;
        _reportsRepository = reportsRepository;
    }

    public async Task<Note> CreateAsync(Note Note)
    {
        var Report = await _reportsRepository.GetAsync(Note.ReportId);
        if (Report == null)
            throw new ReportIdNotFoundException("The Report id of this Note does not exist.");

        var result =  await _noteRepository.CreateAsync(Note);
        await _noteRepository.SaveAsync();
        return result;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _noteRepository.DeleteAsync(id);
        await _noteRepository.SaveAsync();
        return result;
    }

    public async Task<IEnumerable<Note>> GetAllAsync()
    {
        var result = await _noteRepository.GetAllAsync();

        return result.ToList();
    }

    public async Task<Note> GetAsync(Guid id)
    {
        return await _noteRepository.GetAsync(id);
    }

    public async Task<IEnumerable<Note>> GetByReportIdAsync(Guid ReportId)
    {
        var result = await _noteRepository.GetNotesByReportIdAsync(ReportId);

        return result.ToList();
    }

    public async Task<Note> UpdateAsync(Note Note)
    {
        var note =  _noteRepository.Update(Note);
        await _noteRepository.SaveAsync();
        return note;
    }
}
