using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NormadForceProject.DAL;
using NormadForceProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NormadForceProject.Controllers
{
    public class HomeController : Controller
    {
        readonly Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Teams.ToList());
        }
    }
}
