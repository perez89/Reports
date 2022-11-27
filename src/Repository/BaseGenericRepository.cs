using Domain;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Repository;

public abstract class BaseGenericRepository<T>: IBaseGenericRepository<T> where T : BaseEntity
{
    protected readonly ArchiveContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public BaseGenericRepository(ArchiveContext context)
    {
        this._dbContext = context;
        this._dbSet = _dbContext?.Set<T>();
    }

    public async Task<T> CreateAsync(T report)
    {
        var result = await _dbSet.AddAsync(report);

        return result.Entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var cmt = await _dbSet.FirstOrDefaultAsync(c => id.Equals(c.Id));

        if (cmt != null)
        {
            _dbSet.Remove(cmt);

            return true;
        }

        return false;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(c => id.Equals(c.Id));
    }

    public async Task<T> UpdateAsync(T report)
    {
        //_dbSet.Attach(report);
        //_dbContext.Entry(report).State = EntityState.Modified;

        //return report;
        var original = await _dbSet.FirstOrDefaultAsync(c => report.Id.Equals(c.Id));
        if (original != null) {
            _dbSet.Attach(original);
            _dbContext.Entry(original)
            .CurrentValues
                        .SetValues(report);
            _dbContext.Entry(original).State = EntityState.Modified;
        }

        return report;
        //var cmt = await _dbSet.FirstOrDefaultAsync(c => report.Id.Equals(c.Id));

        ////if (cmt != null)
        ////{
        ////    cmt.Author = report.Author;
        ////    cmt.Content = report.Content;
        ////    await SaveAsync();
        ////}

        //return cmt;
    }

  

    //public async Task<IEnumerable<T>> GetAllAsync()
    //{
    //    return await _context.dbs .ToListAsync();
    //}

    //public async Task<T> GetAsync(Guid id)
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

    //public async Task<IEnumerable<Note>> GetNotesByReportIdAsync(Guid ReportId)
    //{
    //    return await _context.Notes.Where(c => ReportId.Equals(c.ReportId)).ToListAsync();
    //}

    public async Task<int> SaveAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}