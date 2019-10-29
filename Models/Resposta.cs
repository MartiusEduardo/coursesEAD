using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models{
    public class Resposta{
        [Key, Column("idresposta")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long idResposta { get; set; }
        [Required]
        [Column("idquestao")]
        public long idQuestao { get; set; }
        [Required]
        [Column("resposta")]
        [Display(Name = "Answer")]
        public string NomeResposta { get; set; }
        [Column("correta")]
        [Display(Name = "Is the correct answer?")]
        public bool Correta { get; set; }
        [Column("explicacao")]
        [Display(Name = "Explanation - Insert the explanation only if is the correct answer.")]
        public string Explicacao { get; set; }
    }
}