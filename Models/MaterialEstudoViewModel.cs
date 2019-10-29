using System.Collections.Generic;

namespace Simulacao.Models {
    public class MaterialEstudoViewModel {
        public Course Course { get; set; }
        public Area Area { get; set; }
        public Subarea Subarea { get; set; }
        public List<Subarea> Subareas { get; set; }
        public MaterialEstudo MaterialEstudo { get; set; }
        public List<MaterialEstudo> MateriaisEstudo { get; set; }
        public string StatusMessage { get; set; }
    }
}