using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models {
    [Table("MaterialEstudo")]
    public class MaterialEstudo{
        [Key, Column("idmaterialestudo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long idMaterialEstudo { get; set; }
        [Column("idarea")]
        public long idArea { get; set; }
        [Column("idsubarea")]
        public long idSubarea { get; set; }
        [Required]
        [Column("titulo")]
        [Display(Name = "Title")]
        public string Titulo { get; set; }
        [Required]
        [Column("descricaoconteudo")]
        [Display(Name = "Content Description")]
        public string DescricaoConteudo { get; set; }
        [Required]
        [Column("ordem")]
        [Display(Name = "Order")]
        public int Ordem { get; set; }
    }
}