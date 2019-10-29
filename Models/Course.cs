using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models {
    [Table("Course")]
    public class Course {
        [Key, Column("idCourse")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long idCourse { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Column("CourseName")]
        [Display(Name = "Course")]
        public string CourseName { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Column("ProfessorName")]
        [Display(Name = "Professor Name")]
        public string ProfessorName { get; set; }
        [Required]
        [Column("About")]
        public string About { get; set; }
        [Required]
        [Column("Image")]
        public byte[] Image { get; set; }
        [Required]
        [Column("Price")]
        public string Price { get; set; }

    }
}