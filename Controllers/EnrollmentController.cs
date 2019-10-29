using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulacao.Data;
using Simulacao.Models;

namespace Simulacao.Controllers
{
    public class EnrollmentController : Controller {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext dbContext;
        public EnrollmentController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
            dbContext = ApplicationDbContextFactory.Create();
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
        
        private async Task<StepCourseViewModel> selecionarMaterialDidatico(int? page, long? idCourse) {
            StepCourseViewModel scvm = new StepCourseViewModel();
            scvm.Course = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse);
            IQueryable<Subarea> subareas = from s in dbContext.Subarea where s.idArea == 0 select s;
            foreach (var area in await dbContext.Area.Where(a => a.idCourse == idCourse).ToListAsync()) {
                subareas = (from s in dbContext.Subarea where s.idArea == area.idArea select s).Concat(subareas);
            }
            scvm.Subareas = await PaginatedList<Subarea>.CreateAsync(subareas.OrderBy(s => s.idArea), page ?? 1, 1);
            scvm.Area = dbContext.Area.SingleOrDefault(a => a.idArea == scvm.Subareas.First().idArea);
            scvm.StudyMaterials = dbContext.MaterialEstudo.Where(m => m.idSubarea == scvm.Subareas.First().idSubarea).OrderBy(m => m.Ordem).ToList();
            return scvm;
        }

        public IActionResult Index() {
            EnrollmentViewModel evm = new EnrollmentViewModel();
            List<CourseViewModel> cvms = new List<CourseViewModel>();
            List<Enrollment> enrollments = dbContext.Enrollment.Where(e => e.UserId == _userManager.GetUserId(User)).ToList();
            foreach (var enrollment in enrollments)
            {
                Course course = dbContext.Course.SingleOrDefault(c => c.idCourse == enrollment.idCourse);
                CourseViewModel cvm = new CourseViewModel {
                    idCourse = course.idCourse,
                    CourseName = course.CourseName,
                    ProfessorName = course.ProfessorName,
                    About = course.About,
                    Price = course.Price
                };
                if (course.Image != null) {
                    var base64 = Convert.ToBase64String(course.Image);
                    cvm.imgSrc = String.Format("data:image/jpg;base64,{0}", base64);
                }
                cvms.Add(cvm);
            }
            if (cvms != null) {
                evm.Courses = cvms;
            }
            if (evm.Courses == null) {
                ViewData["NenhumCurso"] = "1";
            }
            return View("MyCourses", evm);
        }

        public async Task<IActionResult> Enroll(long idCourse) {
            if (ModelState.IsValid) {
                Enrollment enrollment = new Enrollment {
                    UserId = _userManager.GetUserId(User),
                    idCourse = idCourse,
                    RegisterDate = DateTime.Today
                };
                dbContext.Enrollment.Add(enrollment);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToRoute( new {
                controller = "Course",
                action = "listarCursos"
            });
        }

        public IActionResult StartCourse(long idCourse) {
            CourseInfoViewModel civm = new CourseInfoViewModel();
            civm.Course = dbContext.Course.SingleOrDefault(c => c.idCourse ==idCourse);
            civm.Areas = dbContext.Area.Where(c => c.idCourse == idCourse).ToList();
            civm.PaginatedSubareas = new List<PaginatedSubarea>();
            civm.Coursewares = new List<MaterialEstudo>();
            int i = 1;
            foreach (var area in civm.Areas) {
                List<Subarea> subareas = dbContext.Subarea.Where(s => s.idArea == area.idArea).ToList();
                foreach (var subarea in subareas) {
                    PaginatedSubarea pagSubarea = new PaginatedSubarea{
                        idSubarea = subarea.idSubarea,
                        idArea = subarea.idArea,
                        NomeSubarea = subarea.NomeSubarea,
                        Page = i++
                    };
                    civm.PaginatedSubareas.Add(pagSubarea);
                    List<MaterialEstudo> materiais = dbContext.MaterialEstudo.Where(m => m.idSubarea == subarea.idSubarea).ToList();
                    foreach (var material in materiais) {
                        if (material != null) {
                            civm.Coursewares.Add(material);
                        }
                    }
                }
            }
            return View(civm);
        }

        public async Task<IActionResult> Course(long idCourse, int page = 1) {
            StepCourseViewModel scvm = await selecionarMaterialDidatico(page, idCourse);
            ViewData["idCourse"] = idCourse;
            ViewData["CourseName"] = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse).CourseName;
            return View(scvm);
        }

        public IActionResult Discussion(long idCourse) {
            return RedirectToRoute(new {
                controller = "Topic",
                action = "ListTopicsPerCategory",
                idCourse = idCourse
            });
        }
        public async Task<IActionResult> Unenroll(long idCourse) {
            var alvo = await dbContext.Enrollment.SingleOrDefaultAsync(e => e.idCourse == idCourse && e.UserId == _userManager.GetUserId(User));
            if(alvo != null && ModelState.IsValid) {
                dbContext.Enrollment.Remove(alvo);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}