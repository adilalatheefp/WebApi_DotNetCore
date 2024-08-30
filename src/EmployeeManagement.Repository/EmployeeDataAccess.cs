using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.CommonContracts;
using EmployeeManagement.CommonContracts.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DataAccess
{
    public class EmployeeDataAccess : IEmployeeDataAccess
    {
        private readonly EmployeeDbContext _dbContext;

        public EmployeeDataAccess(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EmployeeModel> AddEmployeeAsync(EmployeeModel employee)
        {
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            return employee;
        }

        public async Task DeleteEmployeeAsync(EmployeeModel employee)
        {
            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<EmployeeModel> EditEmployeeAsync(EmployeeModel employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();

            return employee;
        }

        public async Task<IReadOnlyList<EmployeeModel>> GetAllEmployeesAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<EmployeeModel> GetEmployeeByIdAsync(int employeeId)
        {
            return await _dbContext.Employees.FindAsync(employeeId);
        }
    }
}
