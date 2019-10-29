using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simulacao.Data;
using Simulacao.Models;

namespace Simulacao.Controllers {
    public class RespostaController : Controller{
        private ApplicationDbContext dbContext;
        public RespostaController() {
            dbContext = ApplicationDbContextFactory.Create();
        }

        private Resposta GetResposta(long idResposta) => dbContext.Resposta.SingleOrDefault(r => r.idResposta == idResposta);

        private Questao GetQuestao(long idQuestao) => dbContext.Questao.SingleOrDefault(q => q.idQuestao == idQuestao);
        
        public ActionResult VerResposta(long idResposta) {
            Resposta alvo = GetResposta(idResposta);
            if (alvo != null) {
                return View("VerResposta", alvo);
            } else {
                return View("Error");
            }
        }
        public ActionResult inserirResposta(long idQuestao) {
            RespostaViewModel rvm = new RespostaViewModel();
            rvm.Questao = dbContext.Questao.SingleOrDefault(q => q.idQuestao == idQuestao);
            return View(rvm);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CriarResposta(Resposta resposta) {
            if (ModelState.IsValid) {
                dbContext.Resposta.Add(resposta);
                await dbContext.SaveChangesAsync();
                return RedirectToRoute(new {
                    controller = "Questao",
                    action = "GetQuestaoToUpdate",
                    idQuestao = resposta.idQuestao
                });
            }
            RespostaViewModel rvm = new RespostaViewModel();
            rvm.Questao = dbContext.Questao.SingleOrDefault(q => q.idQuestao == resposta.idQuestao);
            return View("InserirResposta", rvm);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetRespostaToUpdate(long idResposta, string StatusMesage = ""){
            RespostaViewModel rvm = new RespostaViewModel();
            rvm.Resposta = GetResposta(idResposta);
            if(rvm != null) {
                rvm.StatusMessage = StatusMesage;
                return View("EditarResposta", rvm);
            } else {
                return View("Error");
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateResposta(Resposta resposta){
            var alvo = GetResposta(resposta.idResposta);
            if(alvo != null && ModelState.IsValid){
                dbContext.Entry(alvo).CurrentValues.SetValues(resposta);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(GetRespostaToUpdate), new {idResposta = alvo.idResposta, StatusMesage = "The answer has been updated."});
            } else {
                RespostaViewModel rvm = new RespostaViewModel();
                rvm.Resposta = alvo;
                return View("EditarResposta", rvm);
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletarResposta(long idResposta){
            var alvo = GetResposta(idResposta);
            if(alvo != null){
                dbContext.Resposta.Remove(alvo);
                await dbContext.SaveChangesAsync();
                return RedirectToRoute(new {
                    controller = "Questao",
                    action = "GetQuestaoToUpdate",
                    idQuestao = alvo.idQuestao
                });
            }else{
                return View("Error");
            }
        }
    }
}