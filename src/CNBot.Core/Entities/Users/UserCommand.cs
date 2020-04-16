using System;

namespace CNBot.Core.Entities.Users
{
    public class UserCommand : BaseEntity
    {
        public User User { get; set; }
        public long UserId { get; set; }
        public UserCommandType Type { get; set; }
        public DateTime Created { get; set; }
        /// <summary>
        /// 命令已经处理
        /// </summary>
        public bool Completed { get; set; }
        public long TGMessageId { get; set;}
        public string Text { get; set; }
    }
}
