using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simulacao.Models {
    public class SubareaViewModel {
        public List<Area> Areas { get; set; }
        public List<Subarea> Subareas { get; set; }
        public Area Area { get; set; }
        public Subarea Subarea { get; set; }
        public Questao Questao { get; set; }
    }
}