using System;
using System.Threading.Tasks;
using EmployeeManagement.CommonContracts;
using EmployeeManagement.CommonContracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployee(int employeeId)
        {
            if (employeeId <= 0)
            {
                return BadRequest("Employee Id cannot be '0' or less.");
            }

            try
            {
                EmployeeModel employee = await _service.GetEmployeeByIdAsync(employeeId);

                if (employee == null)
                {
                    return NotFound($"Employee with id '{employeeId}' was not found!");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                return Ok(await _service.GetAllEmployeesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeModel employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.AddEmployeeAsync(employee);

                return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path.ToString().Replace(nameof(AddEmployee), nameof(GetEmployee))}/{employee.Id}",
                               "New employee record successfully created.");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            if (employeeId <= 0)
            {
                return BadRequest("Employee Id cannot be '0' or less.");
            }

            try
            {
                EmployeeModel employee = await _service.GetEmployeeByIdAsync(employeeId);

                if (employee == null)
                {
                    return NotFound($"Employee with id '{employeeId}' was not found!");
                }

                await _service.DeleteEmployeeAsync(employee);

                // return NoContent() cannot include confirmation message.
                return Ok("Employee record successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> EditEmployee(int employeeId, [FromBody] EmployeeModel employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (employeeId <= 0)
            {
                return BadRequest("Employee Id cannot be '0' or less.");
            }

            try
            {
                EmployeeModel existingEmployee = await _service.GetEmployeeByIdAsync(employeeId);

                if (existingEmployee == null)
                {
                    return NotFound($"Employee with id '{employeeId}' was not found !");
                }

                existingEmployee.Name = employee.Name;
                existingEmployee.Designation = employee.Designation;
                existingEmployee.TotalYearsOfExperience = employee.TotalYearsOfExperience;

                await _service.EditEmployeeAsync(existingEmployee);

                return Ok("Employee record successfully updated.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
