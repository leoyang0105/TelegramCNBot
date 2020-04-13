using System;

namespace CNBot.Core.Entities.Users
{
    public class User : BaseEntity
    {
        public int TGUserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public bool IsBot { get; set; }
        public string LanguageCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
