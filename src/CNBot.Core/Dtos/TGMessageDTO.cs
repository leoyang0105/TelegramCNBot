using CNBot.Core.Entities.Messages;
using CNBot.Core.Entities.Users;
using Newtonsoft.Json;
using System.Linq;

namespace CNBot.Core.Dtos
{
    public class TGMessageDTO
    {
        [JsonProperty("message_id")]
        public long MessageId { get; set; }
        /// <summary>
        /// From user
        /// </summary>
        [JsonProperty("from")]
        public TGUserDTO From { get; set; }

        [JsonProperty("chat")]
        public TGChatDTO Chat { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        #region Forward info

        [JsonProperty("forward_from_message_id")]
        public long ForwardFromMessageId { get; set; }
        [JsonProperty("forward_signature")]
        public string ForwardSignature { get; set; }
        [JsonProperty("forward_sender_name")]
        public string ForwardSenderName { get; set; }
        [JsonProperty("forward_date")]
        public int ForwardDate { get; set; }
        /// <summary>
        /// Forward from user
        /// </summary>
        [JsonProperty("forward_from")]
        public TGUserDTO ForwardFrom { get; set; }
        [JsonProperty("forward_from_chat")]
        public TGChatDTO ForwardFromChat { get; set; }
        #endregion

        [JsonProperty("reply_to_message")]
        public object ReplyToMessage { get; set; }

        [JsonProperty("edit_date")]
        public int EditDate { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("entities")]
        public TGMessageEntityDTO[] Entities { get; set; }

        [JsonProperty("reply_markup")]
        public object ReplyMarkup { get; set; }

        public UserCommandType GetCommandType()
        {
            var text = this.Text;
            var command = UserCommandType.None;
            if (this.Entities == null || !this.Entities.Any(s => s.Type.Equals(nameof(MessageEntityType.bot_command))))
            {
                return command;
            }
            if (!text.StartsWith("/") || !ApplicationDefaults.Commands.Contains(text))
            {
                return command;
            }
            text = text.Replace(ApplicationDefaults.CNBotUserName, string.Empty);
            switch (text)
            {
                case "/help":
                    command = UserCommandType.Help;
                    break;
                case "/list":
                    command = UserCommandType.List;
                    break;
                case "/join":
                    command = UserCommandType.Join;
                    break;
                case "/search":
                    command = UserCommandType.Search;
                    break;
                case "/mylist":
                    command = UserCommandType.MyList;
                    break;

                case "/update":
                    command = UserCommandType.Update;
                    break;
                case "/remove":
                    command = UserCommandType.Remove;
                    break;
                case "/reset":
                    command = UserCommandType.Reset;
                    break;
            }
            return command;
        }
    }
}
