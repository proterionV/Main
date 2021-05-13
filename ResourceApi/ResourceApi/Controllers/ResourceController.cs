using Microsoft.AspNetCore.Mvc;
using ResourceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ResourceApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResourceController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Resource> Get()
        {
            return DataItem.Items.Values.ToList();
        }

        [HttpGet("{id}")]
        public Resource Get(int id)
        {
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
            if (DataItem.RemoveItem(id))
                return Ok();

            return BadRequest(ModelState);
        }
    }
}
