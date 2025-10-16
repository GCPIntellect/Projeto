using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class NotificacaoFila
    {
        [Key]
        public int Id { get; set; }
        public int IdChamado { get; set; }
        [Required, StringLength(5)]
        public string TipoNotificacao { get; set; }
        [Required, StringLength(255)]
        public string Destinatario { get; set; }
        [StringLength(300)]
        public string? Assunto { get; set; }
        [Required]
        public string Conteudo { get; set; }
        [Required, StringLength(15)]
        public string Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataEnvio { get; set; }
        public int Tentativas { get; set; }
        public string? MensagemErro { get; set; }

        // Propriedade de Navegação
        public virtual Chamado? Chamado { get; set; }
    }
}