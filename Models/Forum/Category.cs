using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models{
    [Table("category")]
    public class Category{
        [Key, Column("idcategory")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long idCategory { get; set; }
        [Column("idgroup")]
        [Required]
        public long idGroup { get; set; }
        [Column("idSubarea")]
        [Required]
        public long idSubarea { get; set; }
        [Column("name")]
        [Required]
        public string Name { get; set; }
        [Column("description")]
        [Required]
        public string Description { get; set; }
    }
}