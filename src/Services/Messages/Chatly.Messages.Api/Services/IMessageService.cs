using Chatly.Shared.Messages;
using Chatly.Shared.Messages.Commands;

namespace Chatly.Messages.Api.Services;

public interface IMessageService
{
    Task<List<MessageDto>> GetAllAsync(CancellationToken token = default);
    Task<MessageDto?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<MessageDto> CreateAsync(CreateMessageCommand command, CancellationToken token = default);
    Task<MessageDto?> UpdateAsync(Guid id, UpdateMessageCommand command, CancellationToken token = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
};