using AutoMapper;
using MVC.DAL.Models;
using MVC.PL.Models;

namespace MVC.PL.MappingProfiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            
            CreateMap<Employee,EmployeeViewModel>().ReverseMap();
        }
    }
}
