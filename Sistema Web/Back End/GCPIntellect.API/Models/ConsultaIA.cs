using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class ConsultaIA
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string IdSessao { get; set; }
        public int? IdUsuario { get; set; }
        [Required]
        public string PerguntaUsuario { get; set; }
        [Required]
        public string RespostaGerada { get; set; }
        public DateTime DataConsulta { get; set; }
        public bool? FoiUtil { get; set; }
        public int? IdBaseConhecimentoSugerido { get; set; }
        public int? ChamadoGeradoId { get; set; }

        // Propriedades de Navegação
        public virtual Usuario? Usuario { get; set; }
        public virtual BaseConhecimento? BaseConhecimentoSugerido { get; set; }
        public virtual Chamado? ChamadoGerado { get; set; }
    }
}