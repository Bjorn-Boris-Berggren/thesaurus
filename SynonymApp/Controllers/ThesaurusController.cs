using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using additude.thesaurus.Models;
using Microsoft.EntityFrameworkCore;

namespace additude.thesaurus.Controllers
{
    public class TherasaurusController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ThesaurusContext context;

        public TherasaurusController(ILogger<HomeController> logger, ThesaurusContext context)
        {
            _logger = logger;
            this.context = context;
            List<Word> activities = context.Words.ToList();

            foreach (Word i in activities)
            {
                Console.WriteLine(i.Name);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
