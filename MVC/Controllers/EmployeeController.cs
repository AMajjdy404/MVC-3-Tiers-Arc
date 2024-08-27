using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.BLL.Interfaces;
using MVC.DAL.Models;
using System;
using System.Linq;

namespace MVC.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository ,IWebHostEnvironment env)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _env = env;
        }
        public IActionResult Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(searchInput))
                employees = _employeeRepository.GetAll();
            else
                employees = _employeeRepository.GetEmployeeByName(searchInput);

            return View(employees);
        }

        public IActionResult Create()
        {
            ViewBag.Departments = _departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid) // Servere Side Validation
            {
                var Count = _employeeRepository.Add(employee);
                if (Count > 0)
                    TempData["Message"] = $"Employee is Added!";
                    return RedirectToAction(nameof(Index));


            }
            return View(employee);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null)
                return NotFound();
            return View(viewName, employee);
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.Departments = _departmentRepository.GetAll();
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {
            if (employee.Id != id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                   var count = _employeeRepository.Update(employee);
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

                    return View(employee);

                }
            }

            return View(employee);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(Employee employee)
        {
            try
            {
                var count = _employeeRepository.Delete(employee);
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

                return View(employee);
            }
        }
    }
}

