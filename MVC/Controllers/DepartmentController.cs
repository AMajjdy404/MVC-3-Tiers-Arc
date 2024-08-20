using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MVC.BLL.Interfaces;
using MVC.DAL.Models;
using System;

namespace MVC.PL.Controllers
{
    // Inheritance => DepartmentController is a Controller
    // Composition => DepartmentController has an object that implements IDepartmentRepository interface
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IDepartmentRepository departmentRepository, IWebHostEnvironment env)
        {
            _departmentRepository = departmentRepository;
            _env = env;
        }
        public IActionResult Index()
        {
            var department = _departmentRepository.GetAll();
            return View(department);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if(ModelState.IsValid) // Servere Side Validation
            {
                var Count = _departmentRepository.Add(department);
                if(Count > 0) 
                    return RedirectToAction(nameof(Index));
                
            }
            return View(department);
        }

        public IActionResult Details(int? id,string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = _departmentRepository.GetById(id.Value);
            if(department is null)
                return NotFound();
            return View(viewName,department);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id,Department department)
        {
            if(department.Id != id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    _departmentRepository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    if (_env.IsDevelopment())
                        ModelState.AddModelError(string.Empty, ex.Message);
                    else
                        ModelState.AddModelError(string.Empty, "There is Error Occurred During Updating the Department");

                    return View(department);

                }
            }

            return View(department);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete"); 
        }
        [HttpPost]
        public IActionResult Delete(Department department)
        {
            try
            {
                _departmentRepository.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                if (_env.IsDevelopment())
                    ModelState.AddModelError(string.Empty, ex.Message);
                else
                    ModelState.AddModelError(string.Empty, "There is Error Occurred During Updating the Department");

                return View(department);
            }
        }
    }
}
