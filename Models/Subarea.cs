using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models {
    [Table("subarea")]
    public class Subarea {
        [Key, Column("idsubarea")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long idSubarea { get; set; }
        [Column("idarea")]
        public long idArea { get; set; }
        [Column("subarea")]
        [Display(Name = "Subarea")]
        [Required]
        public string NomeSubarea { get; set; }
    }
}