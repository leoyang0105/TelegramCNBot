using CNBot.Core.Events;

namespace CNBot.API.Application.Events
{
    public class TelegramChatJoinEvent : IntegrationEvent
    {
        public long TgChatId { get; set; }
        public long TgUserId { get; set; }
        public string ChatUserName { get; set; }
        public TelegramChatJoinEvent(long tgChatId, long tgUserId, string chatUserName)
        {
            TgChatId = tgChatId;
            TgUserId = tgUserId;
            ChatUserName = chatUserName;
        }
    }
}
