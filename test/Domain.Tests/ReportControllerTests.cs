namespace Domain.Tests;

public class ReportserviceTests
{
    private readonly Mock<INotesRepository<Note>> _notesRepository;
    private readonly Mock<IReportsRepository<Report>> _reportsRepository;

    private readonly ReportsService _ReportsService;

    public ReportserviceTests()
    {
        _notesRepository = new Mock<INotesRepository<Note>>();
        _reportsRepository = new Mock<IReportsRepository<Report>>();

        _ReportsService = new ReportsService(_reportsRepository.Object, _notesRepository.Object);
    }

    [Fact]
    public async Task GetAll_Returns_Existing_Reports()
    {
        // Arrange
        var expected = new List<Report>();

        _reportsRepository.Setup(s => s.GetAllAsync()).ReturnsAsync(expected);
        // Act
        var result = await _ReportsService.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}