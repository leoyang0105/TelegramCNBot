using System;

namespace CNBot.Core.Entities.Chats
{
    public class Category : BaseEntity
    {
        public static string AllCacheKey => "categories_all";
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public int DisplayOrder { get; set; }
        public bool Published { get; set; }
    }
}
