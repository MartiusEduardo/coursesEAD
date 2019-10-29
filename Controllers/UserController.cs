using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Simulacao.Data;
using Simulacao.Models;
using Simulacao.Models.ManageViewModels;

namespace Simulacao.Controllers {
    public class UserController : Controller {
        private ApplicationDbContext dbContext;
        public UserController() {
            dbContext = ApplicationDbContextFactory.Create();
        }
        [Authorize(Roles = "Developer")]
        public ActionResult listarUsuarios() {
            return View("ListarUsuarios", dbContext.ApplicationUser.ToList());
        }
        public ActionResult GetUserToUpdate(string idUser) {
            UserViewModel uvm = new UserViewModel();
            uvm.ApplicationUser = dbContext.ApplicationUser.SingleOrDefault(u => u.Id == idUser);
            uvm.UserRoles = dbContext.UserRoles.Where(u => u.UserId == idUser).ToList();
            uvm.Roles = dbContext.Roles.ToList();
            uvm.Courses = dbContext.Course.ToList();
            return View("EditarFuncoes", uvm);
        }
        [Authorize(Roles = "Developer")]
        public async Task<ActionResult> ExcluirFuncao(string UserId, string RoleId) {
            IdentityUserRole<string> alvo = dbContext.UserRoles.SingleOrDefault(u => u.UserId == UserId && u.RoleId == RoleId);
            if (alvo != null) {
                dbContext.UserRoles.Remove(alvo);
                await dbContext.SaveChangesAsync();
            }
            return GetUserToUpdate(UserId);
        }
        [Authorize(Roles = "Developer")]
        public async Task<ActionResult> inserirFuncao(IdentityUserRole<string> userRole) {
            IdentityUserRole<string> alvo = dbContext.UserRoles.SingleOrDefault(u => u.UserId == userRole.UserId && u.RoleId == userRole.RoleId);
            if (ModelState.IsValid && alvo == null) {
                dbContext.UserRoles.Add(userRole);
                await dbContext.SaveChangesAsync();
                ModelState.Clear();
                return GetUserToUpdate(userRole.UserId);
            }
            ViewData["Erro"] = "Já existe esta função para o usuário.";
            return GetUserToUpdate(userRole.UserId);
        }
        [Authorize(Roles = "Developer")]
        public ActionResult adicionarRole(IdentityRole role, string UserId) {
            IdentityRole alvo = dbContext.Roles.SingleOrDefault(r => r.Id == role.Id || r.Name == role.Name);
            if (ModelState.IsValid && alvo == null) {
                dbContext.Roles.Add(role);
                dbContext.SaveChanges();
                ModelState.Clear();
                return GetUserToUpdate(UserId);
            }
            ViewData["Erro 2"] = "Já existe esta função (role).";
            return GetUserToUpdate(UserId);
        }

        public async Task<ActionResult> DeleteRole(string RoleId, string UserId) {
            IdentityRole alvo = dbContext.Roles.SingleOrDefault(r => r.Id == RoleId);
            if (alvo != null && ModelState.IsValid) {
                dbContext.Roles.Remove(alvo);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(GetUserToUpdate), new {idUser = UserId});
        }
    }
}