namespace Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("Notes")]
public class NotesController : ControllerBase
{
    private readonly ILogger<NotesController> _logger;
    private readonly INotesService _NotesService;
    private readonly IMapper _mapper;

    public NotesController(ILogger<NotesController> logger, INotesService NotesService, IMapper mapper)
    {
        _logger = logger;
        _NotesService = NotesService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Models.Note>>> GetAllAsync()
    {
        var Notes = await _NotesService.GetAllAsync();

        var results = Notes?.ToList().Select(r => _mapper.Map<Models.Note>(r)).ToList();
        return Ok(results);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.Note>> GetAsync([FromRoute] Guid id)
    {
        var result = await _NotesService.GetAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<Models.Note>(result));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Models.Note>> ReportAsync([FromBody] Models.Note Note)
    {
        Note.Id = null;
        var result = await _NotesService.CreateAsync(_mapper.Map<Note>(Note));

        if (result == null || !result.Id.HasValue)
            return BadRequest();

        return CreatedAtAction(nameof(GetAsync), new { id = result.Id.Value }, _mapper.Map<Models.Note>(result));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PutAsync([FromRoute] Guid id, [FromBody] Models.Note Note)
    {
        var result = await _NotesService.GetAsync(id);

        if (result == null)
        {
            result = await _NotesService.CreateAsync(_mapper.Map<Note>(Note));

            return CreatedAtAction(nameof(GetAsync), new { id = result.Id.Value }, _mapper.Map<Models.Note>(result));
        }
        else
        {
            _NotesService.UpdateAsync(_mapper.Map<Note>(Note));

            return NoContent();
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await _NotesService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        await _NotesService.DeleteAsync(id);

        return NoContent();
    }
}