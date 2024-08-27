using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DAL.Models
{
    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male =1,
        [EnumMember(Value = "Female")]
        Female = 2
    }
    public enum EmpType
    {
        [EnumMember(Value ="Full Time")]
        FullTime =1,
        [EnumMember(Value = "Part Time")]
        PartTime = 2
    }
    public class Employee : BaseEntity
    {
        [Required]
        [MaxLength(50, ErrorMessage ="Max Length for Name is 50 Chars")]
        [MinLength(5, ErrorMessage = "Max Length for Name is 50 Chars")]
        public string Name { get; set; }
        [Range(18,60)]
        public int Age { get; set; }
        [RegularExpression(@"^\d+-[A-Za-z0-9\s]+-[A-Za-z\s]+-[A-Za-z\s]+$"
         , ErrorMessage = "Address Must be like this 123-Main Street-Cairo-Egypt")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public EmpType EmployeeType { get; set; }

        [Display(Name ="Hiring Date")]
        public DateTime HiringDate { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        [InverseProperty("Employees")]
        public Department Department { get; set; }

    }
}
