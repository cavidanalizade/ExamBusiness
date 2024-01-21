using ExamBusiness.DAL;
using ExamBusiness.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExamBusiness.Controllers
{
    public class HomeController : Controller
    {      
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public  IActionResult Index()
        {
            ReadBlogsVM vm = new ReadBlogsVM()
            {
                blogs = _context.blogs.ToList()
            };
            return View(vm);
        }
   
    }
}