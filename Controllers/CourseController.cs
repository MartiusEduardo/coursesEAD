using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Simulacao.Data;
using Simulacao.Models;

namespace Simulacao.Controllers
{
    public class CourseController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext dbContext;
        public CourseController(UserManager<ApplicationUser> userManager) {
            _userManager = userManager;
            dbContext = ApplicationDbContextFactory.Create();
        }

        public IActionResult Index() {
            List<CourseViewModel> cvms = new List<CourseViewModel>();
            foreach (var course in dbContext.Course.ToList()) {
                var UserId = _userManager.GetUserId(User);
                List<Enrollment> enrollments = dbContext.Enrollment.Where(e => e.UserId == UserId).ToList();
                bool enrolledCourse = false;
                foreach (var enrolled in enrollments) {
                    if (course.idCourse == enrolled.idCourse) {enrolledCourse = true;} else {enrolledCourse = false;}
                }
                if (enrolledCourse == false) {
                    CourseViewModel model = new CourseViewModel();
                    model.idCourse = course.idCourse;
                    model.CourseName = course.CourseName;
                    model.ProfessorName = course.ProfessorName;
                    model.About = course.About;
                    if (course.Image != null) {
                        var base64 = Convert.ToBase64String(course.Image);
                        model.imgSrc = String.Format("data:image/jpg;base64,{0}", base64);
                    }
                    cvms.Add(model);
                }
            }
            return View(cvms);
        }

        public IActionResult CourseInfo(long idCourse) {
            CourseInfoViewModel civm = new CourseInfoViewModel();
            civm.Course = dbContext.Course.SingleOrDefault(c => c.idCourse ==idCourse);
            civm.Areas = dbContext.Area.Where(c => c.idCourse == idCourse).ToList();
            civm.Subareas = new List<Subarea>();
            foreach (var area in civm.Areas) {
                List<Subarea> subareas = dbContext.Subarea.Where(s => s.idArea == area.idArea).ToList();
                foreach (var subarea in subareas) {
                    civm.Subareas.Add(subarea);
                }
            }
            if (civm.Course.Image != null) {
                var base64 = Convert.ToBase64String(civm.Course.Image);
                civm.imgSrc = String.Format("data:image/jpg;base64,{0}", base64);
            }
            return View(civm);
        }

        //--------------------------------------------------------------------
        //Parte de administração do curso
        //--------------------------------------------------------------------
        [Authorize(Roles = "Admin")]
        public ActionResult listarCursos() {
            return View("Courses", dbContext.Course.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult inserirCurso() {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CriarCurso(Course course, IFormFile coursephoto) {
            if (coursephoto == null || course.CourseName == null || course.ProfessorName == null || course.About == null || course.Price == null) {
                return View("inserirCurso");
            }
            using (var ms = new System.IO.MemoryStream()) {
                await coursephoto.CopyToAsync(ms);
                course.Image = ms.ToArray();
                await dbContext.Course.AddAsync(course);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(listarCursos));
            }
        }

        private CourseInfoViewModel GetCourseInfoViewModel(long idCourse) {
            CourseInfoViewModel civm = new CourseInfoViewModel();
            civm.Course = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse);
            civm.Areas = dbContext.Area.Where(a => a.idCourse == idCourse).ToList();
            return civm;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetCourseToUpdate(int idCourse, string StatusMessage = "") {
            CourseInfoViewModel civm = GetCourseInfoViewModel(idCourse);
            if (civm != null) {
                if (civm.Course.Image != null) {
                    var base64 = Convert.ToBase64String(civm.Course.Image);
                    civm.imgSrc = String.Format("data:image/jpg;base64,{0}", base64);
                }
                civm.StatusMessage = StatusMessage;
                return View("Editar", civm);
            } else {
                return View("Error");
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCourse(Course course, IFormFile coursephoto) {
            var alvo = dbContext.Course.SingleOrDefault(c => c.idCourse == course.idCourse);
            if(alvo != null) {
                if ((alvo.Image == null && coursephoto == null) || course.CourseName == null || course.ProfessorName == null || course.About == null || course.Price == null) {
                    return View("Editar", GetCourseInfoViewModel(course.idCourse));
                }
            }

            if (course.CourseName != alvo.CourseName) {
                dbContext.Entry(alvo).Property("CourseName").CurrentValue = course.CourseName;
                await dbContext.SaveChangesAsync();
            }

            if (course.ProfessorName != alvo.ProfessorName) {
                dbContext.Entry(alvo).Property("ProfessorName").CurrentValue = course.ProfessorName;
                await dbContext.SaveChangesAsync();
            }

            if (course.Price != alvo.Price) {
                dbContext.Entry(alvo).Property("Price").CurrentValue = course.Price;
                await dbContext.SaveChangesAsync();
            }

            if (course.About != alvo.About) {
                dbContext.Entry(alvo).Property("About").CurrentValue = course.About;
                await dbContext.SaveChangesAsync();
            }

            if (coursephoto != null) {
                using (var ms = new System.IO.MemoryStream()) {
                    await coursephoto.CopyToAsync(ms);
                    dbContext.Entry(alvo).Property("Image").CurrentValue = ms.ToArray();
                    await dbContext.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(GetCourseToUpdate), new {idCourse = alvo.idCourse, StatusMessage = "The course has been updated."});
        }

        [Authorize(Roles = "Developer")]
        public async Task<ActionResult> DeletarCurso(int idCourse) {
            var alvo = dbContext.Course.SingleOrDefault(c => c.idCourse == idCourse);
            IdentityRole targetRole = dbContext.Roles.SingleOrDefault(r => r.Name == idCourse.ToString());
            if(alvo != null){
                foreach (var area in dbContext.Area.ToList()) {
                    foreach (var subarea in dbContext.Subarea.ToList()) {
                        if (area.idArea == subarea.idArea) {
                            foreach (var material in dbContext.MaterialEstudo.ToList()) {
                                if (material.idSubarea == subarea.idSubarea) {
                                    dbContext.MaterialEstudo.Remove(material);
                                }
                            }
                            foreach (var questao in dbContext.Questao.ToList()) {
                                if (questao.idSubarea == subarea.idSubarea) {
                                    foreach (var resposta in dbContext.Resposta.ToList()) {
                                        if (resposta.idQuestao == questao.idQuestao) {
                                            dbContext.Resposta.Remove(resposta);
                                        }
                                    }
                                    dbContext.Questao.Remove(questao);
                                }
                            }
                            dbContext.Subarea.Remove(subarea);
                        }
                    }
                    dbContext.Area.Remove(area);
                }
                if (targetRole != null) {
                    dbContext.Roles.Remove(targetRole);
                }
                dbContext.Course.Remove(alvo);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(listarCursos));
            }else{
                return View("Error");
            }
        }
    }
}