namespace Api.Tests;

public class ReportControllerTests
{
    private readonly Mock<ILogger<ReportsController>> _logger;
    private readonly Mock<IReportsService> _ReportsService;
    private readonly ReportsController _ReportController;

    public ReportControllerTests()
    {
        _logger = new Mock<ILogger<ReportsController>>();
        _ReportsService = new Mock<IReportsService>();

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ReportMappingProfile());
        });
        var mapper = mockMapper.CreateMapper();

        var mc = new Mock<HttpContext>();
        _ReportController = new ReportsController(_logger.Object, _ReportsService.Object, mapper);
    }

    [Fact]
    public async Task Should_GetAll_Existing_Reports_Async()
    {
        var expected = new List<Report> {
            new Report {
                Id=Guid.NewGuid(),
                Title = "title 1",
                Content = "Random text 3",
                CreationDate = DateTime.Parse("01/01/2022")
            },
            new Report {
                Id=Guid.NewGuid(),
                Title = "title 2",
                Content = "Random text 2",
                CreationDate = DateTime.Parse("11/11/2022")
            },
        };

        _ReportsService.Setup(s => s.GetAllAsync()).ReturnsAsync(expected);

        // Act
        var response = await _ReportController.GetAllAsync();

        // Assert
        response.Result.Should().NotBeNull();

        var okObjectResult = Assert.IsType<OkObjectResult>(response.Result);

        okObjectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

        okObjectResult.Value.Should().BeEquivalentTo(expected);

        _ReportsService.Verify(_ => _.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_Get_Reports_ById_Async()
    {
        var expected = new Report
        {
            Id = Guid.NewGuid(),
            Title = "title 1",
            Content = "context xpto",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _ReportsService.Setup(s => s.GetAsync(It.Is<Guid>(g => g == expected.Id.Value))).ReturnsAsync(expected);

        // Act
        var response = await _ReportController.GetAsync(expected.Id.Value);

        response.Result.Should().NotBeNull();

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(response.Result);

        okObjectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
        okObjectResult.Value.Should().BeEquivalentTo(expected);

        _ReportsService.Verify(v => v.GetAsync(It.Is<Guid>(g => g == expected.Id.Value)), Times.Once);
        _ReportsService.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Should_Report_Async()
    {
        var ReportId = Guid.NewGuid();

        var expected = new Report
        {
            Id = ReportId,
            Title = "Title 2",
            Content = "Context text 2",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        var ReportModel = new Models.Report
        {
            Title = "Title 2",
            Content = "Context text 2",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _ReportsService.Setup(s => s.CreateAsync(It.IsAny<Report>())).ReturnsAsync(expected);

        // Act
        var response = await _ReportController.ReportAsync(ReportModel);

        // Assert    
        response.Result.Should().NotBeNull();

        var okObjectResult = Assert.IsType<CreatedAtActionResult>(response.Result);

        okObjectResult?.StatusCode.Should().Be(StatusCodes.Status201Created);
        okObjectResult.Value.Should().BeEquivalentTo(expected);

        _ReportsService.Verify(v => v.CreateAsync(It.IsAny<Report>()), Times.Once);
    }

    [Fact]
    public async Task Should_Fail_Report_Async()
    {
        var ReportModel = new Models.Report
        {
            Title = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _ReportsService.Setup(s => s.CreateAsync(It.IsAny<Report>())).Returns(Task.FromResult<Report>(null));

        // Act
        var response = await _ReportController.ReportAsync(ReportModel);

        // Assert    
        response.Result.Should().NotBeNull();

        var okObjectResult = Assert.IsType<BadRequestResult>(response.Result);

        okObjectResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        _ReportsService.Verify(v => v.CreateAsync(It.IsAny<Report>()), Times.Once);
    }

    [Fact]
    public async Task Should_Delete_Report_Async()
    {
        var guid = Guid.NewGuid();
        var expected = new Report
        {
            Id = guid,
            Title = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _ReportsService.Setup(s => s.GetAsync(It.Is<Guid>(g => g == guid))).ReturnsAsync(expected);

        // Act
        var response = await _ReportController.DeleteAsync(guid);

        response.Should().NotBeNull();

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(response);

        noContentResult?.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        _ReportsService.Verify(v => v.GetAsync(It.Is<Guid>(g => g == guid)), Times.Once);
        _ReportsService.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);

        _ReportsService.Verify(v => v.DeleteAsync(It.Is<Guid>(g => g == guid)), Times.Once);
        _ReportsService.Verify(v => v.DeleteAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Should_Not_Found_On_Delete_Report_Async()
    {
        var guid = Guid.NewGuid();
        var expected = new Report
        {
            Id = guid,
            Title = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _ReportsService.Setup(s => s.GetAsync(It.Is<Guid>(g => g == guid))).ReturnsAsync(expected);

        // Act
        var response = await _ReportController.DeleteAsync(Guid.NewGuid());

        response.Should().NotBeNull();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(response);

        notFoundResult?.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        _ReportsService.Verify(v => v.GetAsync(It.Is<Guid>(g => g == guid)), Times.Never);
        _ReportsService.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);

        _ReportsService.Verify(v => v.DeleteAsync(It.Is<Guid>(g => g == guid)), Times.Never);
        _ReportsService.Verify(v => v.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Should_Throw_Exception_On_Delete_Async()
    {
        var guid = Guid.NewGuid();

        var expected = new Report
        {
            Id = guid,
            Title = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _ReportsService.Setup(s => s.GetAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Something went wrong on the server"));

        // Act
        Func<Task> action = () => _ReportController.DeleteAsync(Guid.NewGuid());
        await action.Should().ThrowAsync<Exception>();

        // Assert
        _ReportsService.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);
        _ReportsService.Verify(v => v.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    //dsgsdfsdfsdf


    [Fact]
    public async Task Should_Update_Reports_On_Put_Async()
    {
        var ReportId = Guid.NewGuid();

        var expected = new Report
        {
            Id = ReportId,
            Title = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        var ReportModel = new Models.Report
        {
            Id = ReportId,
            Title = "Zerep",
            Content = "Was ok",
            CreationDate = DateTime.Parse("01/01/2022")
        };
        _ReportsService.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expected);

        _ReportsService.Setup(s => s.Update(It.IsAny<Report>())).Returns(expected);

        // Act
        var response = await _ReportController.PutAsync(ReportModel.Id.Value, ReportModel);

        // Assert    
        response.Should().NotBeNull();

        var okObjectResult = Assert.IsType<NoContentResult>(response);

        okObjectResult?.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        _ReportsService.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);
        _ReportsService.Verify(v => v.Update(It.IsAny<Report>()), Times.Once);
    }

    [Fact]
    public async Task Should_Create_Reports_On_Put_Async()
    {
        var ReportId = Guid.NewGuid();

        var expected = new Report
        {
            Id = ReportId,
            Title = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        var ReportModel = new Models.Report
        {
            Id = ReportId,
            Title = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };
        _ReportsService.Setup(s => s.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Report>(null));

        _ReportsService.Setup(s => s.CreateAsync(It.IsAny<Report>())).ReturnsAsync(expected);

        // Act
        var response = await _ReportController.PutAsync(ReportModel.Id.Value, ReportModel);

        // Assert    
        response.Should().NotBeNull();

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(response);

        createdAtActionResult?.StatusCode.Should().Be(StatusCodes.Status201Created);

        _ReportsService.Verify(v => v.CreateAsync(It.IsAny<Report>()), Times.Once);
        _ReportsService.Verify(v => v.CreateAsync(It.Is<Report>(c => c.Id.Equals(ReportId))), Times.Once);
    }
}