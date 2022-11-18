using EmployeeManagement1.Models;
using EmployeeManagement1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement1.Controllers
{

   
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private IWebHostEnvironment _hostEnvironment;
        private readonly ILogger logger;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnviroment,
            ILogger<HomeController> logger)
        {
          
            _employeeRepository = employeeRepository;
            _hostEnvironment = hostingEnviroment;
            this.logger = logger;
        }

         //[Route("")]
         //[Route("/")]
         //[Route("Index")]
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View("../Home/Index",model);
        }

        //[Route("Details/{id?}")]
        public ViewResult Details(int? id)
        {
            logger.LogTrace("Trace log");
            logger.LogDebug("Debug log");
            logger.LogInformation("Information log");
            logger.LogWarning("warning log");
            logger.LogError("Error log");
            logger.LogCritical("Critical log");

            Employee model = _employeeRepository.GetEmployee(id??1);

            if(model==null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            ViewData["Employee"] = model;
            ViewData["PageTitle"] = "Employee Details";

            ViewBag.Employee = model;

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel
            {
                Employee = model,
                PageTitle = "Employee Details"
            };
            return View(homeDetailsViewModel);

        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath,
                photoSelect="Click here to choose file"

            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                //if(model.Photo!=null )
                //{
                    //foreach (IFormFile photo in model.Photos)
                    //{
                    //string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    //uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    //string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    //model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    //}
                //}
                Employee newEmployee = new Employee()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                    
                    
                    
                    _employeeRepository.AddEmployee(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }

            return View();
          
        }
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel employeeEditViewModel)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(employeeEditViewModel.Id);
                employee.Name = employeeEditViewModel.Name;
                employee.Email = employeeEditViewModel.Email;
                employee.Department = employeeEditViewModel.Department;
                if (employeeEditViewModel.Photo != null)
                {
                    if(employeeEditViewModel.ExistingPhotoPath!=null)
                    {
                        string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", employeeEditViewModel.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(employeeEditViewModel);
                }
               


                _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }

            return View(employeeEditViewModel);
            
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null )
            {
               
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                   
                   using(var fileStream= new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
                    
                
            }

            return uniqueFileName;
        }
    }

}
