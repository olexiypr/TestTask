using LegiosoftTestTask.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LegiosoftTestTask.Controllers;

[ApiController, Route("api/[controller]"), Authorize, Produces("application/json")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }
    /// <summary>
    /// SearchAsync client by name
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// GET api/search?searchLine=Colt
    /// </remarks>
    /// <param name="searchLine">Client name</param>
    /// <returns>Array of transaction or empty array if transaction not found</returns>
    /// <response code="200">Success</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Search(string searchLine)
    {
        var result = await _searchService.SearchAsync(searchLine);
        return Ok(result);
    }
}