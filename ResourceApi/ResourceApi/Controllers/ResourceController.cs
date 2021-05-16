using Microsoft.AspNetCore.Mvc;
using ResourceApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ResourceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly ILogger<ResourceController> _logger;

        public ResourceController(ILogger<ResourceController> logger)
        {
            _logger = logger;
        }

        #region requsts
        [HttpGet]
        public IEnumerable<Resource> Get()
        {
            _logger.LogInformation("Запрос всех объектов словаря");
            return DataItem.Items.Values.ToList();
        }

        [HttpGet("{id}")]
        public Resource Get(int id)
        {
            _logger.LogInformation($"Запрос объекта {id} словаря");
            return DataItem.Items.Values.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Resource resource)
        {
            if (DataItem.Items.TryAdd(resource.Id, resource))
            {
                _logger.LogInformation($"Оъект \"{resource.Id}\" добавлен");
                return Ok();
            }

            _logger.LogError($"Не удалось добавить объект \"{resource.Id}\"");
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Resource resource)
        {
            if (DataItem.Items.TryUpdate(resource.Id, resource, DataItem.Items[resource.Id]))
            {
                _logger.LogInformation($"Оъект \"{resource.Id}\" обновлен");
                return Ok();
            }

            _logger.LogError($"Невозможно обновить ресурс. {resource.Id}");
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (DataItem.Items.TryRemove(id, out _))
            {
                _logger.LogInformation($"Оъект \"{id}\" удален");
                return Ok();
            }

            _logger.LogError($"Не удалось удалить объект \"{id}\"");
            return BadRequest(ModelState);
        }
        #endregion
    }
}
