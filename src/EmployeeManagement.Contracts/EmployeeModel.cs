using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.CommonContracts
{
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }

        [Required()]
        [MinLength(2, ErrorMessage = "Name should be atleast 2 characters")]
        public string Name { get; set; }

        [Required()]
        [MaxLength(50, ErrorMessage = "Designation should not exceed 50 characters")]
        public string Designation { get; set; }

        [Required()]
        [Range(1,50, ErrorMessage = "Total experience should be between 1 and 50 years")]
        public int TotalYearsOfExperience { get; set; }
    }
}
