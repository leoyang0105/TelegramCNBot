using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CNBot.API.Application.Events;
using CNBot.Core.Dtos;
using CNBot.Core.EventBus.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CNBot.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PushController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEventBus _eventBus;
        public PushController(
            ILogger<PushController> logger,
            IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        [HttpPost]
        public IActionResult Post([FromBody]object model)
        {
            try
            {
                var dto = JsonConvert.DeserializeObject<TGUpdateDTO>(model.ToString());
                if (dto.Message != null)
                {
                    _eventBus.Publish(new TelegramMessageEvent(dto.Message));
                }
                else
                {
                    _eventBus.Publish(new TelegramCallbackQueryEvent(dto.CallbackQuery));
                }
                _logger.LogInformation(model.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "接收消息推送失败");
            }
            return Ok();
        }
    }
}