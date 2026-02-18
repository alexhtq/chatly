using Chatly.Messages.Api.Database;
using Chatly.Messages.Api.Entities;
using Chatly.Messages.Api.Extensions;
using Chatly.Shared.Messages;
using Chatly.Shared.Messages.Commands;
using Microsoft.EntityFrameworkCore;

namespace Chatly.Messages.Api.Services;

public class MessageService(MessagesContext context) : IMessageService
{
    private readonly MessagesContext _context = context;

    public async Task<List<MessageDto>> GetAllAsync(CancellationToken token = default)
    {
        List<MessageDto> messages = await _context.Messages
            .ProjectToDto()
            .ToListAsync(token);
        
        return messages;
    }

    public async Task<MessageDto?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        MessageDto? message = await _context.Messages
            .ProjectToDto()
            .FirstOrDefaultAsync(m => m.Id == id, token);
            
        return message;
    }

    public async Task<MessageDto> CreateAsync(
        CreateMessageCommand command,
        CancellationToken token = default)
    {      
        Message message = command.ToMessage();
        
        await _context.Messages.AddAsync(message, token);
        
        await _context.SaveChangesAsync(token);

        return message.ProjectToDto();
    }

    public async Task<MessageDto?> UpdateAsync(
        Guid id,
        UpdateMessageCommand command,
        CancellationToken token = default)
    {
        var messageInput = command.ToMessageWithId(id);
        
        Message? existingMessage = await _context.Messages
            .FirstOrDefaultAsync(m => m.Id == messageInput.Id, token);

        if (existingMessage is null)
        {
            return null;
        }

        messageInput.CopyTo(existingMessage);

        await _context.SaveChangesAsync(token);

        return existingMessage.ProjectToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
    {
        var existingMessage =  await _context.Messages.FindAsync(id, token);

        if (existingMessage is null)
        {
            return false;
        }

        _context.Messages.Remove(existingMessage);

        await _context.SaveChangesAsync(token);

        return true;
    }
}