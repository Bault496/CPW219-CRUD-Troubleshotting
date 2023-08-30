using CPW219_CRUD_Troubleshooting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;

namespace CPW219_CRUD_Troubleshooting.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext context;

        public StudentsController(SchoolContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Student> products = StudentDb.GetStudents(context);
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student studentCreate)
        {
            if (ModelState.IsValid)
            {
                StudentDb.Add(studentCreate, context);
                await context.SaveChangesAsync();
                ViewData["Message"] = $"{studentCreate.Name} was added!";
                return View();
            }

            //Show web page with errors
            return View(studentCreate);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //get the product by id
            Student studentEdit = StudentDb.GetStudent(context, id);

            if (studentEdit == null)
            {
                return NotFound();
            }
            //show it on web page
            return View(studentEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student studentModel)
        {
            if (ModelState.IsValid)
            {
                StudentDb.Update(context, studentModel);
                await context.SaveChangesAsync();
                TempData["Message"] = $"student {studentModel.Name} was Updated!";
                return RedirectToAction("Index");
            }
            //return view with errors
            return View(studentModel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Student studentDelete = StudentDb.GetStudent(context, id);

            if(studentDelete == null)
            {
                return NotFound();
            }
            return View(studentDelete);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            //Get Product from database
            Student studentDelete = StudentDb.GetStudent(context, id);
            if (studentDelete != null)
            {
                StudentDb.Delete(context, studentDelete);
                await context.SaveChangesAsync();

                TempData["Message"] = studentDelete.Name + " was deleted.";
                return RedirectToAction("Index");
            }
          
    
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Student studentDetails = StudentDb.GetStudent(context, id);

            return View(studentDetails);
        }
    }
}
