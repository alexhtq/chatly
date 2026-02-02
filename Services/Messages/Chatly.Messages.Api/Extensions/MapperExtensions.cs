using Chatly.Messages.Api.Entities;
using Chatly.Shared.Messages;
using Chatly.Shared.Messages.Commands;
using Riok.Mapperly.Abstractions;

namespace Chatly.Messages.Api.Extensions;

[Mapper]
public static partial class MappingExtensions
{
    public static partial IQueryable<MessageDto> ProjectToDto(this IQueryable<Message> source);

    public static partial MessageDto ProjectToDto(this Message source);

    [MapperIgnoreTarget(nameof(Message.Id))]
    public static partial Message MapToNewMessage(this AddMessageCommand source);
}