using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.BLL.Interfaces;
using MVC.DAL.Models;
using MVC.PL.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MVC.PL.Controllers
{
    // Inheritance => DepartmentController is a Controller
    // Composition => DepartmentController has an object that implements IDepartmentRepository interface
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentRepository, IWebHostEnvironment env, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _env = env;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var department = _departmentRepository.GetAll();
            var MappedDepartment = _mapper.Map<IEnumerable<Department> ,IEnumerable<DepartmentViewModel> >(department);
            return View(MappedDepartment);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVm)
        {
            if(ModelState.IsValid) // Servere Side Validation
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                var Count = _departmentRepository.Add(MappedDepartment);
                if(Count > 0) 
                    return RedirectToAction(nameof(Index));
                
            }
            return View(departmentVm);
        }

        public IActionResult Details(int? id,string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = _departmentRepository.GetById(id.Value);
            var MappedDepartment = _mapper.Map<Department,DepartmentViewModel>(department);
            if(department is null)
                return NotFound();
            return View(viewName, MappedDepartment);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id,DepartmentViewModel departmentVm)
        {
            if(departmentVm.Id != id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel,Department>(departmentVm);
                    _departmentRepository.Update(MappedDepartment);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        ModelState.AddModelError(string.Empty, "There is Error Occurred During Updating the Department");

                    return View(departmentVm);

                }
            }

            return View(departmentVm);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete"); 
        }
        [HttpPost]
        public IActionResult Delete(DepartmentViewModel departmentVm)
        {
            try
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                _departmentRepository.Delete(MappedDepartment);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "There is Error Occurred During Updating the Department");

                return View(departmentVm);
            }
        }
    }
}
