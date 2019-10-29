using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Simulacao.Data;
using Simulacao.Models;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Simulacao.Controllers {
    public class QuestaoController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext dbContext;
        public QuestaoController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
            dbContext = ApplicationDbContextFactory.Create();
        }
        private Questao GetQuestao(long idQuestao) => dbContext.Questao.SingleOrDefault(q => q.idQuestao == idQuestao);
        private QuestaoViewModel GetQuestaoViewModel(long idQuestao) {
            long idSubarea = dbContext.Questao.SingleOrDefault(q => q.idQuestao == idQuestao).idSubarea;
            long idArea = dbContext.Subarea.SingleOrDefault(s => s.idSubarea == idSubarea).idArea;
            long idCourse = dbContext.Area.SingleOrDefault(a => a.idArea == idArea).idCourse;
            QuestaoViewModel qvm = preencherQVM(idCourse, idSubarea);
            qvm.Questao = GetQuestao(idQuestao);
            qvm.Respostas = dbContext.Resposta.Where(r => r.idQuestao == idQuestao).ToList();
            return qvm;
        }
        private QuestaoViewModel preencherQVM(long idCourse, long idSubarea) {
            QuestaoViewModel qvm = new QuestaoViewModel();
            qvm.Course = dbContext.Course.SingleOrDefault(q => q.idCourse == idCourse);
            qvm.Subarea = dbContext.Subarea.SingleOrDefault(s => s.idSubarea == idSubarea);
            long idArea = dbContext.Subarea.SingleOrDefault(s => s.idSubarea == idSubarea).idArea;
            qvm.Area = dbContext.Area.SingleOrDefault(a => a.idArea == idArea);
            return qvm;
        }
        private CoursesViewModel preencherCVM() {
            CoursesViewModel cvm = new CoursesViewModel();
            cvm.Courses = dbContext.Course.ToList();
            cvm.Areas = dbContext.Area.ToList();
            cvm.Subareas = dbContext.Subarea.ToList();
            return cvm;
        }
        public JsonResult filtrarSubareas(long idArea) {
            IEnumerable<Subarea> subareas;
            if (idArea != 0) {
                subareas = dbContext.Subarea.Where(s => s.idArea == idArea).ToList();
            } else {
                subareas = dbContext.Subarea.ToList();
            }
            return Json(new SelectList(subareas, "idSubarea", "NomeSubarea"));
        }
        public JsonResult filtrarAreas(long idCourse) {
            IEnumerable<Area> areas;
            if (idCourse != 0) {
                areas = dbContext.Area.Where(s => s.idCourse == idCourse).ToList();
            } else {
                areas = dbContext.Area.ToList();
            }
            return Json(new SelectList(areas, "idArea", "NomeArea"));
        }
        public ActionResult PerguntasArea() {
            CoursesViewModel cvm = preencherCVM();
            return View(cvm);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> listarPerguntas(long idCourse, long idSubarea) {
            var roles = _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));
            Course alvo = dbContext.Course.SingleOrDefault(c => c.idCourse ==idCourse);
            List<Questao> questoes = new List<Questao>();
            if (idCourse != 0) {
                foreach (var role in await roles) {
                    if (idCourse.ToString() == role) {
                        questoes = dbContext.Questao.Where(q => q.idSubarea == idSubarea).ToList();
                        ViewData["idSubarea"] = idSubarea;
                        ViewData["idCourse"] = idCourse;
                        return View("Questoes", questoes);
                    }
                }
            }
            ViewData["Erro"] = "You are not allowed to access the questions.";
            return View("PerguntasArea", preencherCVM());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult VerDetalhes(long idQuestao) {
            return View("VerDetalhes", GetQuestaoViewModel(idQuestao));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult inserirPergunta(long idCourse, long idSubarea) {
            return View("Inserir", preencherQVM(idCourse, idSubarea));
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CriarQuestao(Questao questao) {
            long idCourse = dbContext.Area.SingleOrDefault(a => a.idArea == questao.idArea).idCourse;
            if (ModelState.IsValid) {
                dbContext.Questao.Add(questao);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(listarPerguntas), new {idCourse = idCourse, idSubarea = questao.idSubarea});
            }
            return View("Inserir", preencherQVM(idCourse, questao.idSubarea));
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetQuestaoToUpdate(long idQuestao, string StatusMessage = ""){
            QuestaoViewModel qvm = GetQuestaoViewModel(idQuestao);
            qvm.StatusMessage = StatusMessage;
            if(qvm.Questao != null) {
                return View("Editar", qvm);
            } else {
                return View("Error");
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateQuestao(Questao questao){
            var alvo = GetQuestao(questao.idQuestao);
            if(alvo != null && ModelState.IsValid) {
                dbContext.Entry(alvo).CurrentValues.SetValues(questao);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetQuestaoToUpdate), new {idQuestao = questao.idQuestao, StatusMessage = "The question has been updated."});
            } else {
                return View("Editar", GetQuestaoViewModel(questao.idQuestao));
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletarQuestao(long idQuestao){
            var alvo = GetQuestao(idQuestao);
            List<Resposta> respostas = dbContext.Resposta.Where(r => r.idQuestao == idQuestao).ToList();
            if(alvo != null){
                if (respostas != null) {
                    foreach (var resposta in respostas) {
                        dbContext.Resposta.Remove(resposta);
                    }
                }
                dbContext.Questao.Remove(alvo);
                await dbContext.SaveChangesAsync();
                long idArea = dbContext.Subarea.SingleOrDefault(s => s.idSubarea == alvo.idSubarea).idArea;
                long idCourse = dbContext.Area.SingleOrDefault(a => a.idArea == idArea).idCourse;
                return RedirectToAction(nameof(listarPerguntas), new {idCourse = idCourse, idSubarea = alvo.idSubarea});
            }else{
                return View("Error");
            }
        }
    }
}