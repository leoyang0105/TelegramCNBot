namespace CNBot.Core.Entities.Chats
{
    public class ChatPhoto
    {
        public Chat Chat { get; set; }
        public long ChatId { get; set; }
        public string SmallFileId { get; set; }
        public string SmallFileUniqueId { get; set; }
        public string BigFileId { get; set; }
        public string BigFileUniqueId { get; set; }
    }
}
