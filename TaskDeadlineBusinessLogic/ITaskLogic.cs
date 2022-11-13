using TaskDeadlineDataServices.Models;

namespace TaskDeadlineBusinessLogic
{
    public interface ITaskLogic
    {
        public ResponseDetails CalculateTaskEndDate(DateTime taskstartDate, double workingDays);
    }

}
