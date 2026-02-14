using Chatly.Messages.Api.Entities;
using Chatly.Shared.Messages;

namespace Chatly.Messages.Api.Services;

public interface IMessageService
{
    Task<List<MessageDto>> GetAllAsync(CancellationToken token = default);

    Task<MessageDto?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task CreateAsync(Message message, CancellationToken token = default);
};