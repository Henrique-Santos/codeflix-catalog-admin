using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.Create;
using FC.Codeflix.Catalog.Application.UseCases.Category.Delete;
using FC.Codeflix.Catalog.Application.UseCases.Category.Get;
using FC.Codeflix.Catalog.Application.UseCases.Category.List;
using FC.Codeflix.Catalog.Application.UseCases.Category.Update;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryOutput), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryInput input, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(input, cancellationToken);

        return CreatedAtAction(nameof(Create), new { id = output.Id }, output);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(new GetCategoryInput(id), cancellationToken);

        return Ok(output);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCategoryInput(id), cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CategoryOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryInput input, CancellationToken cancellationToken)
    {
        var output = await _mediator.Send(input, cancellationToken);

        return Ok(output);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ListCategoriesOutput), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken cancellationToken, [FromQuery] int? page = null, [FromQuery] int? perPage = null, [FromQuery] string? search = null, [FromQuery] string? sort = null, [FromQuery] SearchOrder? dir = null)
    {
        var input = new ListCategoriesInput();

        BuildListCategoriesInput(page, perPage, search, sort, dir, input);

        var output = await _mediator.Send(input, cancellationToken);

        return Ok(output);
    }

    private static void BuildListCategoriesInput(int? page, int? perPage, string? search, string? sort, SearchOrder? dir, ListCategoriesInput input)
    {
        if (page.HasValue)
        {
            input.Page = page.Value;
        }
        if (perPage.HasValue)
        {
            input.PerPage = perPage.Value;
        }
        if (!string.IsNullOrWhiteSpace(search))
        {
            input.Search = search;
        }
        if (!string.IsNullOrWhiteSpace(sort))
        {
            input.Sort = sort;
        }
        if (dir.HasValue)
        {
            input.Dir = dir.Value;
        }
    }
}