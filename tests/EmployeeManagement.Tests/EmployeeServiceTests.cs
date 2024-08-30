using EmployeeManagement.CommonContracts;
using EmployeeManagement.CommonContracts.DataAccess;
using EmployeeManagement.Services;
using Moq;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class EmployeeServiceTests
    {
        private readonly EmployeeService _employeeService;
        private readonly Mock<IEmployeeDataAccess> _mockEmployeeDataAccess = new Mock<IEmployeeDataAccess>();

        public EmployeeServiceTests()
        {
            _employeeService = new EmployeeService(_mockEmployeeDataAccess.Object);
        }

        [Fact]
        public async void TestAddEmployeeAsync()
        {
            //Arrange
            string employeeName = "Sam";
            string employeeDesignation = "Engineer";
            EmployeeModel employee = new EmployeeModel
            {
                Name = "Sam",
                Designation = "Engineer",
                TotalYearsOfExperience = 10
            };
            _mockEmployeeDataAccess.Setup(x => x.AddEmployeeAsync(employee)).ReturnsAsync(employee);

            //Act
            EmployeeModel result = await _employeeService.AddEmployeeAsync(employee);

            //Assert
            Assert.Equal(employeeName, result.Name);
            Assert.Equal(employeeDesignation, result.Designation);
        }

        [Fact]
        public async void TestDeleteEmployeeAsync()
        {
            //Arrange
            EmployeeModel employee = new EmployeeModel();
            _mockEmployeeDataAccess.Setup(x => x.DeleteEmployeeAsync(employee));

            //Act
            await _employeeService.DeleteEmployeeAsync(employee);

            //Assert
            _mockEmployeeDataAccess.Verify(x => x.DeleteEmployeeAsync(employee), Times.AtLeastOnce());
        }

        [Fact]
        public async void TestEditEmployeeAsync()
        {
            //Arrange
            string employeeName = "Sam";
            string employeeDesignation = "Engineer";
            int employeeExperience = 5;
            EmployeeModel employee = new EmployeeModel
            {
                Name = "Sam",
                Designation = "Engineer",
                TotalYearsOfExperience = 5
            };
            _mockEmployeeDataAccess.Setup(x => x.EditEmployeeAsync(employee)).ReturnsAsync(employee);

            //Act
            EmployeeModel result = await _employeeService.EditEmployeeAsync(employee);

            //Assert
            Assert.Equal(employeeName, result.Name);
            Assert.Equal(employeeDesignation, result.Designation);
            Assert.Equal(employeeExperience, result.TotalYearsOfExperience);
        }

        [Fact]
        public async void TestGetAllEmployeesAsync()
        {
            //Arrange
            _mockEmployeeDataAccess.Setup(x => x.GetAllEmployeesAsync());

            //Act
            await _employeeService.GetAllEmployeesAsync();

            //Assert
            _mockEmployeeDataAccess.Verify(x => x.GetAllEmployeesAsync(), Times.AtLeastOnce());
        }

        [Fact]
        public async void TestGetEmployeeByIdAsync()
        {
            //Arrange
            int employeeId = 1;
            string employeeName = "Sam";
            EmployeeModel employee = new EmployeeModel
            {
                Id = 1,
                Name = "Sam",
                Designation = "Engineer",
                TotalYearsOfExperience = 10
            };
            _mockEmployeeDataAccess.Setup(x => x.GetEmployeeByIdAsync(employeeId)).ReturnsAsync(employee);

            //Act
            EmployeeModel result = await _employeeService.GetEmployeeByIdAsync(employeeId);

            //Assert
            Assert.Equal(employeeId, result.Id);
            Assert.Equal(employeeName, result.Name);
        }
    }
}
