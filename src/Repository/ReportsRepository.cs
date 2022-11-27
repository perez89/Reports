namespace Repository;

public sealed class ReportsRepository : BaseGenericRepository<Report>, IReportsRepository<Report>
{
    public ReportsRepository(ArchiveContext context) : base(context)
    {
    }

    //public async Task<IEnumerable<Report>> GetAllAsync()
    //{
    //    return await _context.Reports.ToListAsync();
    //}

    //public async Task<Report> GetAsync(Guid id)
    //{
    //    return await _context.Reports.FirstOrDefaultAsync(p => id.Equals(p.Id));
    //}

    //public async Task<Report> CreateAsync(Report Report)
    //{
    //    var result = await _context.Reports.AddAsync(Report);

    //    await SaveAsync();
    //    return result.Entity;
    //}

    //public async Task<Report> UpdateAsync(Report Report)
    //{
    //    var pst = await _context.Reports.FirstOrDefaultAsync(p => Report.Id.Equals(p.Id));

    //    if (pst != null)
    //    {
    //        pst.Title = Report.Title;
    //        pst.Content = Report.Content;
    //        await SaveAsync();
    //    }
    //    return pst;
    //}

    //public async Task<bool> DeleteAsync(Guid id)
    //{
    //    var pst = await _context.Reports.FirstOrDefaultAsync(p => id.Equals(p.Id));

    //    if (pst != null)
    //    {
    //        _context.Reports.Remove(pst);
    //        await SaveAsync();

    //        return true;
    //    }

    //    return false;
    //}
}