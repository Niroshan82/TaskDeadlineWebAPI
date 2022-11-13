using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TaskDeadlineBusinessLogic;
using TaskDeadlineDataServices;
using TaskDeadlineDataServices.Models;
using TaskDeadlineWebAPI.Controllers;

namespace TaskDeadlineUnitTest
{
    public class TaskControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITaskLogic> _taskLogic;
        private readonly Mock<ILogger<TaskController>> _iLoggerMock;
        private readonly Mock<IDataService> _iDataServiceMock;
        private readonly Mock<IConfiguration> _iConfigurationMock;
        private readonly TaskController _taskController;

        public TaskControllerTest()
        {
            _fixture = new Fixture();
            _taskLogic = _fixture.Freeze<Mock<ITaskLogic>>();
            _iLoggerMock = new Mock<ILogger<TaskController>>();
            _iDataServiceMock = _fixture.Freeze<Mock<IDataService>>();
            _iConfigurationMock = new Mock<IConfiguration>();
            _taskController = new TaskController(_taskLogic.Object, _iLoggerMock.Object, _iDataServiceMock.Object, _iConfigurationMock.Object);
        }

        [Fact]
        public void CalculateTaskEndDate_CheckPositiveSenario()
        {
            //Arrange
            DateTime taskStartDate = DateTime.Now;
            double workingDays = 5;
            ResponseDetails responseDetails = new ResponseDetails();
            responseDetails.statusCode = 200;
            _taskLogic.Setup(x => x.CalculateTaskEndDate(taskStartDate, workingDays)).Returns(responseDetails);

            //Act
            var result = _taskController.CalculateTaskEndDate(taskStartDate, workingDays);

            //Assert
            _taskLogic.Verify(x => x.CalculateTaskEndDate(taskStartDate, workingDays), Times.Once);
            result.Should().NotBeNull();  //if null, fail

        }

        [Fact]
        public void CalculateTaskEndDate_CheckBadRequest()
        {
            //Arrange
            DateTime taskStartDate = DateTime.Now;
            double workingDays = -1;

            //Act
            var result = _taskController.CalculateTaskEndDate(taskStartDate, workingDays);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void AddHolidayDetails_CheckPositiveSenario()
        {
            //Arrange
            HolidayDetails holidayDetails = new HolidayDetails();
            ResponseDetails responseDetails = new ResponseDetails();
            responseDetails.statusCode = 200;
            _iDataServiceMock.Setup(x => x.AddHolidayDetails(holidayDetails)).Returns(responseDetails);

            //Act
            var result = _taskController.AddHolidayDetails(holidayDetails);

            //Assert
            _iDataServiceMock.Verify(x => x.AddHolidayDetails(holidayDetails), Times.Once);
            result.Should().NotBeNull();  //if null, fail

        }

        [Fact]
        public void AddHolidayDetails_CheckModelState_False()
        {
            //Arrange
            HolidayDetails holidayDetails = new HolidayDetails();
            _taskController.ModelState.AddModelError("key", "error message");

            //Act
            var result = _taskController.AddHolidayDetails(holidayDetails);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetAllHolidayDetails_CheckPositiveSenario()
        {
            //Arrange
            HolidayDetails holidayDetails = new HolidayDetails();
            List<HolidayDetails> holidayDetailsList = new List<HolidayDetails>();
            _iDataServiceMock.Setup(x => x.GetAllHolidayDetails()).Returns(holidayDetailsList);

            //Act
            var result = _taskController.GetAllHolidayDetails();

            //Assert
            _iDataServiceMock.Verify(x => x.GetAllHolidayDetails(), Times.Once);
            result.Should().NotBeNull();  //if null, fail
        }
        [Fact]
        public void GetAllHolidayDetails_CheckNegativeSenario()
        {
            //Arrange
            List<HolidayDetails>? holidayDetailsListValue = null;

            _iDataServiceMock.Setup(x => x.GetAllHolidayDetails()).Returns(holidayDetailsListValue);

            //Act
            var result = _taskController.GetAllHolidayDetails();

            //Assert
            _iDataServiceMock.Verify(x => x.GetAllHolidayDetails(), Times.Once);

        }

    }
}
