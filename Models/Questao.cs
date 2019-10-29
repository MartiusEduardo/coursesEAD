using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models
{
    [Table("questao")]
    public class Questao {
        [Key, Column("idquestao")]
        public long idQuestao { get; set; }
        [Required]
        [Column("questao")]
        [Display(Name = "Question")]
        public string Pergunta { get; set; }
        [Required]
        [Column("idsubarea")]
        public int idSubarea { get; set; }
        [Column("idarea")]
        public int idArea { get; set; }
    }
}