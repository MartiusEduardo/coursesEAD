using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models{
    [Table("group")]
    public class Group{
        [Key, Column("idgroup")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long idGroup { get; set; }
        [Column("idCourse")]
        [Required]
        public long idCourse { get; set; }
        [Column("name")]
        [Required]
        public string Name { get; set; }
        [Column("description")]
        [Required]
        public string Description { get; set; }
    }
}