using Chatly.Messages.Api.Database;
using Chatly.Messages.Api.Entities;
using Chatly.Messages.Api.Extensions;
using Chatly.Shared.Messages;
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

    public async Task CreateAsync(Message message, CancellationToken token = default)
    {      
        await _context.Messages.AddAsync(message, token);
        
        await _context.SaveChangesAsync(token);
    }
}