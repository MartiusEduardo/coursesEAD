using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models{
    [Table("topic")]
    public class Topic{
        [Key, Column("idtopic")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idTopic { get; set; }
        [Column("idcategory")]
        [Required]
        public int idCategory { get; set; }
        [Required]
        [Column("iduser")]
        public string idUser { get; set; }
        [Column("name")]
        [Required]
        public string Name { get; set; }
        [Column("description")]
        [Required]
        public string Description { get; set; }
        [Column("dateregister")]
        [Required]
        public DateTime DateRegister { get; set; }
    }
}