using GCPIntellect.API.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCPIntellect.API.Models
{
    [Table("NotificacaoFila")]
    public class NotificacaoFila
    {
        [Key]
        public int Id { get; set; }

        public int? IdChamado { get; set; }

        [Required]
        public TipoNotificacao TipoNotificacao { get; set; }

        [Required]
        [StringLength(255)]
        public string Destinatario { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Assunto { get; set; }

        [Required]
        public string Conteudo { get; set; } = string.Empty;

        [Required]
        public StatusNotificacao Status { get; set; } = StatusNotificacao.Pendente;

        [Required]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataEnvio { get; set; }

        [Required]
        public int Tentativas { get; set; } = 0;

        public string? MensagemErro { get; set; }

        [ForeignKey("IdChamado")]
        public virtual Chamado? Chamado { get; set; }
    }
}