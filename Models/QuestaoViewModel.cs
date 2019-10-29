using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simulacao.Models {
    public class QuestaoViewModel {
        public Course Course { get; set; }
        public Area Area { get; set; }
        public Subarea Subarea { get; set; }
        public Questao Questao { get; set; }
        public List<Resposta> Respostas { get; set; }
        public string StatusMessage { get; set; }
    }
}