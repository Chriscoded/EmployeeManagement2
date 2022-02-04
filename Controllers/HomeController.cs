using EmployeeManagement2.Models;
using EmployeeManagement2.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement2.Controllers
{

    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ILogger logger;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment, ILogger<HomeController> logger)
        { 
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.logger = logger;
              
        }

        public ViewResult index()
        {
            //return _employeeRepository.GetEmployee(1).Name;
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }
        public ViewResult Details(int? id)
        {
            //throw new Exception("Error in detail view");

            logger.LogTrace("Log Trace");
            logger.LogDebug("Log Debug");
            logger.LogInformation("Log Information");
            logger.LogWarning("Log Warning");
            logger.LogError("Log Error");
            logger.LogCritical("Log critical");
            //get the particular employee using id
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            else
            {
                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
                {

                    Employee = employee,
                    PageTitle = "Employee Details"
                };
                //Employee model = _employeeRepository.GetEmployee(3);
                //ViewBag.PageTitle = "Employee Details";

                return View(homeDetailsViewModel);
            }
        }

        [HttpGet]
        [Authorize]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                uniqueFileName = ProcessUploadedFiles(model, uniqueFileName);
                Employee newEmployee = new Employee()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ViewResult Edit(int id)
        {
            Employee Employee = _employeeRepository.GetEmployee(id);

            EmployeeEditViewModel editViewModel = new EmployeeEditViewModel()
            {
                Id = Employee.Id,
                Name = Employee.Name,
                Email = Employee.Email,
                Department = Employee.Department,
                ExistingPhotoPath = Employee.PhotoPath
            };
            return View(editViewModel);

        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(EmployeeEditViewModel model)
        {


            //if (ModelState.IsValid)
            //{
            Employee Employee = _employeeRepository.GetEmployee(model.Id);
            Employee.Name = model.Name;
            Employee.Email = model.Email;
            Employee.Department = model.Department;
            string uniqueFileName = null;

            if (model.Photos != null)
            {
                if (model.ExistingPhotoPath != null)
                {
                    //lets get the path of existing photopath for deleting
                    var filePath = Path.Combine(hostingEnvironment.WebRootPath,
                        "images", model.ExistingPhotoPath);
                    System.IO.File.Delete(filePath);

                }
                uniqueFileName = ProcessUploadedFiles(model, uniqueFileName);
                Employee.PhotoPath = uniqueFileName;
            }

            _employeeRepository.Update(Employee);
            return RedirectToAction("index");
            //}
            //return View();
        }

        private string ProcessUploadedFiles(EmployeeCreateViewModel model, string uniqueFileName)
        {
            if (model.Photos != null)
            {
                String UploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                //create a unique file name for our image using Guid each NewGuid() function is unique
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photos.FileName;
                //lets combine image path with image name their by getting us the image base 
                String filePath = Path.Combine(UploadsFolder, uniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photos.CopyTo(filestream);
                }
            }

            return uniqueFileName;
        }


    }
}
