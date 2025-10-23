using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Necessário para ICollection

namespace GCPIntellect.API.Models
{
    public class BaseConhecimento
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(300)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Resposta { get; set; } = string.Empty;

        public int? IdCategoria { get; set; }
        
        public int IdUsuarioCriador { get; set; }
        
        public DateTime DataCriacao { get; set; }
        
        public DateTime DataUltimaAtualizacao { get; set; }

        // Propriedades de Navegação
        public virtual Categoria? Categoria { get; set; }
        public virtual Usuario? UsuarioCriador { get; set; }

        // --- CORREÇÃO ADICIONADA ---
        // Lista que representa a relação com a tabela de junção
        public virtual ICollection<BaseConhecimento_PalavraChave> PalavrasChave { get; set; } = new List<BaseConhecimento_PalavraChave>();
    }
}