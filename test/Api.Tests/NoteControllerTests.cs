namespace Api.Tests;

public class NoteControllerTests
{

    private readonly Mock<ILogger<NotesController>> _logger;
    private readonly Mock<INotesService> _NotesService;
    private readonly NotesController _NoteController;

    public NoteControllerTests()
    {
        _logger = new Mock<ILogger<NotesController>>();
        _NotesService = new Mock<INotesService>();

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new NoteMappingProfile());
        });
        var mapper = mockMapper.CreateMapper();

        var mc = new Mock<HttpContext>();
        _NoteController = new NotesController(_logger.Object, _NotesService.Object, mapper);
    }

    [Fact]
    public async Task Should_Return_Existing_Notes_Async()
    {
        var expected = new List<Note> {
            new Note {
                Id=Guid.NewGuid(),
                ReportId = Guid.NewGuid(),
                Author = "Zerep",
                Content = "Great text, 5*",
                CreationDate = DateTime.Parse("01/01/2022")
            },
            new Note {
                Id=Guid.NewGuid(),
                ReportId = Guid.NewGuid(),
                Author = "Lemos",
                Content = "Could be better",
                CreationDate = DateTime.Parse("11/11/2022")
            },
        };

        _NotesService.Setup(s => s.GetAllAsync()).ReturnsAsync(expected);

        // Act
        var response = await _NoteController.GetAllAsync();

        // Assert
        response.Result.Should().NotBeNull();

        var okObjectResult = Assert.IsType<OkObjectResult>(response.Result);

        okObjectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);

        okObjectResult.Value.Should().BeEquivalentTo(expected);

        _NotesService.Verify(_ => _.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_Get_Notes_ById_Async()
    {
        var expected = new Note
        {
            Id = Guid.NewGuid(),
            ReportId = Guid.NewGuid(),
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _NotesService.Setup(s => s.GetAsync(It.Is<Guid>(g => g == expected.Id.Value))).ReturnsAsync(expected);

        // Act
        var response = await _NoteController.GetAsync(expected.Id.Value);

        response.Result.Should().NotBeNull();

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(response.Result);

        okObjectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
        okObjectResult.Value.Should().BeEquivalentTo(expected);

        _NotesService.Verify(v => v.GetAsync(It.Is<Guid>(g => g == expected.Id.Value)), Times.Once);
        _NotesService.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Should_Report_Notes_Ok_Async()
    {
        var NoteId = Guid.NewGuid();
        var ReportId = Guid.NewGuid();

        var expected = new Note
        {
            Id = NoteId,
            ReportId = ReportId,
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        var ReportModel = new Models.Note
        {
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _NotesService.Setup(s => s.CreateAsync(It.IsAny<Note>())).ReturnsAsync(expected);

        // Act
        var response = await _NoteController.ReportAsync(ReportModel);

        // Assert    
        response.Result.Should().NotBeNull();

        var okObjectResult = Assert.IsType<CreatedAtActionResult>(response.Result);

        okObjectResult?.StatusCode.Should().Be(StatusCodes.Status201Created);
        okObjectResult.Value.Should().BeEquivalentTo(expected);

        _NotesService.Verify(v => v.CreateAsync(It.IsAny<Note>()), Times.Once);
    }

    [Fact]
    public async Task Should_Fail_Report_Notes_Async()
    {
        var NoteId = Guid.NewGuid();
        var ReportId = Guid.NewGuid();

        var expected = new Note
        {
            ReportId = ReportId,
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        var ReportModel = new Models.Note
        {
            ReportId = ReportId,
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _NotesService.Setup(s => s.CreateAsync(It.IsAny<Note>())).ReturnsAsync(expected);

        // Act
        var response = await _NoteController.ReportAsync(ReportModel);

        // Assert    
        response.Result.Should().NotBeNull();

        var okObjectResult = Assert.IsType<BadRequestResult>(response.Result);

        okObjectResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        _NotesService.Verify(v => v.CreateAsync(It.IsAny<Note>()), Times.Once);
        _NotesService.Verify(v => v.CreateAsync(It.Is<Note>(c => c.ReportId.Equals(ReportId))), Times.Once);
    }

    [Fact]
    public async Task Should_Delete_Notes_Async()
    {
        var guid = Guid.NewGuid();
        var expected = new Note
        {
            Id = guid,
            ReportId = Guid.NewGuid(),
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _NotesService.Setup(s => s.GetAsync(It.Is<Guid>(g => g == guid))).ReturnsAsync(expected);

        // Act
        var response = await _NoteController.DeleteAsync(guid);

        response.Should().NotBeNull();

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(response);

        noContentResult?.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        _NotesService.Verify(v => v.GetAsync(It.Is<Guid>(g => g == guid)), Times.Once);
        _NotesService.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);

        _NotesService.Verify(v => v.DeleteAsync(It.Is<Guid>(g => g == guid)), Times.Once);
        _NotesService.Verify(v => v.DeleteAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Should_Return_Not_Found_Delete_Notes_Async()
    {
        var guid = Guid.NewGuid();
        var expected = new Note
        {
            Id = guid,
            ReportId = Guid.NewGuid(),
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _NotesService.Setup(s => s.GetAsync(It.Is<Guid>(g => g == guid))).ReturnsAsync(expected);

        // Act
        var response = await _NoteController.DeleteAsync(Guid.NewGuid());

        response.Should().NotBeNull();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(response);

        notFoundResult?.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        _NotesService.Verify(v => v.GetAsync(It.Is<Guid>(g => g == guid)), Times.Never);
        _NotesService.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);

        _NotesService.Verify(v => v.DeleteAsync(It.Is<Guid>(g => g == guid)), Times.Never);
        _NotesService.Verify(v => v.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Should_Throw_Exception_Delete_Notes_Async()
    {
        var guid = Guid.NewGuid();

        var expected = new Note
        {
            Id = guid,
            ReportId = Guid.NewGuid(),
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        _NotesService.Setup(s => s.GetAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Something went wrong on the server"));

        // Act
        Func<Task> action = () => _NoteController.DeleteAsync(Guid.NewGuid());
        await action.Should().ThrowAsync<Exception>();

        // Assert
        _NotesService.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);
        _NotesService.Verify(v => v.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Should_Update_Note_On_Put_Async()
    {
        var NoteId = Guid.NewGuid();
        var ReportId = Guid.NewGuid();

        var expected = new Note
        {
            Id = NoteId,
            ReportId = ReportId,
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        var ReportModel = new Models.Note
        {
            Id = NoteId,
            ReportId = ReportId,
            Author = "Zerep",
            Content = "Was ok",
            CreationDate = DateTime.Parse("01/01/2022")
        };
        _NotesService.Setup(s => s.GetAsync(It.IsAny<Guid>())).ReturnsAsync(expected);

        _NotesService.Setup(s => s.Update(It.IsAny<Note>())).Returns(expected);

        // Act
        var response = await _NoteController.PutAsync(ReportModel.Id.Value, ReportModel);

        // Assert    
        response.Should().NotBeNull();

        var okObjectResult = Assert.IsType<NoContentResult>(response);

        okObjectResult?.StatusCode.Should().Be(StatusCodes.Status204NoContent);

        _NotesService.Verify(v => v.GetAsync(It.IsAny<Guid>()), Times.Once);
        _NotesService.Verify(v => v.Update(It.IsAny<Note>()), Times.Once);
    }

    [Fact]
    public async Task Should_Create_Note_On_Put_Async()
    {
        var NoteId = Guid.NewGuid();
        var ReportId = Guid.NewGuid();

        var expected = new Note
        {
            Id = NoteId,
            ReportId = ReportId,
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };

        var ReportModel = new Models.Note
        {
            Id = NoteId,
            ReportId = ReportId,
            Author = "Zerep",
            Content = "Great text, 5*",
            CreationDate = DateTime.Parse("01/01/2022")
        };
        _NotesService.Setup(s => s.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Note>(null));

        _NotesService.Setup(s => s.CreateAsync(It.IsAny<Note>())).ReturnsAsync(expected);

        // Act
        var response = await _NoteController.PutAsync(ReportModel.Id.Value, ReportModel);

        // Assert    
        response.Should().NotBeNull();

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(response);

        createdAtActionResult?.StatusCode.Should().Be(StatusCodes.Status201Created);

        _NotesService.Verify(v => v.CreateAsync(It.IsAny<Note>()), Times.Once);
        _NotesService.Verify(v => v.CreateAsync(It.Is<Note>(c => c.ReportId.Equals(ReportId))), Times.Once);
    }
}