using System.Collections.Generic;
using System.Linq;

namespace Simulacao.Models{
    public class SimuladoViewModel{
        public PaginatedList<Questao> Questoes { get; set; }
        public List<Resposta> RespostasQuestao { get; set; }
        public static IQueryable<Questao> listaQuestoes { get; set; }
        public static int QuestoesAcertadas { get; set; }
        public static int TotalQuestoes { get; set; }
        public int idRespostaCorreta { get; set; }
    }
}