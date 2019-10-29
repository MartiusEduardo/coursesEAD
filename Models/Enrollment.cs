using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models {
    [Table("Enrollment")]
    public class Enrollment {
        [Key, Column("idEnrollment")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long idEnrollment { get; set; }
        [Required]
        [Column("UserId")]
        public string UserId { get; set; }
        [Required]
        [Column("idCourse")]
        public long idCourse { get; set; }
        [Column("RegisterDate")]
        [Display(Name = "Register Date")]
        public DateTime RegisterDate { get; set; }
    }
}