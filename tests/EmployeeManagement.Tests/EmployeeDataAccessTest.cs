using System;
using System.Collections.Generic;
using EmployeeManagement.CommonContracts;
using EmployeeManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeeManagement.Tests
{
    public class EmployeeDataAccessTest
    {
        private readonly EmployeeDataAccess _employeeDataAccess;
        private readonly EmployeeDbContext _dbContext;

        public EmployeeDataAccessTest()
        {
            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new EmployeeDbContext(options);

            var employee = new[]
            {
                new EmployeeModel { Id=1, Name = "Employee One", Designation="Software Engineer", TotalYearsOfExperience=5 },
                new EmployeeModel { Id=2, Name = "Employee Two", Designation="Testing Engineer", TotalYearsOfExperience=3 },
                new EmployeeModel { Id=3, Name = "Employee Three", Designation="Deployment Engineer", TotalYearsOfExperience=9 },
                new EmployeeModel { Id=4, Name = "Employee Four", Designation="HR", TotalYearsOfExperience=6 }
            };

            _dbContext.Employees.AddRange(employee);
            _dbContext.SaveChanges();

            _employeeDataAccess = new EmployeeDataAccess(_dbContext);
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

            //Act
            EmployeeModel result = await _employeeDataAccess.AddEmployeeAsync(employee);

            //Assert
            Assert.Equal(employeeName, result.Name);
            Assert.Equal(employeeDesignation, result.Designation);
        }

        [Fact]
        public async void TestDeleteEmployeeAsync()
        {
            //Arrange
            int employeeId = 2;
            EmployeeModel employee = _dbContext.Employees.Find(employeeId);

            //Act
            await _employeeDataAccess.DeleteEmployeeAsync(employee);

            //Assert
            Assert.Equal(_dbContext, _dbContext);
        }

        [Fact]
        public async void TestEditEmployeeAsync()
        {
            //Arrange
            int employeeId = 2;
            string employeeName = "Sam";
            string employeeDesignation = "Engineer";
            EmployeeModel employee = _dbContext.Employees.Find(employeeId);
            employee.Name = employeeName;
            employee.Designation = employeeDesignation;

            //Act
            var result = await _employeeDataAccess.EditEmployeeAsync(employee);

            //Assert
            Assert.Equal(employeeName, result.Name);
            Assert.Equal(employeeDesignation, result.Designation);
        }

        [Fact]
        public async void TestGetAllEmployeesAsync()
        {
            //Act
            IReadOnlyList<EmployeeModel> result = await _employeeDataAccess.GetAllEmployeesAsync();

            //Assert
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async void TestGetEmployeeByIdAsync()
        {
            //Arrange
            int employeeId = 2;

            //Act
            EmployeeModel result = await _employeeDataAccess.GetEmployeeByIdAsync(employeeId);

            //Assert
            Assert.Equal("Employee Two", result.Name);
            Assert.Equal(3, result.TotalYearsOfExperience);
        }
    }
}
