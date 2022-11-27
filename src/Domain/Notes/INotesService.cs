namespace Domain.Notes;

public interface INotesService
{
    public Task<IEnumerable<Note>> GetAllAsync();
    public Task<Note> GetAsync(Guid id);
    public Task<Note> CreateAsync(Note Note);
    public Note Update(Note Note);
    public Task<bool> DeleteAsync(Guid id);
}
