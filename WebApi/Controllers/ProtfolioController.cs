using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Protfolios;

[ApiController]
[Route("api/[controller]")]
public class ProtfolioController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProtfolioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //// GET: api/Protfolio
    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<ProtfolioDto>>> GetAll()
    //{
    //    var result = await _mediator.Send(new GetProtfoliosQuery());
    //    return Ok(result); // Returns a list of ProtfolioDto
    //}

    // GET: api/Protfolio/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProtfolioDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new List.Query { Id = id });
        if (result == null)
            return NotFound();
        return Ok(result); // Returns a single ProtfolioDto
    }

    // POST: api/Protfolio
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Command command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = command.Protfolio.Id }, null);
    }

    //// PUT: api/Protfolio/{id}
    //[HttpPut("{id:guid}")]
    //public async Task<IActionResult> Update(Guid id, [FromBody] Command command)
    //{
    //    if (id != command.Protfolio.Id)
    //        return BadRequest("ID in the URL and body do not match.");

    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    await _mediator.Send(command);
    //    return NoContent();
    //}

    //// DELETE: api/Protfolio/{id}
    //[HttpDelete("{id:guid}")]
    //public async Task<IActionResult> Delete(Guid id)
    //{
    //    await _mediator.Send(new DeleteProtfolioCommand { Id = id });
    //    return NoContent();
    //}
}
