using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simulacao.Data;
using Simulacao.Models;

namespace Simulacao.Controllers{
    public class GroupController : Controller{
        private ApplicationDbContext dbContext;
        public GroupController(){
            dbContext = ApplicationDbContextFactory.Create();
        }
        private Group GetGroup(int idGroup) => dbContext.Group.SingleOrDefault(g => g.idGroup == idGroup);
        [Authorize(Roles = "Admin")]
        public ActionResult Index() {
            return View("Index", dbContext.Group.ToList());
        }
        [Authorize(Roles = "Developer")]
        public ActionResult CreatingGroup() {
            return View("CreatingGroup");
        }
        [Authorize(Roles = "Developer")]
        public ActionResult CreateGroup(Group group){
            if(ModelState.IsValid){
                dbContext.Group.Add(group);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            } else {
                ViewData["Erro"] = "It is necessary to fill all the informations";
                return View("CreatingGroup");
            }
        }
        [Authorize(Roles = "Developer")]
        public ActionResult GetGroupToUpdate(int idGroup) {
            Group group = dbContext.Group.SingleOrDefault(g => g.idGroup == idGroup);
            if (group != null) {
                return View("Editar", group);
            } else {
                return View("Error");
            }
        }
        [Authorize(Roles = "Developer")]
        public ActionResult UpdateGroup(Group group) {
            var alvo = dbContext.Group.SingleOrDefault(g => g.idGroup == group.idGroup);
            if(alvo != null && ModelState.IsValid){
                dbContext.Entry(alvo).CurrentValues.SetValues(group);
                dbContext.SaveChanges();
                ViewData["Atualizado"] = "Updated successfully.";
                ViewData["IdGroup"] = group.idGroup;
                return View("Atualizado");
            }else{
                ViewData["Erro"] = "Error updating group. See if all the information are filled.";
                return View("Editar", group);
            }
        }
        [Authorize(Roles = "Developer")]
        public ActionResult DeleteGroup(int idGroup) {
            var alvo = dbContext.Group.SingleOrDefault(g => g.idGroup == idGroup);
            List<Category> categories = dbContext.Category.Where(c => c.idGroup == idGroup).ToList();
            if(alvo != null){
                if (categories != null) {
                    foreach (var category in categories) {
                        List<Topic> topics = dbContext.Topic.Where(t => t.idCategory == category.idCategory).ToList();
                        if (topics != null) {
                            foreach (var topic in topics) {
                                dbContext.Topic.Remove(topic);
                            }
                        }
                        dbContext.Category.Remove(category);
                    }
                }
                dbContext.Group.Remove(alvo);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }else{
                return View("Error");
            }
        }
    }
}