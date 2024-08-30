using EmployeeManagement.CommonContracts;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.DataAccess
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }

        public DbSet<EmployeeModel> Employees { get; set; }
    }
}
