using System.Collections.Generic;

namespace Simulacao.Models {
    public class CourseInfoViewModel {
        public Course Course { get; set; }
        public List<Area> Areas { get; set; }
        public List<Subarea> Subareas { get; set; }
        public List<PaginatedSubarea> PaginatedSubareas { get; set; }
        public List<MaterialEstudo> Coursewares { get; set; }
        public string StatusMessage { get; set; }
        public string imgSrc { get; set; }
    }
}