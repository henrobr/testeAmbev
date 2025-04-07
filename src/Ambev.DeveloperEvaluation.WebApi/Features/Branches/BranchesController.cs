using Ambev.DeveloperEvaluation.Application.Branches.Commands.Create;
using Ambev.DeveloperEvaluation.Application.Branches.Queries.GetAll;
using Ambev.DeveloperEvaluation.Domain.Models.BranchAggregate.DTOs;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches;

/// <summary>
/// Controller for managing branches operation
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BranchesController(IMediator mediator) : BaseController
{
    /// <summary>
    /// Creates a new branch.
    /// </summary>
    /// <param name="command">The command containing the branch data to be created.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// Returns <see cref="ApiResponseWithData{Guid}"/> with the ID of the created branch  
    /// or <see cref="ApiResponse"/> with a bad request status if validation fails.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBranchCommand command,
        CancellationToken cancellationToken)
    {
        var validator = new CreateBranchValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.ToApiResponse());

        return Ok(await mediator.Send(command, cancellationToken));
    }

    /// <summary>
    /// Retrieves a paginated list of branches, optionally filtered by name.
    /// </summary>
    /// <param name="name">Optional name filter to search branches by name.</param>
    /// <param name="page">The current page number (default is 1).</param>
    /// <param name="pageSize">The number of records per page (default is 10).</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>
    /// Returns <see cref="ApiResponseWithData{BranchDto}"/> with a paginated list of branches  
    /// or <see cref="ApiResponse"/> with a bad request status if an error occurs.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<BranchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        string? name = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = await mediator.Send(new GetAllBranchQuery(name));

        var response = await PaginatedList<BranchDto>
            .CreateAsync(query, page, pageSize, cancellationToken);

        return OkPaginated(response);
    }
}
