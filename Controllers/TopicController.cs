using System.Collections.Generic;
using System.Linq;
using Simulacao.Models;
using Simulacao.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace Simulacao.Controllers{
    public class TopicController : Controller{
        private ApplicationDbContext dbContext;
        private Topic GetTopic(long id) => dbContext.Topic.SingleOrDefault(t => t.idTopic == id);
        private Category GetCategory(long idCategory) => dbContext.Category.SingleOrDefault(c => c.idCategory == idCategory);
        private List<Posting> GetPostsByTopic(long idTopic) => dbContext.Posting.Where(p => p.idTopic == idTopic).ToList();
        private List<Topic> GetTopicsByCategory(long id) => dbContext.Topic.Where(t => t.idCategory == id).ToList();
        private Posting GetPosting(long idPosting) => dbContext.Posting.SingleOrDefault(p => p.idPosting == idPosting);
        public TopicController(){
            dbContext = ApplicationDbContextFactory.Create();
        }
        private async Task<TopicViewModel> GetTopicViewModel(long idTopic, int? page) {
            TopicViewModel tvm = new TopicViewModel();
            tvm.topic = GetTopic(idTopic);
            var posting = from p in dbContext.Posting where p.idTopic == idTopic select p;
            tvm.postings = await PaginatedList<Posting>.CreateAsync(posting, page ?? 1, 10);
            tvm.postingsViewModel = new List<PostingViewModel>();
            string photo = "../images/blank-profile-picture.png";
            ApplicationUser user = new ApplicationUser();
            user = dbContext.ApplicationUser.SingleOrDefault(u => u.Id == tvm.topic.idUser);
            if (user.UserPhoto != null) {
                var base64 = Convert.ToBase64String(user.UserPhoto);
                photo = String.Format("data:image/jpg;base64,{0}", base64);
            }
            ViewData["imgUserTopic"] = photo;
            foreach (var post in tvm.postings) {
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
                tvm.postingsViewModel.Add(postingTarget);
            }
            return tvm;
        }
        public async Task<ActionResult> ListTopicsPerCategory(long idCourse){
           List<Topic> topics = new List<Topic>();
            Group group = await dbContext.Group.SingleOrDefaultAsync(g => g.idCourse == idCourse);
            List<Category> categories = await dbContext.Category.Where(c => c.idGroup == group.idGroup).ToListAsync();
            foreach (var category in categories) {
                topics.AddRange(dbContext.Topic.Where(t => t.idCategory == category.idCategory).ToList());
            }
            ViewData["idCourse"] = idCourse;
            ViewData["CourseName"] = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse).CourseName;
            return View("Index", topics);
        }
        public async Task<ActionResult> TopicDetails(long idTopic, int? page){
            TopicViewModel tvm = new TopicViewModel();
            tvm = await GetTopicViewModel(idTopic, page);
            if(tvm != null){
                long idGroup = dbContext.Category.SingleOrDefault(c => c.idCategory == tvm.topic.idCategory).idGroup;
                long idCourse = dbContext.Group.SingleOrDefault(g => g.idGroup == idGroup).idCourse;
                ViewData["idCourse"] = idCourse;
                ViewData["CourseName"] = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse).CourseName;
                return View("TopicDetails", tvm);
            } else {
                return View("Error");
            }
        }
        [Authorize(Roles = "Admin, User")]
        public ActionResult CreatingTopic(long idCategory){
            ViewData["categoryName"] = GetCategory(idCategory).Name;
            ViewData["idCategory"] = idCategory;
            long idCourse = dbContext.Group.SingleOrDefault(g => g.idGroup == dbContext.Category.SingleOrDefault(c => c.idCategory == idCategory).idGroup).idCourse;
            ViewData["idCourse"] = idCourse;
            ViewData["CourseName"] = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse).CourseName;
            return View("CreateTopic");
        }
        public async Task<ActionResult> CreateTopic(Topic topic){
            long idCourse = dbContext.Group.SingleOrDefault(g => g.idGroup == dbContext.Category.SingleOrDefault(c => c.idCategory == topic.idCategory).idGroup).idCourse;
            ViewData["idCourse"] = idCourse;
            ViewData["CourseName"] = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse).CourseName;
            if(ModelState.IsValid){
                dbContext.Topic.Add(topic);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(ListTopicsPerCategory), new {idCourse = idCourse});
            }
            ViewData["categoryName"] = GetCategory(topic.idCategory).Name;
            ViewData["idCategory"] = topic.idCategory;
            return View("CreateTopic");
        }
        public ActionResult GetToEditTopic(long idTopic){
            long idCourse = dbContext.Group.SingleOrDefault(g => g.idGroup == dbContext.Category.SingleOrDefault(c => c.idCategory == dbContext.Topic.SingleOrDefault(t => t.idTopic == idTopic).idCategory).idGroup).idCourse;
            ViewData["CourseName"] = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse).CourseName;
            ViewData["idCourse"] = idCourse;
            return View("EditTopic", GetTopic(idTopic));
        }
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> EditTopic(Topic topic){
            long idCourse = dbContext.Group.SingleOrDefault(g => g.idGroup == dbContext.Category.SingleOrDefault(c => c.idCategory == dbContext.Topic.SingleOrDefault(t => t.idTopic == topic.idTopic).idCategory).idGroup).idCourse;
            ViewData["CourseName"] = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse).CourseName;
            ViewData["idCourse"] = idCourse;
            var target = GetTopic(topic.idTopic);
            if(target != null && ModelState.IsValid){
                dbContext.Entry(target).CurrentValues.SetValues(topic);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(TopicDetails), new {idTopic = topic.idTopic, page = 1});
            }
            return View("EditTopic", GetTopic(topic.idTopic));
        }
        //POSTS PART
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult> CreatePost(Posting posting){
            if (ModelState.IsValid) {
                dbContext.Posting.Add(posting);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(TopicDetails), new {idTopic = posting.idTopic, page = 1});
        }
        public ActionResult GetToEditPosting(long idPosting) {
            long idCourse = dbContext.Group.SingleOrDefault(g => g.idGroup == dbContext.Category.SingleOrDefault(c => c.idCategory == dbContext.Topic.SingleOrDefault(t => t.idTopic == dbContext.Posting.SingleOrDefault(p => p.idPosting == idPosting).idTopic).idCategory).idGroup).idCourse;
            ViewData["CourseName"] = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse).CourseName;
            ViewData["idCourse"] = idCourse;
            return View("EditPosting", GetPosting(idPosting));
        }
        public async Task<ActionResult> EditPosting(Posting posting) {
            var target = GetPosting(posting.idPosting);
            if (target != null && ModelState.IsValid) {
                dbContext.Entry(target).CurrentValues.SetValues(posting);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(TopicDetails), new {idTopic = posting.idTopic, page = 1});
            } else {
                return View("Error");
            }
        }
        public async Task<ActionResult> DeletePosting(long idPosting) {
            var target = GetPosting(idPosting);
            if (target != null) {
                dbContext.Posting.Remove(target);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(TopicDetails), new {idTopic = target.idTopic, page = 1});
            } else {
                return View("Error");
            }
        }
    }
}