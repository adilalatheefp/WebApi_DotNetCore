using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.CommonContracts;
using EmployeeManagement.CommonContracts.DataAccess;
using EmployeeManagement.CommonContracts.Services;

namespace EmployeeManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDataAccess _repository;

        public EmployeeService(IEmployeeDataAccess repository)
        {
            _repository = repository;
        }

        public async Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employee)
        {
            return await _repository.AddEmployeeAsync(employee);
        }

        public async Task DeleteEmployeeAsync(EmployeeModel employee)
        {
            await _repository.DeleteEmployeeAsync(employee);
        }

        public async Task<EmployeeModel> EditEmployeeAsync(EmployeeModel employee)
        {
            return await _repository.EditEmployeeAsync(employee);
        }

        public async Task<IReadOnlyList<EmployeeModel>> GetAllEmployeesAsync()
        {
            return await _repository.GetAllEmployeesAsync();
        }

        public async Task<EmployeeModel> GetEmployeeByIdAsync(int employeeId)
        {
            return await _repository.GetEmployeeByIdAsync(employeeId);
        }
    }
}
