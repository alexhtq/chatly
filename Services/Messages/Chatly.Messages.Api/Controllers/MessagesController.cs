using Chatly.Messages.Api.Database;
using Chatly.Messages.Api.Extensions;
using Chatly.Shared.Constants;
using Chatly.Shared.Messages;
using Chatly.Shared.Messages.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chatly.Messages.Api.Controllers;
 
[ApiController]
public class MessagesController(MessagesContext context) : ControllerBase
{
    private readonly MessagesContext _context = context;
    
    [HttpGet(Routes.Api.Messages.GetAll)]
    [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MessageDto>>> GetAll()
    {
        var messages = await _context.Messages
            .ProjectToDto()
            .ToListAsync();
        
        return Ok(messages);
    }
    
    [HttpPost(Routes.Api.Messages.Create)]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageDto>> Create(
        [FromBody] AddMessageCommand command,
        IValidator<AddMessageCommand> validator)
    {
        var validationResult = await validator.ValidateAsync(command);

        if (validationResult.IsValid is false)
        {
            var problem = ProblemDetailsFactory.CreateProblemDetails(
                httpContext: HttpContext,
                statusCode: StatusCodes.Status400BadRequest,
                detail: "One or more validation errors occurred.");
            
            problem.Extensions.Add("errors", validationResult.ToDictionary());

            return BadRequest(problem);
        }

        var message = command.MapToNewMessage();
        
        await _context.Messages.AddAsync(message);
        
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Get),
            new { id = message.Id },
            message.ProjectToDto());
    }

    [HttpGet(Routes.Api.Messages.Get)]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageDto?>> Get([FromRoute] Guid id)
    {
        var message = await _context.Messages
            .ProjectToDto()
            .FirstOrDefaultAsync(m => m.Id == id);
            
        if (message is null)
        {
            return NotFound();
        }

        return Ok(message);
    }
}