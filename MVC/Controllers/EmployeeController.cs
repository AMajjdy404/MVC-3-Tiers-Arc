using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.BLL.Interfaces;
using MVC.DAL.Models;
using MVC.PL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static MVC.PL.Models.EmployeeViewModel;
using Employee = MVC.DAL.Models.Employee;

namespace MVC.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IWebHostEnvironment env,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _env = env;
            _mapper = mapper;
        }
        public IActionResult Index(string searchInput)
        {
             IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModel> employeesVm;

            if (string.IsNullOrEmpty(searchInput))
                employees = _employeeRepository.GetAll();
            else
                employees = _employeeRepository.GetEmployeeByName(searchInput);

            employeesVm = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(employeesVm);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                var Count = _employeeRepository.Add(MappedEmployee);
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
            var employee = _employeeRepository.GetById(id.Value);
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();
            return View(viewName, MappedEmployee);
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.Departments = _departmentRepository.GetAll();
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
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                    var count = _employeeRepository.Update(MappedEmployee);
                    if (count > 0)
                        TempData["Message"] = $"Employee is Updated!";
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
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                var count = _employeeRepository.Delete(MappedEmployee);
                if (count > 0)
                    TempData["Message"] = $"Employee is Deleted!";
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
