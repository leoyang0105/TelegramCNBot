namespace CNBot.Core.Entities.Messages
{
    public class MessageEntity : BaseEntity
    {
        public Message Message { get; set; }
        public long MessageId { get; set; }
        public MessageEntityType Type { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public string Url { get; set; }
        public int? TGUserId { get; set; }
        public string Language { get; set; }
    }
}
