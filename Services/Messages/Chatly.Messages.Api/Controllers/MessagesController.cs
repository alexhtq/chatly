using Chatly.Messages.Api.Entities;
using Chatly.Messages.Api.Extensions;
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
    public async Task<ActionResult<List<MessageDto>>> GetAll(
        CancellationToken token = default)
    {
        List<MessageDto> messages = await _messageService.GetAllAsync(token);

        return Ok(messages);
    }

    [HttpGet(Routes.Api.Messages.GetById)]
    [ProducesResponseType(typeof(MessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MessageDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken token = default)
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
        [FromBody] AddMessageCommand command,
        IValidator<AddMessageCommand> validator,
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

        Message message = command.MapToNewMessage();
        await _messageService.CreateAsync(message, token);

        return CreatedAtAction(
            nameof(GetById),
            new { id = message.Id },
            message.ProjectToDto());
    }
}