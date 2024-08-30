using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.CommonContracts;
using EmployeeManagement.CommonContracts.Services;
using EmployeeManagement.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class EmployeeControllerTests
    {
        private readonly EmployeeController _employeeController;
        private readonly Mock<IEmployeeService> _mockEmployeeService = new Mock<IEmployeeService>();

        public EmployeeControllerTests()
        {
            _employeeController = new EmployeeController(_mockEmployeeService.Object);
        }

        [Fact]
        public async Task TestGetEmployee_BadRequest()
        {
            //Arrange
            int employeeId = -1;
            EmployeeModel employee = new EmployeeModel();
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);

            //Act
            IActionResult response = await _employeeController.GetEmployee(employeeId);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task TestGetEmployee_NotFound()
        {
            //Arrange
            int employeeId = 1;
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync((EmployeeModel)null);

            //Act
            IActionResult response = await _employeeController.GetEmployee(employeeId);

            //Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task TestGetEmployee_OK()
        {
            //Arrange
            int employeeId = 1;
            var employee = new EmployeeModel
            {
                Id = 1,
                Name = "Sam",
                Designation = "Engineer",
                TotalYearsOfExperience = 5
            };
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);

            //Act
            IActionResult response = await _employeeController.GetEmployee(employeeId);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async Task TestGetEmployee_InternalServerError()
        {
            //Arrange
            int employeeId = 1;
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ThrowsAsync(new Exception("Server Error!"));

            //Act
            IActionResult response = await _employeeController.GetEmployee(employeeId);

            //Assert
            Assert.IsType<ObjectResult>(response);
        }

        [Fact]
        public async Task TestGetAllEmployees_OK()
        {
            //Arrange
            List<EmployeeModel> employee = new List<EmployeeModel>();
            _mockEmployeeService.Setup(x => x.GetAllEmployeesAsync()).ReturnsAsync(employee);

            //Act
            IActionResult response = await _employeeController.GetAllEmployees();

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async Task TestGetAllEmployees_InternalServerError()
        {
            //Arrange
            int employeeId = 1;
            _mockEmployeeService.Setup(x => x.GetAllEmployeesAsync()).ThrowsAsync(new Exception("Server Error!"));

            //Act
            IActionResult response = await _employeeController.GetAllEmployees();

            //Assert
            Assert.IsType<ObjectResult>(response);
        }

        [Fact]
        public async Task TestAddEmployee_BadRequest()
        {
            //Arrange
            EmployeeModel employee = new EmployeeModel
            {
                Name = "Sam",
                Designation = "Engineer",
                TotalYearsOfExperience = 100
            };
            _employeeController.ModelState.AddModelError("TotalYearsOfExperience", "Total experience should be between 1 and 50 years");

            //Act
            IActionResult response = await _employeeController.AddEmployee(employee);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task TestAddEmployee_Ok()
        {
            //Arrange
            EmployeeModel employee = new EmployeeModel
            {
                Name = "Sam",
                Designation = "Engineer",
                TotalYearsOfExperience = 100
            };
            _mockEmployeeService.Setup(x => x.AddEmployeeAsync(employee)).ReturnsAsync(employee);
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("https");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("localhost:44350"));
            request.Setup(x => x.Path).Returns(PathString.FromUriComponent("/api/Employee/GetEmployee"));
            var httpContext = Mock.Of<HttpContext>(x => x.Request == request.Object);
            _employeeController.ControllerContext.HttpContext = httpContext;

            //Act
            IActionResult response = await _employeeController.AddEmployee(employee);

            //Assert
            Assert.IsType<CreatedResult>(response);
        }

        [Fact]
        public async Task TestAddEmployee_InternalServerError()
        {
            //Arrange
            EmployeeModel employee = new EmployeeModel
            {
                Name = "Sam",
                Designation = "Engineer",
                TotalYearsOfExperience = 100
            };
            _mockEmployeeService.Setup(x => x.AddEmployeeAsync(employee)).ThrowsAsync(new Exception("Server Error!"));

            //Act
            IActionResult response = await _employeeController.AddEmployee(employee);

            //Assert
            Assert.IsType<ObjectResult>(response);
        }

        [Fact]
        public async Task TestDeleteEmployee_BadRequest()
        {
            //Arrange
            int employeeId = -1;
            EmployeeModel employee = new EmployeeModel();
            _mockEmployeeService.Setup(x => x.DeleteEmployeeAsync(employee));

            //Act
            IActionResult response = await _employeeController.DeleteEmployee(employeeId);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task TestDeleteEmployee_NotFound()
        {
            //Arrange
            int employeeId = 1;
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync((EmployeeModel)null);

            //Act
            IActionResult response = await _employeeController.DeleteEmployee(employeeId);

            //Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task TestDeleteEmployee_OK()
        {
            //Arrange
            int employeeId = 1;
            EmployeeModel employee = new EmployeeModel();
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);

            //Act
            IActionResult response = await _employeeController.DeleteEmployee(employeeId);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async Task TestDeleteEmployee_InternalServerError()
        {
            //Arrange
            int employeeId = 1;
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ThrowsAsync(new Exception("Server Error!"));

            //Act
            IActionResult response = await _employeeController.DeleteEmployee(employeeId);

            //Assert
            Assert.IsType<ObjectResult>(response);
        }

        [Fact]
        public async Task TestEditEmployee_BadRequestForModelValidation()
        {
            //Arrange
            int employeeId = 1;
            EmployeeModel employee = new EmployeeModel
            {
                Name = "Sam",
                Designation = "Engineer",
                TotalYearsOfExperience = 100
            };
            _employeeController.ModelState.AddModelError("TotalYearsOfExperience", "Total experience should be between 1 and 50 years");

            //Act
            IActionResult response = await _employeeController.EditEmployee(employeeId, employee);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task TestEditEmployee_BadRequestForEmployeeIdValidation()
        {
            //Arrange
            int employeeId = -1;
            EmployeeModel employee = new EmployeeModel();
            _mockEmployeeService.Setup(x => x.EditEmployeeAsync(employee));

            //Act
            IActionResult response = await _employeeController.EditEmployee(employeeId, employee);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact]
        public async Task TestEditEmployee_NotFound()
        {
            //Arrange
            int employeeId = 1;
            EmployeeModel employee = new EmployeeModel();
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync((EmployeeModel)null);

            //Act
            IActionResult response = await _employeeController.EditEmployee(employeeId, employee);

            //Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task TestEditEmployee_OK()
        {
            //Arrange
            int employeeId = 1;
            EmployeeModel employee = new EmployeeModel();
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);
            _mockEmployeeService.Setup(x => x.EditEmployeeAsync(employee)).ReturnsAsync(employee);

            //Act
            IActionResult response = await _employeeController.EditEmployee(employeeId, employee);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async Task TestEditEmployee_InternalServerError()
        {
            //Arrange
            int employeeId = 1;
            EmployeeModel employee = new EmployeeModel();
            _mockEmployeeService.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ThrowsAsync(new Exception("Server Error!"));

            //Act
            IActionResult response = await _employeeController.EditEmployee(employeeId, employee);

            //Assert
            Assert.IsType<ObjectResult>(response);
        }
    }
}
