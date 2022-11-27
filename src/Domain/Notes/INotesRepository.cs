using Domain;

namespace Repository.Interfaces;

public interface INotesRepository<T> : IBaseGenericRepository<T> where T : BaseEntity
{
    public Task<IEnumerable<T>> GetNotesByReportIdAsync(Guid ReportId);
}