using AutoMapper;
using MVC.DAL.Models;
using MVC.PL.ViewModels;

namespace MVC.PL.MappingProfiles
{
    public class DepartmentProfile: Profile
    {
        public DepartmentProfile()
        {
            
            CreateMap<Department,DepartmentViewModel>().ReverseMap();
        }
    }
}
