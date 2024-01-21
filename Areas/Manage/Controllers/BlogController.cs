using ExamBusiness.Areas.Manage.ViewModels;
using ExamBusiness.DAL;
using ExamBusiness.Helper;
using ExamBusiness.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace ExamBusiness.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]

    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IValidator<CreateBlogVM> _CreateValidator;
        private readonly IValidator<UpdateBlogVM> _UpdateValidator;
        private readonly IWebHostEnvironment _env;
        

        public BlogController(AppDbContext context , IValidator<CreateBlogVM> CreateValidator , IValidator<UpdateBlogVM> UpdateValidator , IWebHostEnvironment env)
        {
            _context = context;
            _CreateValidator = CreateValidator;
            _UpdateValidator = UpdateValidator;
            _env = env;
        }

        public IActionResult Index()
        {
            ReadBlogsManageVM vm = new ReadBlogsManageVM()
            {
                blogs = _context.blogs .ToList()

            };
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM createBlogVM)
        {
            ValidationResult result = await _CreateValidator.ValidateAsync(createBlogVM);
            if (!result.IsValid) 
            {
                result.AddToModelState(this.ModelState);
                return View("Create" , createBlogVM);
            }
            if (!createBlogVM.Image.CheckContent("image"))
            {
                ModelState.AddModelError("image", "Duzgun format daxil edin");
                return View(createBlogVM);
            }
            Blog blog = new Blog()
            {
                Title = createBlogVM.Title,
                Description = createBlogVM.Description,
                ImageUrl = createBlogVM.Image.UploadFile(env:_env.WebRootPath , folderName:"/Upload/"),
                CreatedAt= DateTime.Now,

            };
            await _context.blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            TempData["error"]=string.Empty;
            if (id <= 0) 
            {
                TempData["error"] = "Problem bas verdi";
                return RedirectToAction(nameof(Index));
            }
            Blog blog = _context.blogs.Where(x=>x.Id == id).FirstOrDefault();
            TempData["error"] = string.Empty;
            if (blog == null)
            {
                TempData["error"] = "Problem bas verdi";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = string.Empty;

            UpdateBlogVM vm = new UpdateBlogVM()
            {
                Id = id,
                Title = blog.Title , 
                Description =blog.Description,
                CreatedAt=blog.CreatedAt,
                ImageUrl = blog.ImageUrl 
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateBlogVM updateBlogVM)
        {
            ValidationResult result = await _UpdateValidator.ValidateAsync(updateBlogVM);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View("Update", updateBlogVM);
            }
            Blog blog =  _context.blogs.Where(x=>x.Id==updateBlogVM.Id).FirstOrDefault();
            TempData["error"] = string.Empty;

            if (blog == null) 
            {
                TempData["error"] = "Problem bas verdi";
                return RedirectToAction(nameof(Index));
            }
            if (!updateBlogVM.Image.CheckContent("image"))
            {
                ModelState.AddModelError("image", "Duzgun format daxil edin");
                return View(updateBlogVM);
            }
            TempData["error"] = string.Empty;

            blog.Title = updateBlogVM.Title;
            blog.Description = updateBlogVM.Description;
            blog.ImageUrl = updateBlogVM.Image.UploadFile(env: _env.WebRootPath, folderName: "/Upload/");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            TempData["error"] = string.Empty;
            if (id <= 0)
            {
                TempData["error"] = "Problem bas verdi";
                return RedirectToAction(nameof(Index));
            }
            Blog blog = _context.blogs.Where(x => x.Id == id).FirstOrDefault();
            TempData["error"] = string.Empty;
            if (blog == null)
            {
                TempData["error"] = "Problem bas verdi";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Problem bas verdi";

            _context.blogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
