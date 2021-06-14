using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using MainApi.Models;
using MainApi.Services;

namespace MainApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : Controller
    {
        private readonly UsersRepository _repos;
        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger, IRepository repos)
        {
            _logger = logger;
            _repos = (UsersRepository)repos;
        }

        #region responses
        [HttpGet("all")]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Запрос всех записей");
                return Ok(_repos.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("saveone")]
        public IActionResult Post(UserActivity userActivity)
        {
            _logger.LogInformation($"Добавление пользователя: {userActivity.UserID}");

            try
            {
                _repos.SaveOne(userActivity);
                return Ok(userActivity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("removeone/{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Удаление пользователя {id}");

            try
            {
                _repos.RemoveOne(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("retention")]
        public IActionResult GetRetention()
        {
            _logger.LogInformation($"Запрос расчета удержания");

            try
            {
                return Ok(_repos.Calculate(_repos.GetAll()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public IActionResult Put([FromBody] UserActivity userActivity)
        {
            _logger.LogInformation($"Изменение записи {userActivity.UserID}");

            try
            {
                _repos.Update(userActivity);
                return Ok(userActivity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("execution")]
        public IActionResult GetExecution()
        {
            try
            {
                _logger.LogInformation("Запрос времени выполнения запросов");
                return Ok(_repos.GetExecutionTimes());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
