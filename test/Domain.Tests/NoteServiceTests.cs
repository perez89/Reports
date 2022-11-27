namespace Domain.Tests;

public class NoteServiceTests
{

    private readonly Mock<INotesRepository<Note>> _NotesRepository;
    private readonly Mock<IReportsRepository<Report>> _ReportsRepository;

    private readonly NotesService _NotesService;

    public NoteServiceTests()
    {
        _NotesRepository = new Mock<INotesRepository<Note>>();
        _ReportsRepository = new Mock<IReportsRepository<Report>>();

        _NotesService = new NotesService(_NotesRepository.Object, _ReportsRepository.Object);
    }

    [Fact]
    public async Task GetAll_Returns_Existing_Notes()
    {
        // Arrange
        var expected = new List<Note>();

        _NotesRepository.Setup(s => s.GetAllAsync()).ReturnsAsync(expected);
        // Act
        var result = await _NotesService.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Report_Create_Fail_Id_NotExist_Notes()
    {
        var Note = new Note
        {
            ReportId = Guid.NewGuid(),
            Author = "Perez",
            Content = "great Report",
            CreationDate = DateTime.Now,
        };

        // Arrange
        _ReportsRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Report>(null));

        // Act
        Func<Task> action = () => _NotesService.CreateAsync(Note);
        await action.Should().ThrowAsync<ReportIdNotFoundException>();

        // Assert
        _ReportsRepository.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);
        _NotesRepository.Verify(v => v.CreateAsync(It.IsAny<Note>()), Times.Never);
    }

    [Fact]
    public async Task Report_Create_Ok_Notes()
    {
        var ReportId = Guid.NewGuid();
        var Report = new Report
        {
            Id = ReportId,
            Title = "Title xpto",
            Content = "Book.....",
            CreationDate = DateTime.Now,
        };

        var Note = new Note
        {
            ReportId = ReportId,
            Author = "Perez",
            Content = "great Report",
            CreationDate = DateTime.Now,
        };

        var createNote = new Note
        {
            Id = Guid.NewGuid(),
            ReportId = Note.ReportId,
            Author = Note.Author,
            Content = Note.Author,
            CreationDate = Note.CreationDate
        };

        // Arrange
        _ReportsRepository.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(Report);
        _NotesRepository.Setup(s => s.CreateAsync(It.IsAny<Note>())).ReturnsAsync(createNote);

        // Act
        var result = await _NotesService.CreateAsync(Note);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createNote);

        // Assert
        _ReportsRepository.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);
        _NotesRepository.Verify(v => v.CreateAsync(It.IsAny<Note>()), Times.Once);
    }
}