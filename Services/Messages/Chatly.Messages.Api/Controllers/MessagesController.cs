using Chatly.Messages.Api.Services;
using Chatly.Shared.Constants;
using Chatly.Shared.Messages;
using Chatly.Shared.Messages.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chatly.Messages.Api.Controllers;
 
[ApiController]
public class MessagesController(IMessageService messageService) : ControllerBase
{
    private readonly IMessageService _messageService = messageService;
    
    [HttpGet(Routes.Api.Messages.GetAll)]
    [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MessageDto>>> GetAll(CancellationToken token = default)
    {
        List<MessageDto> messages = await _messageService.GetAllAsync(token);

        return Ok(messages);
    }

    [HttpGet(Routes.Api.Messages.GetById)]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageDto>> GetById([FromRoute] Guid id, CancellationToken token = default)
    {
        MessageDto? message = await _messageService.GetByIdAsync(id, token);
            
        if (message is null)
        {
            return NotFound();
        }

        return Ok(message);
    }
    
    [HttpPost(Routes.Api.Messages.Create)]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageDto>> Create(
        [FromBody] CreateMessageCommand command,
        IValidator<CreateMessageCommand> validator,
        CancellationToken token = default)
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

        var message = await _messageService.CreateAsync(command, token);

        return CreatedAtAction(
            nameof(GetById),
            new { id = message.Id },
            message);
    }

    [HttpPut(Routes.Api.Messages.Update)]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMessageCommand command,
        CancellationToken token = default)
    {     
        MessageDto? updatedMessage = await _messageService.UpdateAsync(id, command, token);
            
        if (updatedMessage == null)
        {
            return NotFound();
        }

        return Ok(updatedMessage);
    }

    [HttpDelete(Routes.Api.Messages.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken token = default)
    {
        bool isDeleted = await _messageService.DeleteAsync(id, token);

        if (isDeleted is false)
        {
            return NotFound();
        }

        return NoContent();
    }
}