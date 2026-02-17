using Chatly.Messages.Api.Entities;
using Chatly.Shared.Messages;
using Chatly.Shared.Messages.Commands;
using Riok.Mapperly.Abstractions;

namespace Chatly.Messages.Api.Extensions;

[Mapper]
public static partial class MappingExtensions
{
    // Project entity to DTO.
    public static partial IQueryable<MessageDto> ProjectToDto(this IQueryable<Message> source);
    public static partial MessageDto ProjectToDto(this Message source);

    // Create entity from command.
    [MapperIgnoreTarget(nameof(Message.Id))]
    public static partial Message ToMessage(this CreateMessageCommand source);
    public static partial Message ToMessageWithId(this UpdateMessageCommand source, Guid id);

    // Update existing entity.
    public static partial void CopyTo(this Message source, Message target);
}