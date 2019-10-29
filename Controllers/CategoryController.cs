using System.Linq;
using Simulacao.Models;
using Simulacao.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System;

namespace Simulacao.Controllers{
    public class CategoryController : Controller{
        private ApplicationDbContext dbContext;
        public CategoryController(){
            dbContext = ApplicationDbContextFactory.Create();
        }
        public ActionResult Index(){
            return View();
        }
        private CategoryViewModel GetCategoryViewModel(long idSubarea){
            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Category = dbContext.Category.SingleOrDefault(c => c.idSubarea == idSubarea);
            cvm.Topics = dbContext.Topic.Where(t => t.idCategory == cvm.Category.idCategory).ToList();
            cvm.Postings = new List<PostingViewModel>();
            string photo = "../images/blank-profile-picture.png";
            ApplicationUser user = new ApplicationUser();
            foreach (var topic in cvm.Topics) {
                user = dbContext.ApplicationUser.SingleOrDefault(u => u.Id == topic.idUser);
                if (user.UserPhoto != null) {
                    var base64 = Convert.ToBase64String(user.UserPhoto);
                    photo = String.Format("data:image/jpg;base64,{0}", base64);
                }
                ViewData["imgUserTopic"] = photo;
                List<Posting> postings = dbContext.Posting.Where(p => p.idTopic == topic.idTopic).ToList();
                foreach (var post in postings) {
                    photo = "../images/blank-profile-picture.png";
                    user = dbContext.ApplicationUser.SingleOrDefault(u => u.Id == post.idUser);
                    if (user.UserPhoto != null) {
                        var base64 = Convert.ToBase64String(user.UserPhoto);
                        photo = String.Format("data:image/jpg;base64,{0}", base64);
                    }
                    PostingViewModel postingTarget = new PostingViewModel{
                        idTopic = post.idTopic,
                        idPosting = post.idPosting,
                        idUser = post.idUser,
                        Message = post.Message,
                        imgSrc = photo
                    };
                    cvm.Postings.Add(postingTarget);
                }
            }
            return cvm;
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ListarCategorias() {
            return View("ListarCategorias", dbContext.Category.ToList());
        }
        [Authorize(Roles = "Developer")]
        public ActionResult CreatingCategory(){
            return View("CreatingCategory", dbContext.Group.ToList());
        }
        [HttpPost]
        [Authorize(Roles = "Developer")]
        public ActionResult CreateCategory(Category category){
            if(ModelState.IsValid){
                dbContext.Category.Add(category);
                dbContext.SaveChanges();
                return RedirectToAction("ListarCategorias");
            } else {
                ViewData["Erro"] = "It is necessary to fill all the informations";
                return View("CreatingCategory", dbContext.Group.ToList());
            }
        }
        [Authorize(Roles = "Developer")]
        public ActionResult GetCategoryToUpdate(int idCategory) {
            Category category = dbContext.Category.SingleOrDefault(c => c.idCategory == idCategory);
            if (category != null) {
                return View("Editar", category);
            } else {
                return View("Error");
            }
        }
        [Authorize(Roles = "Developer")]
        public ActionResult UpdateCategory(Category category) {
            var alvo = dbContext.Category.SingleOrDefault(c => c.idCategory == category.idCategory);
            if(alvo != null && ModelState.IsValid){
                dbContext.Entry(alvo).CurrentValues.SetValues(category);
                dbContext.SaveChanges();
                ViewData["Atualizado"] = "Updated successfully.";
                ViewData["IdCategory"] = category.idCategory;
                return View("Atualizado");
            }else{
                ViewData["Erro"] = "Error updating category. See if all the information are filled.";
                return View("Editar", category);
            }
        }
        [Authorize(Roles = "Developer")]
        public ActionResult DeleteCategory(int idCategory) {
            var alvo = dbContext.Category.SingleOrDefault(c => c.idCategory == idCategory);
            List<Topic> topics = dbContext.Topic.Where(t => t.idCategory == idCategory).ToList();
            if(alvo != null){
                if (topics != null) {
                    foreach (var topic in topics) {
                        dbContext.Topic.Remove(topic);
                    }
                }
                dbContext.Category.Remove(alvo);
                dbContext.SaveChanges();
                return RedirectToAction("ListarCategorias");
            }else{
                return View("Error");
            }
        }
    }
}