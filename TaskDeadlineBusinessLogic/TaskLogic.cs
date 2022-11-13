using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using TaskDeadlineDataServices;
using TaskDeadlineDataServices.Models;
using HolidayDetails = TaskDeadlineDataServices.Models.HolidayDetails;

namespace TaskDeadlineBusinessLogic
{
    public class TaskLogic : ITaskLogic
    {
        private readonly ILogger<TaskLogic> _logger;
        private readonly IDataService _dataService;
        public TaskLogic(ILogger<TaskLogic> logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }
        public ResponseDetails CalculateTaskEndDate(DateTime taskStartDate, double workingDays)
        {
            _logger.LogInformation(" TaskLogic.CalculateTaskEndDate | CalculateTaskEndDate process start in the TaskLogic at {Time}", DateTime.UtcNow);
            ResponseDetails responseDetails = new ResponseDetails();

            //get a list of public holiday details
            List<HolidayDetails> holidayDetailsList = _dataService.GetAllHolidayDetails();

            DateTime startDate = taskStartDate;
            for (int i = 1; i < workingDays;)
            {
                startDate = startDate.AddDays(1);
                if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday || holidayDetailsList.Any(x => x.holidayDate.Equals(startDate.Date)))
                {
                    continue;
                }
                i++;
            }
            var endDate = startDate;
            string? endDateValue = endDate.ToString("yyyy-M-dd", CultureInfo.InvariantCulture);
            responseDetails.statusMessage = endDateValue + " is the end date of this task.";
            responseDetails.statusCode = 200;
            _logger.LogInformation(" TaskLogic.CalculateTaskEndDate | CalculateTaskEndDate process completed in the TaskLogic at {Time}", DateTime.UtcNow);
            return responseDetails;
        }
    }


}
