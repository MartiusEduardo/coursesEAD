using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models{
    [Table("posting")]
    public class Posting{
        [Key, Column("idposting")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idPosting { get; set; }
        [Column("idtopic")]
        public int idTopic { get; set; }
        [Required]
        [Column("iduser")]
        public string idUser { get; set; }
        [Required]
        [Column("message")]
        public string Message { get; set; }
        [Column("datepublication")]
        public DateTime DatePublication { get; set; }
    }
}