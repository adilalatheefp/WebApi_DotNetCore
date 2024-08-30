using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.CommonContracts.DataAccess
{
    public interface IEmployeeDataAccess
    {
        public Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employee);

        public Task DeleteEmployeeAsync(EmployeeModel employee);

        public Task<EmployeeModel> EditEmployeeAsync(EmployeeModel employee);

        public Task<EmployeeModel> GetEmployeeByIdAsync(int employeeId);

        public Task<IReadOnlyList<EmployeeModel>> GetAllEmployeesAsync();
    }
}
