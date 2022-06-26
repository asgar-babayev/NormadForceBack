using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NormadForceProject.DAL;
using NormadForceProject.Models;
using System;
using System.IO;
using System.Linq;

namespace NormadForceProject.Areas.Manage.Controllers
{
    [Area("Manage"), Authorize]
    public class TeamController : Controller
    {
        readonly Context _context;
        readonly IWebHostEnvironment _env;

        public TeamController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.Teams.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Team team)
        {
            if (team.ImageFile != null)
            {
                if (team.ImageFile.ContentType != "image/jpeg" && team.ImageFile.ContentType != "image/png" && team.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("", "Invalid image format");
                    return View(team);
                }
                if (team.ImageFile.Length / 1024 > 2000)
                {
                    ModelState.AddModelError("", "Invalid image size");
                    return View(team);
                }
                string fileName = team.ImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName.Substring(fileName.Length - 64, 64);
                }
                string newFileName = Guid.NewGuid().ToString() + fileName;
                string path = Path.Combine(_env.WebRootPath, "assets/images", newFileName);
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    team.ImageFile.CopyTo(fs);
                }
                team.Image = newFileName;
                _context.Teams.Add(team);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Image is required");
            return View(team);
        }

        public IActionResult Edit(int id)
        {
            return View(_context.Teams.FirstOrDefault(x => x.Id == id));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Team team)
        {
            Team existTeam = _context.Teams.FirstOrDefault(x => x.Id == team.Id);
            string newFileName = null;
            if (team.ImageFile != null)
            {
                if (team.ImageFile.ContentType != "image/jpeg" && team.ImageFile.ContentType != "image/png" && team.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("", "Invalid image format");
                    return View(team);
                }
                if (team.ImageFile.Length / 1024 > 2000)
                {
                    ModelState.AddModelError("", "Invalid image size");
                    return View(team);
                }
                string fileName = team.ImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName.Substring(fileName.Length - 64, 64);
                }
                newFileName = Guid.NewGuid().ToString() + fileName;
                string path = Path.Combine(_env.WebRootPath, "assets/images", newFileName);
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    team.ImageFile.CopyTo(fs);
                }
                team.Image = newFileName;

                string delPath = Path.Combine(_env.WebRootPath, "assets/images", existTeam.Image);
                if (System.IO.File.Exists(delPath))
                {
                    System.IO.File.Delete(delPath);
                }
                existTeam.Image = newFileName;
            }

            existTeam.FullName = team.FullName;
            existTeam.Profession = team.Profession;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var existTeam = _context.Teams.FirstOrDefault(x => x.Id == id);
            _context.Teams.Remove(existTeam);
            _context.SaveChanges();
            string delPath = Path.Combine(_env.WebRootPath, "assets/images", existTeam.Image);
            if (System.IO.File.Exists(delPath))
            {
                System.IO.File.Delete(delPath);
            }
            return RedirectToAction("index");
        }
    }
}
