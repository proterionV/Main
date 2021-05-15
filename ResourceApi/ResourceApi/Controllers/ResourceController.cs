using Microsoft.AspNetCore.Mvc;
using ResourceApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace ResourceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourceController : ControllerBase
    {
        public ResourceController()
        {

        }

        #region requsts
        [HttpGet]
        public IEnumerable<Resource> Get()
        {
            Trace.TraceInformation("Запрос всех объектов словаря");
            return DataItem.Items.Values.ToList();
        }

        [HttpGet("{id}")]
        public Resource Get(int id)
        {
            Trace.TraceInformation($"Запрос объекта {id} словаря");
            return DataItem.Items.Values.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Resource resource)
        {
            if (DataItem.AddItem(resource))
            {
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Resource resource)
        {
            if (DataItem.UpdateItem(resource))
            {
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (DataItem.Items.TryRemove(id, out _))
            {
                Trace.TraceInformation($"Оъект \"{id}\" успешно удален");
                return Ok();
            }

            Trace.TraceError($"Не удалось удалить объект \"{id}\"");
            return BadRequest(ModelState);
        }
        #endregion
    }
}
