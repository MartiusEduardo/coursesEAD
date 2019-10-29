using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Simulacao.Data;
using Simulacao.Models;

namespace Simulacao.Controllers {
    public class MaterialEstudoController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext dbContext;
        public MaterialEstudoController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
            dbContext = ApplicationDbContextFactory.Create();
        }
        private SubareaViewModel preencherSVM(long? idSubarea = 0) {
            SubareaViewModel svm = new SubareaViewModel();
            if (idSubarea > 0) {
                svm.Subarea = dbContext.Subarea.SingleOrDefault(s => s.idSubarea == idSubarea);
                long idArea = dbContext.Subarea.SingleOrDefault(s => s.idSubarea == idSubarea).idArea;
                svm.Area = dbContext.Area.SingleOrDefault(a => a.idArea == idArea);
            }
            svm.Areas = dbContext.Area.ToList();
            svm.Subareas = dbContext.Subarea.ToList();
            return svm;
        }

        private MaterialEstudoViewModel GetMaterialEstudoViewModel(long idMaterialEstudo) {
            long idSubarea = dbContext.MaterialEstudo.SingleOrDefault(m => m.idMaterialEstudo == idMaterialEstudo).idSubarea;
            long idArea = dbContext.Subarea.SingleOrDefault(s => s.idSubarea == idSubarea).idArea;
            long idCourse = dbContext.Area.SingleOrDefault(a => a.idArea == idArea).idCourse;
            MaterialEstudoViewModel mevm = preencherMEVM(idCourse, idSubarea);
            mevm.MaterialEstudo = dbContext.MaterialEstudo.SingleOrDefault(m => m.idMaterialEstudo == idMaterialEstudo);
            return mevm;

        }

        private MaterialEstudoViewModel preencherMEVM(long idCourse, long idSubarea) {
            MaterialEstudoViewModel mevm = new MaterialEstudoViewModel();
            mevm.Course = dbContext.Course.SingleOrDefault(q => q.idCourse == idCourse);
            mevm.Subarea = dbContext.Subarea.SingleOrDefault(s => s.idSubarea == idSubarea);
            long idArea = dbContext.Subarea.SingleOrDefault(s => s.idSubarea == idSubarea).idArea;
            mevm.Area = dbContext.Area.SingleOrDefault(a => a.idArea == idArea);
            return mevm;
        }

        private CoursesViewModel preencherCVM() {
            CoursesViewModel cvm = new CoursesViewModel();
            cvm.Courses = dbContext.Course.ToList();
            cvm.Areas = dbContext.Area.ToList();
            cvm.Subareas = dbContext.Subarea.ToList();
            return cvm;
        }
        public ActionResult Index() {
            CoursesViewModel cvm = preencherCVM();
            return View(cvm);
        }
        public async Task<ActionResult> ListarMaterial(long idCourse, long idSubarea, string StatusMessage = "") {
            var roles = _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));
            Course alvo = dbContext.Course.SingleOrDefault(c => c.idCourse ==idCourse);
            CoursesViewModel cvm = new CoursesViewModel();
            List<MaterialEstudo> materiaisEstudo = new List<MaterialEstudo>();
            if (idSubarea != 0) {
                long idArea = dbContext.Subarea.SingleOrDefault(s => s.idSubarea == idSubarea).idArea;
                foreach (var role in await roles) {
                    if (idCourse.ToString() == role) {
                        materiaisEstudo = dbContext.MaterialEstudo.Where(q => q.idSubarea == idSubarea).OrderBy(q => q.Ordem).ToList();
                        ViewData["idSubarea"] = idSubarea;
                        ViewData["idCourse"] = idCourse;
                        ViewData["StatusMessage"] = StatusMessage;
                        return View("MateriaisEstudo", materiaisEstudo);
                    }
                }
            }
            ViewData["Erro"] = "You are not allowed to access the study material.";
            return View("Index", preencherCVM());
        }
        public ActionResult InserirMaterial(long idCourse, long idSubarea) {
            return View("Inserir", preencherMEVM(idCourse, idSubarea));
        }
        public async Task<ActionResult> CriarMaterial(MaterialEstudo materialEstudo) {
            long idCourse = dbContext.Area.SingleOrDefault(a => a.idArea == materialEstudo.idArea).idCourse;
            if (ModelState.IsValid) {
                dbContext.MaterialEstudo.Add(materialEstudo);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(ListarMaterial), new {idCourse = idCourse, idSubarea = materialEstudo.idSubarea});
            }
            return View("Inserir", preencherMEVM(idCourse, materialEstudo.idSubarea));
        }
        public ActionResult VerDetalhes(long idMaterialEstudo) {
            MaterialEstudoViewModel mevm = GetMaterialEstudoViewModel(idMaterialEstudo);
            return View(mevm);
        }
        public ActionResult GetMaterialToUpdate(long idMaterialEstudo, string StatusMessage = "") {
            MaterialEstudoViewModel mevm = GetMaterialEstudoViewModel(idMaterialEstudo);
            mevm.StatusMessage = StatusMessage;
            if(mevm != null) {
                return View("Editar", mevm);
            } else {
                return View("Error");
            }
        }
        public async Task<ActionResult> UpdateMaterial(MaterialEstudoViewModel material) {
            var alvo = dbContext.MaterialEstudo.SingleOrDefault(m => m.idMaterialEstudo == material.MaterialEstudo.idMaterialEstudo);
            if(alvo != null && ModelState.IsValid){
                dbContext.Entry(alvo).CurrentValues.SetValues(material.MaterialEstudo);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetMaterialToUpdate), new {idMaterialEstudo = material.MaterialEstudo.idMaterialEstudo, StatusMessage = "The study material has been updated."});
            } else {
                return View("Editar", GetMaterialEstudoViewModel(material.MaterialEstudo.idMaterialEstudo));
            }
        }
        public async Task<ActionResult> DeletarMaterial(long idMaterialEstudo) {
            var alvo = dbContext.MaterialEstudo.SingleOrDefault(m => m.idMaterialEstudo == idMaterialEstudo);
            if(alvo != null){
                long idCourse = dbContext.Area.SingleOrDefault(a => a.idArea == alvo.idArea).idCourse;
                dbContext.MaterialEstudo.Remove(alvo);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(ListarMaterial), new {idCourse = idCourse, idSubarea = alvo.idSubarea});
            }else{
                return View("Error");
            }
        }
        public async void SaveOrder(int[] ordem, long[] idMaterialEstudo) {
            if(ordem.Length != 0 && idMaterialEstudo.Length != 0){
                for (int i = 0; i < idMaterialEstudo.Length; i++)
                {
                    MaterialEstudo material = dbContext.MaterialEstudo.SingleOrDefault(m => m.idMaterialEstudo == idMaterialEstudo[i]);
                    material.Ordem = ordem[i];
                }
                await dbContext.SaveChangesAsync();
            }
        }
    }
}