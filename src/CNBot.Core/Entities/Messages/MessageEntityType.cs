using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Core.Entities.Messages
{
    public enum MessageEntityType
    {
        none,
        mention,
        hashtag,
        cashtag,
        bot_command,
        url,
        email,
        phone_number,
        bold,
        italic,
        underline,
        strikethrough,
        code,
        pre,
        text_link,
        text_mention
    }
}
