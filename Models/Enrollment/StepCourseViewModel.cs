using System.Collections.Generic;
using System.Linq;

namespace Simulacao.Models {
    public class StepCourseViewModel {
        public Course Course { get; set; }
        public Area Area { get; set; }
        public List<PaginatedSubarea> PaginatedSubareas { get; set; }
        public PaginatedList<Subarea> Subareas { get; set; }
        public List<MaterialEstudo> StudyMaterials { get; set; }
    }
}