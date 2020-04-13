using System;
using System.Collections.Generic;
using System.Text;

namespace CNBot.Core.Entities.Messages
{
    public enum MessageType
    {
        Text,
        Photo,
        Audio,
        Document,
        Voice,
        Video,
        VideoNote,
        Animation,
        MediaGroup,
        Location
    }
}
