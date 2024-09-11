using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using System.Threading.Tasks;


namespace MVC.PL.Controllers
{
    [Authorize]

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
        public async Task<IActionResult> Index(string searchInput)
        {
             IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModel> employeesVm;

            if (string.IsNullOrEmpty(searchInput))
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
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
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                employeeVM.ImageName = await DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Add(MappedEmployee);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                    TempData["Message"] = $"Employee is Added!";
                return RedirectToAction(nameof(Index));

            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();

            TempData["ImageName"] = MappedEmployee.ImageName;
            return View(viewName, MappedEmployee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVm)
        {
            if (employeeVm.Id != id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    string oldImage = TempData["ImageName"] as string;
                    DocumentSettings.DeleteFile("Images", oldImage);
                    employeeVm.ImageName = await DocumentSettings.UploadFile(employeeVm.Image, "Images");
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    var count = await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVm)
        {
            try
            {
                employeeVm.ImageName = TempData["ImageName"] as string;
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                var count = await _unitOfWork.CompleteAsync();
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
