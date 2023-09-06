using Microsoft.AspNetCore.Mvc;
using ResumeCreation.Data;
using ResumeCreation.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ResumeCreation.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ResumeDbContext _context;

        private readonly IWebHostEnvironment _webHost;




        public ResumeController(ResumeDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;

        }
        public IActionResult Index()
        {
            List<Applicant> applicants;
            applicants = _context.Applicants.ToList();
            return View(applicants);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Applicant applicant = new Applicant();
            applicant.Experiences.Add(new Experience() { ExperienceId = 1 });
            //applicant.Experiences.Add(new Experience() { ExperienceId = 2 });
            //applicant.Experiences.Add(new Experience() { ExperienceId = 3 });

            ViewBag.Gender = GetGender();

            return View(applicant);
        }


        [HttpPost]
        public IActionResult Create(Applicant applicant)
        {
            applicant.Experiences.RemoveAll(n => n.YearsWorked == 0);
            applicant.Experiences.RemoveAll(n => n.IsDeleted == true);

            string uniqueFileName = GetUploadedFileName(applicant);
            applicant.PhotoUrl = uniqueFileName;

            _context.Add(applicant);
            _context.SaveChanges();
            return RedirectToAction("index");

        }


        private string GetUploadedFileName(Applicant applicant)
        {
            string uniqueFileName = null;

            if (applicant.ProfilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + applicant.ProfilePhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    applicant.ProfilePhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public IActionResult Details(int Id)
        {
            Applicant applicant = _context.Applicants
                .Include(e => e.Experiences)
                .Where(a => a.Id == Id).FirstOrDefault();

            return View(applicant);

        }

       [HttpGet]
        
        public IActionResult Delete(int Id)
        {
            Applicant applicant = _context.Applicants
                .Include(e => e.Experiences)
                .Where(a => a.Id == Id).FirstOrDefault();

            return View(applicant);
        }

        [HttpPost]

        public IActionResult Delete(Applicant applicant)
        {
            _context.Attach(applicant);
            _context.Entry(applicant).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }



       private List<SelectListItem> GetGender()
        {
            List<SelectListItem> selGender = new List<SelectListItem>();

            var selItem = new SelectListItem() { Value = "", Text = "Select Gender" };

            selGender.Insert(0, selItem);

            selItem = new SelectListItem()
            {
                Value = "Male",
                Text = "Male"
            };

            selGender.Add(selItem);

            selItem = new SelectListItem()
            {
                Value = "Female",
                Text = "Female"
            };

            selGender.Add(selItem);

            return selGender;
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Applicant applicant = _context.Applicants
                .Include(e => e.Experiences)
                .Where(a => a.Id == Id).FirstOrDefault();
            ViewBag.Gender = GetGender();

            return View(applicant);

        }


        [HttpPost]
        public IActionResult Edit(Applicant applicant)
        {

            List<Experience> expDetails = _context.Experiences.Where(d => d.ApplicantId == applicant.Id).ToList();
            _context.Experiences.RemoveRange(expDetails);
            _context.SaveChanges();


            applicant.Experiences.RemoveAll(n => n.YearsWorked == 0);
            applicant.Experiences.RemoveAll(n => n.IsDeleted == true);

            if (applicant.ProfilePhoto != null)
            {
                string uniqueFileName = GetUploadedFileName(applicant);
                applicant.PhotoUrl = uniqueFileName;

            }

            _context.Attach(applicant);
            _context.Entry(applicant).State = EntityState.Modified;
            _context.Experiences.AddRange(applicant.Experiences);
            _context.SaveChanges();
            return RedirectToAction("index");

        }

    }
}
