using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CNBot.API.Application.Events;
using CNBot.Core.EventBus.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public IActionResult Get()
        {
            _logger.LogInformation("test");
            return Ok();
        }
        [HttpPost]
        public IActionResult Post([FromBody]object model)
        {
            try
            {
                _logger.LogInformation(model.ToString());
                _eventBus.Publish(new TelegramUpdateEvent { Body = model.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发布消息推送失败");
            }
            return Ok();
        }
    }
}