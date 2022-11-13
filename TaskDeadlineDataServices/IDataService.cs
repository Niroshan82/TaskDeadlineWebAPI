using TaskDeadlineDataServices.Models;

namespace TaskDeadlineDataServices
{
    public interface IDataService
    {
        public ResponseDetails AddHolidayDetails(HolidayDetails holidayDetails);
        public List<HolidayDetails> GetAllHolidayDetails();
    }
}
