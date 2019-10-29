using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models
{
    [Table("Pagamento")]
    public class Pagamento
    {
        [Key, Column("idPagamento")]
        public long idPagamento { get; set; }
        [Required]
        [Column("UserId")]
        public string UserId { get; set; }
        [Required]
        [Column("idCourse")]
        public long idCourse { get; set; }
        [Column("DataVencimento")]
        [Display(Name = "Due Date")]
        public DateTime DataVencimento { get; set; }
        [Column("DataRegistro")]
        [Display(Name = "Register Date")]
        [Required]
        public DateTime DataRegistro { get; set; }
        [Column("DataEmissao")]
        [Display(Name = "Date of Issue")]
        public DateTime DataEmissao { get; set; }
        [Column("DataBaixa")]
        [Display(Name = "Low Date")]
        [Required]
        public DateTime DataBaixa { get; set; }
        [Column("ValorPagar")]
        [Display(Name = "Amount to Pay")]
        [Required]
        public double ValorPagar { get; set; }
        [Required]
        [Column("ValorPago")]
        [Display(Name = "Amount Paid")]
        public double ValorPago { get; set; }
        [Column("Course")]
        public bool Course { get; set; }
        [Column("Simulado")]
        public bool Simulado { get; set; }
        [Column("Prova")]
        public bool Prova { get; set; }
    }
}