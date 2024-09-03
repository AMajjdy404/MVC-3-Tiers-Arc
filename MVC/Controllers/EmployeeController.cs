using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.BLL.Interfaces;
using MVC.BLL.Repositories;
using MVC.DAL.Models;
using MVC.PL.Helpers;
using MVC.PL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace MVC.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork
            /*IEmployeeRepository employeeRepository*/
            /*IDepartmentRepository departmentRepository*/,
            IWebHostEnvironment env,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _mapper = mapper;
        }
        public IActionResult Index(string searchInput)
        {
             IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModel> employeesVm;

            if (string.IsNullOrEmpty(searchInput))
                employees = _unitOfWork.EmployeeRepository.GetAll();
            else
                employees = _unitOfWork.EmployeeRepository.GetEmployeeByName(searchInput);

            employeesVm = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(employeesVm);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Add(MappedEmployee);
                var Count = _unitOfWork.Complete();
                if (Count > 0)
                    TempData["Message"] = $"Employee is Added!";
                return RedirectToAction(nameof(Index));

            }
            return View(employeeVM);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = _unitOfWork.EmployeeRepository.GetById(id.Value);
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();

            TempData["ImageName"] = MappedEmployee.ImageName;
            return View(viewName, MappedEmployee);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVm)
        {
            if (employeeVm.Id != id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    string oldImage = TempData["ImageName"] as string;
                    DocumentSettings.DeleteFile("Images", oldImage);
                    employeeVm.ImageName = DocumentSettings.UploadFile(employeeVm.Image, "Images");
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    var count = _unitOfWork.Complete();
                    if (count > 0)
                        TempData["Message"] = "Employee is Updated!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        ModelState.AddModelError(string.Empty, "There is Error Occurred During Updating the Employee");


                    return View(employeeVm);
                }

            }
            return View(employeeVm);

        }
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVm)
        {
            try
            {
                employeeVm.ImageName = TempData["ImageName"] as string;
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    DocumentSettings.DeleteFile("Images", MappedEmployee.ImageName);
                    TempData["Message"] = $"Employee is Deleted!";
                }

                    
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "There is Error Occurred During Deleting this Employee");

                return View(employeeVm);
            }
        }
    }

}
