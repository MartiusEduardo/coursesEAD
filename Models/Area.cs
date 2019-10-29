using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models {
    [Table("area")]
    public class Area {
        [Key, Column("idarea")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long idArea { get; set; }
        [Required]
        [Column("idCourse")]
        public long idCourse { get; set; }
        [Required]
        [Column("area")]
        [Display(Name = "Area")]
        public string NomeArea { get; set; }
    }
}