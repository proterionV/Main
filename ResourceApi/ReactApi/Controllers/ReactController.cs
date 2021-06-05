using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReactController : ControllerBase
    {
        private UsersContext uc;

        private readonly ILogger<ReactController> _logger;

        public ReactController(ILogger<ReactController> logger)
        {
            _logger = logger;
            uc.
        }

        [HttpGet]
        public IEnumerable<UserActivity> Get()
        {
            
        }
    }
}
