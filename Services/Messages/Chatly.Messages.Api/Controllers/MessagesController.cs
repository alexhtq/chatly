using Chatly.Messages.Api.Attributes;
using Chatly.Messages.Api.Services;
using Chatly.Shared.Constants;
using Chatly.Shared.Messages;
using Chatly.Shared.Messages.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Chatly.Messages.Api.Controllers;
 
[ApiController]
public class MessagesController(IMessageService messageService) : ControllerBase
{    
    [HttpGet(Routes.Api.Messages.GetAll)]
    [ProducesResponseType(typeof(List<MessageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MessageDto>>> GetAll(CancellationToken token = default)
    {
        List<MessageDto> messages = await messageService.GetAllAsync(token);

        return Ok(messages);
    }

    [HttpGet(Routes.Api.Messages.GetById)]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageDto>> GetById([FromRoute] Guid id, CancellationToken token = default)
    {
        MessageDto? message = await messageService.GetByIdAsync(id, token);
            
        if (message is null)
        {
            return NotFound();
        }

        return Ok(message);
    }

    [HttpPost(Routes.Api.Messages.Create)]
    [Idempotent] // Idempotency is optional and applies only when `Idempotency-Key` is provided
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MessageDto>> Create(
        [FromBody] CreateMessageCommand command,
        CancellationToken token = default)
    {
        MessageDto createdMessage = await messageService.CreateAsync(command, token);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdMessage.Id },
            createdMessage);
    }

    [HttpPut(Routes.Api.Messages.Update)]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMessageCommand command,
        CancellationToken token = default)
    {
        MessageDto? updatedMessage = await messageService.UpdateAsync(id, command, token);
            
        if (updatedMessage is null)
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
        bool isDeleted = await messageService.DeleteAsync(id, token);

        if (isDeleted is false)
        {
            return NotFound();
        }

        return NoContent();
    }
}