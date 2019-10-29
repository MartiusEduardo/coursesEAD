using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simulacao.Data;
using Simulacao.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace Simulacao.Controllers {
    public class AreaController : Controller {
        private ApplicationDbContext dbContext;
        public AreaController() {
            dbContext = ApplicationDbContextFactory.Create();
        }
        //--------------------------------------------------------------------
        //CRUD - Área
        //--------------------------------------------------------------------
        private Area GetArea(long idArea) => dbContext.Area.SingleOrDefault(a => a.idArea == idArea);
        private AreaViewModel GetAreaViewModel(long idArea) {
            AreaViewModel avm = new AreaViewModel();
            avm.Area = dbContext.Area.SingleOrDefault(a => a.idArea == idArea);
            avm.Subareas = dbContext.Subarea.Where(a => a.idArea == idArea).ToList();
            return avm;
        }
        [Authorize(Roles = "Admin")]
        public ActionResult listarAreas() {
            return View("Areas", dbContext.Area.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult inserirArea(long idCourse) {
            ViewData["idCourse"] = idCourse;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult VerDetalhes(int idArea) {
            AreaViewModel avm = GetAreaViewModel(idArea);
            if (avm != null) {
                return View("VerDetalhes", avm);
            } else {
                return View("Error");
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CriarArea(Area area) {
            if (ModelState.IsValid) {
                await dbContext.Area.AddAsync(area);
                await dbContext.SaveChangesAsync();
                return RedirectToRoute(new { 
                    Controller = "Course",
                    Action = "GetCourseToUpdate",
                    idCourse = area.idCourse
                });
            }
            ViewData["idCourse"] = area.idCourse;
            return View("inserirArea");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetAreaToUpdate(long idArea, string StatusMessage = "") {
            AreaViewModel avm = GetAreaViewModel(idArea);
            if (avm != null) {
                avm.StatusMessage = StatusMessage;
                return View("Editar", avm);
            } else {
                return View("Error");
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateArea(Area area) {
            var alvo = GetArea(area.idArea);
            if(alvo != null && ModelState.IsValid) {
                dbContext.Entry(alvo).CurrentValues.SetValues(area);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetAreaToUpdate), new {idArea = alvo.idArea, StatusMessage = "The area has been updated."});
            }
            AreaViewModel avm = GetAreaViewModel(area.idArea);
            avm.StatusMessage = "";
            return View("Editar", avm);
        }
        
        [Authorize(Roles = "Developer")]
        public async Task<ActionResult> DeletarArea(long idArea) {
            var alvo = GetArea(idArea);
            List<Subarea> subareas = dbContext.Subarea.Where(s => s.idArea == idArea).ToList();
            List<MaterialEstudo> materiaisEstudo = dbContext.MaterialEstudo.Where(m => m.idArea == idArea).ToList();
            List<Questao> questoes = dbContext.Questao.Where(q => q.idArea == idArea).ToList();
            if(alvo != null){
                if (subareas != null) {
                    foreach (var subarea in subareas) {
                        dbContext.Subarea.Remove(subarea);
                    }
                }
                if (materiaisEstudo != null) {
                    foreach (var material in materiaisEstudo) {
                        dbContext.MaterialEstudo.Remove(material);
                    }
                }
                if (questoes != null) {
                    foreach (var questao in questoes) {
                        List<Resposta> respostas = dbContext.Resposta.Where(r => r.idQuestao == questao.idQuestao).ToList();
                        foreach (var resposta in respostas) {
                            dbContext.Resposta.Remove(resposta);
                        }
                        dbContext.Questao.Remove(questao);
                    }
                }
                dbContext.Area.Remove(alvo);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(listarAreas));
            }else{
                return View("Error");
            }
        }
        //------------------------------------------------------------------------------------------
        //CRUD - Subáreas
        //------------------------------------------------------------------------------------------
        private Subarea GetSubarea(long idSubarea) => dbContext.Subarea.SingleOrDefault(s => s.idSubarea == idSubarea);
        [Authorize(Roles = "Admin")]
        public ActionResult InserirSubarea(long idArea) {
            ViewData["idArea"] = idArea;
            ViewData["Area"] = GetArea(idArea).NomeArea;
            return View("InserirSubarea");
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CriarSubarea(Subarea subarea) {
            if (ModelState.IsValid) {
                dbContext.Subarea.Add(subarea);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetAreaToUpdate), new {idArea = subarea.idArea});
            }
            ViewData["idArea"] = subarea.idArea;
            ViewData["Area"] = GetArea(subarea.idArea).NomeArea;
            return View("InserirSubarea");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetSubareaToUpdate(long idSubarea) {
            Subarea alvo = GetSubarea(idSubarea);
            if(alvo != null) {
                ViewData["idArea"] = alvo.idArea;
                ViewData["Area"] = GetArea(alvo.idArea).NomeArea;
                return View("EditarSubarea", alvo);
            } else {
                return View("Error");
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateSubarea(Subarea subarea){
            var alvo = GetSubarea(subarea.idSubarea);
            if(alvo != null && ModelState.IsValid){
                dbContext.Entry(alvo).CurrentValues.SetValues(subarea);
                await dbContext.SaveChangesAsync();
                return GetAreaToUpdate(subarea.idArea);
            }else{
                ViewData["idArea"] = alvo.idArea;
                ViewData["Area"] = GetArea(alvo.idArea).NomeArea;
                return View("EditarSubarea", alvo);
            }
        }
        [Authorize(Roles = "Developer")]
        public ActionResult DeletarSubarea(int idSubarea){
            var alvo = GetSubarea(idSubarea);
            List<MaterialEstudo> materiaisEstudo = dbContext.MaterialEstudo.Where(m => m.idSubarea == idSubarea).ToList();
            List<Questao> questoes = dbContext.Questao.Where(q => q.idSubarea == idSubarea).ToList();
            if(alvo != null){
                if (materiaisEstudo != null) {
                    foreach (var material in materiaisEstudo) {
                        dbContext.MaterialEstudo.Remove(material);
                    }
                }
                if (questoes != null) {
                    foreach (var questao in questoes) {
                        List<Resposta> respostas = dbContext.Resposta.Where(r => r.idQuestao == questao.idQuestao).ToList();
                        foreach (var resposta in respostas) {
                            dbContext.Resposta.Remove(resposta);
                        }
                        dbContext.Questao.Remove(questao);
                    }
                }
                dbContext.Subarea.Remove(alvo);
                dbContext.SaveChanges();
                return GetAreaToUpdate(alvo.idArea);
            }else{
                return View("Error");
            }
        }
    }
}