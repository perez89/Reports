namespace Repository;

public class ArchiveContext : DbContext
{
    public ArchiveContext(DbContextOptions<ArchiveContext> options) : base(options)
    {
    }

    public DbSet<Note> Notes { get; set; }

    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Note>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Note>()
            .Property(b => b.CreationDate)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Note>()
            .Property(b => b.Content)
            .HasMaxLength(120);

        modelBuilder.Entity<Note>()
            .Property(b => b.Author)
            .HasMaxLength(30);

        modelBuilder.Entity<Note>()
            .HasOne<Report>()
            .WithMany()
            .HasForeignKey(b => b.ReportId);

        modelBuilder.Entity<Report>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Report>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Report>()
            .Property(b => b.CreationDate)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Report>()
            .Property(b => b.Title)
            .HasMaxLength(30);

        modelBuilder.Entity<Report>()
            .Property(b => b.Content)
            .HasMaxLength(12000);
    }
}

