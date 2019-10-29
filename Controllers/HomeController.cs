using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simulacao.Data;
using Simulacao.Models;

namespace Simulacao.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext dbContext;
        public HomeController() {
            dbContext = ApplicationDbContextFactory.Create();
        }
        public IActionResult Index()
        {
            ViewData["Controller"] = "1";
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Certification() {
            ViewData["Controller"] = "2";
            return View();
        }

        public IActionResult GetCertified() {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard() {
            ViewData["numQuestoes"] = dbContext.Questao.Count();
            ViewData["numAreas"] = dbContext.Area.Count();
            ViewData["numUsuarios"] = dbContext.Users.Count();
            ViewData["numMaterial"] = dbContext.MaterialEstudo.Count();
            ViewData["numGrupos"] = dbContext.Group.Count();
            ViewData["numCategorias"] = dbContext.Category.Count();
            ViewData["numCursos"] = dbContext.Course.Count();
            return View("Dashboard");
        }

        public IActionResult GestaoEstoques(long idArea) {
            MaterialEstudoViewModel mevm = new MaterialEstudoViewModel();
            mevm.Subareas = dbContext.Subarea.Where(s => s.idArea == idArea).ToList();
            mevm.MateriaisEstudo = dbContext.MaterialEstudo.Where(m => m.idArea == idArea).ToList();
            ViewData["NomeArea"] = dbContext.Area.SingleOrDefault(a => a.idArea == idArea).NomeArea;
            return View("GestaoEstoques", mevm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
