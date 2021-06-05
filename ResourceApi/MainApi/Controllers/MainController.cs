using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

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

        [HttpDelete("remove")]
        public IActionResult Delete(IEnumerable<UserActivity> ua)
        {
            _logger.LogInformation("Удаление всех записей");

            try
            {
                _repos.Remove(ua);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("saveall")]
        public IActionResult Post([FromBody] IEnumerable<UserActivity> userActivities)
        {
            _logger.LogInformation("Сохранение всех записей");

            try
            {
                _repos.SaveAll(userActivities);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("retention/{id}")]
        public IActionResult Post([FromBody] IEnumerable<UserActivity> userActivities, int id)
        {
            _logger.LogInformation($"Запрос расчета удержания: {id} дней");

            try
            {

                return Ok("{\"result\":" + _repos.Calculate(userActivities, id) + "}");
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
