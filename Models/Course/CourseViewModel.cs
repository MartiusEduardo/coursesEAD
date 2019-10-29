using System.ComponentModel.DataAnnotations;

namespace Simulacao.Models {
    public class CourseViewModel {
        public long idCourse { get; set; }
        [Display(Name = "Course")]
        public string CourseName { get; set; }
        [Display(Name = "Professor Name")]
        public string ProfessorName { get; set; }
        public string About { get; set; }
        public byte[] Image { get; set; }
        public string Price { get; set; }
        public string StatusMessage { get; set; }
        public string imgSrc { get; set; }
    }
}