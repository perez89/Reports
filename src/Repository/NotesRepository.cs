namespace Repository;

public sealed class NotesRepository : BaseGenericRepository<Note>, INotesRepository<Note>, IDisposable
{
    private readonly ArchiveContext _context;

    public NotesRepository(ArchiveContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Note>> GetNotesByReportIdAsync(Guid ReportId)
    {
        return await _context.Notes.Where(c => ReportId.Equals(c.ReportId)).ToListAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    //public async Task<IEnumerable<Note>> GetAllAsync()
    //{
    //    return await _context.Notes.ToListAsync();
    //}

    //public async Task<Note> GetAsync(Guid id)
    //{
    //    return await _context.Notes.FirstOrDefaultAsync(c => id.Equals(c.Id));
    //}

    //public async Task<Note> CreateAsync(Note Note)
    //{
    //    var result = await _context.Notes.AddAsync(Note);

    //    await SaveAsync();

    //    return result.Entity;
    //}

    //public async Task<Note> UpdateAsync(Note Note)
    //{
    //    var cmt = await _context.Notes.FirstOrDefaultAsync(c => Note.Id.Equals(c.Id));

    //    if (cmt != null)
    //    {
    //        cmt.Author = Note.Author;
    //        cmt.Content = Note.Content;
    //        await SaveAsync();
    //    }

    //    return cmt;
    //}

    //public async Task<bool> DeleteAsync(Guid id)
    //{
    //    var cmt = await _context.Notes.FirstOrDefaultAsync(c => id.Equals(c.Id));

    //    if (cmt != null)
    //    {
    //        _context.Notes.Remove(cmt);
    //        await SaveAsync();

    //        return true;
    //    }

    //    return false;
    //}

}