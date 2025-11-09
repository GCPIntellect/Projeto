// ARQUIVO: Models/ConsultaIA.cs (VERSÃO CORRIGIDA)

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCPIntellect.API.Models
{
    public class ConsultaIA
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string IdSessao { get; set; } = string.Empty;
        
        [ForeignKey("Usuario")]
        public int? IdUsuario { get; set; }

        [ForeignKey("BaseConhecimentoSugerido")]
        public int? IdBaseConhecimentoSugerido { get; set; }

        [ForeignKey("ChamadoGerado")]
        public int? ChamadoGeradoId { get; set; }
        
        [Required]
        public string PerguntaUsuario { get; set; } = string.Empty;

        [Required]
        public string RespostaGerada { get; set; } = string.Empty;

        public DateTime DataConsulta { get; set; } = DateTime.UtcNow;
        
        public bool? FoiUtil { get; set; }

        // Propriedades de Navegação
        public virtual UsuarioBase? Usuario { get; set; } // <-- CORREÇÃO
        public virtual BaseConhecimento? BaseConhecimentoSugerido { get; set; }
        public virtual Chamado? ChamadoGerado { get; set; }
    }
}