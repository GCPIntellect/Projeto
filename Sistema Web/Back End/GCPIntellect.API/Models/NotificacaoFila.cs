using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // 1. ADICIONADO: Necessário para o [ForeignKey]

namespace GCPIntellect.API.Models
{
    public class NotificacaoFila
    {
        [Key]
        public int Id { get; set; }

        // 2. ADICIONADO: Atributo [ForeignKey] 
        [ForeignKey("Chamado")]
        public int? IdChamado { get; set; }

        [Required, StringLength(5)]
        public string TipoNotificacao { get; set; } = string.Empty;

        [Required, StringLength(255)]
        public string Destinatario { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Assunto { get; set; }

        [Required]
        public string Conteudo { get; set; } = string.Empty;

        [Required, StringLength(15)]
        public string Status { get; set; } = "Pendente";

        // 3. ADICIONADO: Valor padrão para consistência
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        public DateTime? DataEnvio { get; set; }

        // 4. ADICIONADO: Valor padrão para consistência
        public int Tentativas { get; set; } = 0;

        public string? MensagemErro { get; set; }

        // Propriedade de Navegação
        public virtual Chamado? Chamado { get; set; }
    }
}