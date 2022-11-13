using Microsoft.AspNetCore.Mvc;
using TaskDeadlineBusinessLogic;
using TaskDeadlineDataServices;
using TaskDeadlineDataServices.Models;

namespace TaskDeadlineWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskLogic _taskLogic;
        private readonly ILogger<TaskController> _logger;
        private IDataService _dbServiceRepository;
        private IConfiguration _config;
        public TaskController(ITaskLogic taskLogicReposistory, ILogger<TaskController> logger, IDataService dbServiceRepository, IConfiguration config)
        {
            _taskLogic = taskLogicReposistory;
            _logger = logger;
            _dbServiceRepository = dbServiceRepository;
            _config = config;

        }

        /// <summary>
        ///Calculate the end date of a task.
        /// </summary>
        /// <param name="taskStartDate">start date of the task.</param>
        /// <param name="workingDays">how many working days does the employee need to complete this task?</param>
        /// <returns>End date of this task.</returns>

        [HttpGet]
        [Route("CalculateTaskEndDate")]
        public IActionResult CalculateTaskEndDate(DateTime taskStartDate, double workingDays)
        {
            if (workingDays > 0)
            {
                _logger.LogInformation(" TaskController.CalculateTaskEndDate | CalculateTaskEndDate process start in the TaskController at {Time}", DateTime.UtcNow);
                var result = _taskLogic.CalculateTaskEndDate(taskStartDate, workingDays);
                _logger.LogInformation(" TaskController.CalculateTaskEndDate | CalculateTaskEndDate process complete in the TaskController at {Time}", DateTime.UtcNow);
                return Ok(result);

            }
            else
            {
                _logger.LogInformation(" TaskController.CalculateTaskEndDate | CalculateTaskEndDate process end with bad request in the TaskController at {Time}", DateTime.UtcNow);
                return BadRequest("Please enter the valid details");
            }
        }

        [HttpPost]
        [Route("AddHolidayDetails")]
        public IActionResult AddHolidayDetails(HolidayDetails holidayDetails)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation(" TaskController.AddHolidayDetails | AddHolidayDetails process start in the TaskController at {Time}", DateTime.UtcNow);
                var result = _dbServiceRepository.AddHolidayDetails(holidayDetails);
                _logger.LogInformation(" TaskController.AddHolidayDetails | AddHolidayDetails process complete in the TaskController at {Time}", DateTime.UtcNow);
                return Ok(result);

            }
            else
            {
                _logger.LogInformation(" TaskController.AddHolidayDetails | AddHolidayDetails process end with bad request in the TaskController at {Time}", DateTime.UtcNow);
                return BadRequest("Please enter the valid details");
            }
        }

        [HttpGet]
        [Route("GetAllHolidayDetails")]
        public IActionResult GetAllHolidayDetails()
        {
            _logger.LogInformation(" TaskController.GetAllHolidayDetails | GetAllHolidayDetails process start in the TaskController at {Time}", DateTime.UtcNow);
            var result = _dbServiceRepository.GetAllHolidayDetails();
            if (result != null)
            {
                _logger.LogInformation(" TaskController.GetAllHolidayDetails | GetAllHolidayDetails process complete in the TaskController at {Time}", DateTime.UtcNow);
                return StatusCode(200, result);
            }
            else
            {
                _logger.LogInformation(" TaskController.GetAllHolidayDetails | GetAllHolidayDetails process complte without data in the TaskController at {Time}", DateTime.UtcNow);
                return StatusCode(200, "Holiday Details not found");
            }
        }

    }

}
