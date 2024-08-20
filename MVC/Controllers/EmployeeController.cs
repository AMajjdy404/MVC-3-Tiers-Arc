using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.BLL.Interfaces;
using MVC.DAL.Models;
using System;

namespace MVC.PL.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IGenericRepository<Employee> _employeeRepository;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IGenericRepository<Employee> employeeRepository, IWebHostEnvironment env)
        {
            _employeeRepository = employeeRepository;
            _env = env;
        }
        public IActionResult Index()
        {
            var employee = _employeeRepository.GetAll();
            return View(employee);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid) // Servere Side Validation
            {
                var Count = _employeeRepository.Add(employee);
                if (Count > 0)
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
                    _employeeRepository.Update(employee);
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
                _employeeRepository.Delete(employee);
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

