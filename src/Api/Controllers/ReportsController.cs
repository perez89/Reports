namespace Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("Reports")]
public class ReportsController : ControllerBase
{
    private readonly ILogger<ReportsController> _logger;
    private readonly IReportsService _ReportsService;
    private readonly IMapper _mapper;

    public ReportsController(ILogger<ReportsController> logger, IReportsService ReportsService, IMapper mapper)
    {
        _logger = logger;
        _ReportsService = ReportsService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Models.Report>>> GetAllAsync()
    {
        var results = await _ReportsService.GetAllAsync();

        return Ok(results.ToList().Select(r => _mapper.Map<Models.Report>(r)).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Models.Report>> GetAsync([FromRoute] Guid id)
    {
        var result = await _ReportsService.GetAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Models.Report), (int)HttpStatusCode.Created)]
    public async Task<ActionResult<Models.Report>> ReportAsync([FromBody] Models.Report Report)
    {
        Report.Id = null;
        var result = await _ReportsService.CreateAsync(_mapper.Map<Report>(Report));

        if (result == null || !result.Id.HasValue)
            return BadRequest();

        return CreatedAtAction(nameof(GetAsync), new { id = result.Id.Value }, _mapper.Map<Models.Report>(result));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> PutAsync([FromRoute] Guid id, [FromBody] Models.Report Report)
    {
        try
        {   
            if (!id.ToString().Equals(Report.Id.ToString()))
                return BadRequest();

            var result = await _ReportsService.GetAsync(id);

            if (result == null)
            {
                result = await _ReportsService.CreateAsync(_mapper.Map<Report>(Report));

                return CreatedAtAction(nameof(GetAsync), new { id = result.Id.Value }, _mapper.Map<Models.Report>(result));
            }
            else
            {
                await _ReportsService.UpdateAsync(_mapper.Map<Report>(Report));

                return NoContent();
            }
        }
        catch (Exception ex) {
            return BadRequest("Something went wroing during the Update.");
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await _ReportsService.GetAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        await _ReportsService.DeleteAsync(id);

        return NoContent();
    }

    [HttpGet("{id:guid}/Notes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Models.Note>>> GetNotesAsync([FromRoute] Guid id)
    {
        var results = await _ReportsService.GetNotesByReportIdAsync(id);

        return Ok(results.ToList().Select(r => _mapper.Map<Models.Note>(r)).ToList());
    }
}