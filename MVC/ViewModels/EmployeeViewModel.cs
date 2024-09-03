using MVC.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System;
using Microsoft.AspNetCore.Http;

namespace MVC.PL.Models
{
    public enum Gender
    {
        [EnumMember(Value = "Male")]
        Male = 1,
        [EnumMember(Value = "Female")]
        Female = 2
    }
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Max Length for Name is 50 Chars")]
        [MinLength(5, ErrorMessage = "Max Length for Name is 50 Chars")]
        public string Name { get; set; }
        [Range(18, 60)]
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

        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        [InverseProperty("Employees")]
        public Department Department { get; set; }

        public IFormFile Image { get; set; }

        public string ImageName { get; set; }

    }
}

