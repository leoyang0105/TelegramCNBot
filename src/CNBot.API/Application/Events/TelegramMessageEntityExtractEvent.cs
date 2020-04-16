using CNBot.Core.Dtos;
using CNBot.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.API.Application.Events
{ 
    public class TelegramMessageEntityExtractEvent : IntegrationEvent
    {
        public TGMessageEntityDTO[] Entities { get; set; }
        public TelegramMessageEntityExtractEvent(TGMessageEntityDTO[]  entities)
        {
            Entities = entities;
        }
    }
}
