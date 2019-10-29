using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Simulacao.Data;
using Simulacao.Models;

namespace Simulacao.Controllers{
    public class SimulacaoController : Controller {
        private ApplicationDbContext dbContext;
        private readonly Configuracoes _configuracoes;
        public SimulacaoController(IOptions<Configuracoes> configuracoes) {
            _configuracoes = configuracoes.Value;
            dbContext = ApplicationDbContextFactory.Create();
        }
        private async Task<SimuladoViewModel> selecionaPerguntas(int? page) {
            SimuladoViewModel svm = new SimuladoViewModel();
            svm.Questoes = await PaginatedList<Questao>.CreateAsync(SimuladoViewModel.listaQuestoes, page ?? 1, 1);
            svm.RespostasQuestao = dbContext.Resposta.Where(r => r.idQuestao == svm.Questoes.First().idQuestao).ToList();
            foreach (var resposta in svm.RespostasQuestao) {
                if (resposta.Correta == true) {
                    ViewData["idRespostaCorreta"] = resposta.idResposta;
                }
            }
            return svm;
        }
        [Authorize(Roles = "Admin, Exam")]
        public async Task<ActionResult> Simulacao(int? page, long idCourse) {
            List<Area> areas = dbContext.Area.Where(a => a.idCourse == idCourse).ToList();
            var idsArea = new long[areas.Count()];
            int i = 0;
            foreach (var area in areas) {
                idsArea.SetValue(area.idArea, i);
                i++;
            }
            var random = new Random();
            var questoesArea = (from q in dbContext.Questao where idsArea.Contains(q.idArea) orderby random.Next() select q).Take(_configuracoes.NumeroQuestoesSimulado);
            if (questoesArea.Count() != 0) {
                //Grava temporariamente as 20 questões do simulado
                SimuladoViewModel.listaQuestoes = questoesArea;
                SimuladoViewModel svm = await selecionaPerguntas(page);
                ViewData["segundos"] = 0;
                return View("Index", svm);
            }
            return RedirectToRoute(new {
                Controller = "Enrollment",
                Action = "Index"
            });
        }
        public async Task<ActionResult> SimulacaoStep(int? page, int segundos) {
            //Seleciona a nova pergunta que irá mostrar para o usuário
            SimuladoViewModel svm = await selecionaPerguntas(page);
            ViewData["segundos"] = segundos;
            return View("Index", svm);
        }
        public ActionResult FinalizarSimulado(int questoesAcertadas, string tempoFinalizado) {
            ViewData["questoesAcertadas"] = questoesAcertadas;
            ViewData["totalQuestoes"] = _configuracoes.NumeroQuestoesSimulado;
            float tempo = Convert.ToInt32(tempoFinalizado);
            if (tempo <= 60) {
                ViewData["tempoFinalizado"] = tempoFinalizado + " seconds";
            } else {
                tempo = tempo / 60;
                int minutos = Convert.ToInt32(tempo);
                if (minutos > tempo) {
                    minutos--;
                }
                int segundos = Convert.ToInt32((tempo - minutos) * 60);
                ViewData["tempoFinalizado"] = minutos + " minute(s) e " + segundos + " seconds";
            }
            return View("FinalizarSimulado");
        }
    }
}