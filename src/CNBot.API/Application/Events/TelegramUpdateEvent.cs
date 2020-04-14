using CNBot.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Application.Events
{
    public class TelegramUpdateEvent : IntegrationEvent
    {
        public string Body { get; set; }
    }
}
