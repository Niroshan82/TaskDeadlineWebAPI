using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TaskDeadlineBusinessLogic;
using TaskDeadlineDataServices;
using TaskDeadlineDataServices.Models;

namespace TaskDeadlineUnitTest
{
    public class TaskLogicTest
    {
        private readonly Mock<ILogger<TaskLogic>> _iLoggerMock;
        private readonly TaskLogic _taskLogic;
        private readonly IFixture _fixture;
        private readonly Mock<IDataService> _iDataServiceMock;
       
        public TaskLogicTest()
        {
            _iLoggerMock = new Mock<ILogger<TaskLogic>>();
            _fixture = new Fixture();
            _iDataServiceMock = _fixture.Freeze<Mock<IDataService>>();
            _taskLogic = new TaskLogic(_iLoggerMock.Object, _iDataServiceMock.Object);
        }

        [Fact]
        public void CalculateTaskEndDate_ShouldReturn200_WhenCorrectDataFound()
        {
            //Arrange
            DateTime taskStartDate = DateTime.ParseExact("2022-11-09 00:00:00,000", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime holidayDate = DateTime.ParseExact("2022-11-10 00:00:00,000", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture);


            double workingDays = 5;
            List<HolidayDetails> holidayDetailsList = new List<HolidayDetails>() { new HolidayDetails { holidayID = 3, holidayName = "Mock",holidayDate= holidayDate } };
            _iDataServiceMock.Setup(x => x.GetAllHolidayDetails()).Returns(holidayDetailsList);


            //Act
            var result = _taskLogic.CalculateTaskEndDate(taskStartDate, workingDays);

            //Assert
            _iDataServiceMock.Verify(x => x.GetAllHolidayDetails(), Times.Once);
            result.Should().NotBeNull();
            result.statusCode.Should().Be(200);
        }

    }
}
