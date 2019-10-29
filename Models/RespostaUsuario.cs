using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulacao.Models{
    [Table("respostausuario")]
    public class RespostaUsuario{
        [Key, Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        [Column("iduser")]
        public string idUser { get; set; }
        [Column("idquestao")]
        public long idQuestao { get; set; }
        [Column("idrespostacorreta")]
        public long idRespostaCorreta { get; set; }
        [Column("idrespostausuario")]
        public long idRespostaUsuario { get; set; }
        [Column("idprova")]
        public long idProva { get; set; }
    }
}