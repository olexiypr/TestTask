using System.Transactions;
using LegiosoftTestTask.Entities.Enums;
using LegiosoftTestTask.Models;
using LegiosoftTestTask.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LegiosoftTestTask.Controllers;
[ApiController, Route("api/[controller]"), Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IFileService _fileService;

    public TransactionController(ITransactionService transactionService, IFileService fileService)
    {
        _transactionService = transactionService;
        _fileService = fileService;
    }
    /// <summary>
    /// Export data from data base to .csv file. It is possible to filter the data
    /// </summary>
    /// <remarks>
    /// Sample request
    /// POST api/Transaction/export/?AllowedTypes=0&Status=1
    /// </remarks>
    /// <param name="filterModel">Filter data: Allowed types, Status</param>
    /// <returns>.csv file with (filtered) data</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="500">Server error</response>
    [HttpPost("export")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ExportAsync([FromQuery] FilterModel? filterModel)
    {
        var memoryStream = await _fileService.ExportAsync(filterModel);
        return File(memoryStream, "text/csv", "data.csv");
    }
    /// <summary>
    /// Get transaction by id
    /// </summary>
    /// <remarks>
    /// Sample request
    /// GET api/Transaction/gettransactionbyid/1
    /// </remarks>
    /// <param name="id">Transaction id</param>
    /// <returns>TransactionModel</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Is transaction not found</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="500">Server error</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TransactionModel>> GetTransactionByIdAsync(int id)
    {
        var transaction = await _transactionService.GetTransactionByIdAsync(id);
        return Ok(transaction);
    }
    /// <summary>
    /// Get all transaction
    /// </summary>
    /// <remarks>
    /// Sample request
    /// GET api/Transaction/getalltransaction/?AllowedTypes=0&Status=1
    /// </remarks>
    /// <param name="filterModel">Filter data: Allowed types, Status</param>
    /// <returns>Array of TransactionModel</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="500">Server error</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<TransactionModel>>> GetAllTransactionAsync([FromQuery] FilterModel? filterModel)
    {
        var transactions = await _transactionService.GetAllTransactionsAsync(filterModel);
        return Ok(transactions);
    }
    /// <summary>
    /// Import data from Excel file
    /// </summary>
    /// <remarks>
    /// Sample request
    /// POST api/Transaction/import
    /// </remarks>
    /// <param name="file">File with extension .xlsx or .xlsm or .xlsb or .xls</param>
    /// <returns>True if success, false if error</returns>
    /// <response code="200">Success</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="500">Server error</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ImportAsync(IFormFile file)
    {
        var res = await _fileService.ImportAsync(file);
        return Ok(res);
    }
    /// <summary>
    /// Update transaction status by id
    /// </summary>
    /// <remarks>
    /// Sample request
    /// PUT api/Transaction/updatestatus/5
    /// {
    ///     2
    /// }
    /// </remarks>
    /// <param name="id">Transaction id</param>
    /// <param name="statusToUpdate">Status in range (0, 2)</param>
    /// <returns>TransactionModel</returns>
    /// <response code="200">Success</response>
    /// <response code="400">Is transaction not found</response>
    /// <response code="401">If the user is unauthorized</response>
    /// <response code="500">Server error</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TransactionModel>> UpdateStatusAsync(int id, [FromBody] Status statusToUpdate)
    {
        var transaction = await _transactionService.UpdateStatusAsync(id, statusToUpdate);
        return Ok(transaction);
    }
}