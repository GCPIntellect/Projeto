// ARQUIVO: Models/BaseConhecimento.cs (VERSÃO CORRIGIDA)

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

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

        [ForeignKey("Categoria")]
        public int? IdCategoria { get; set; }
        
        [ForeignKey("UsuarioCriador")]
        public int IdUsuarioCriador { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        
        public DateTime DataUltimaAtualizacao { get; set; } = DateTime.UtcNow;

        // --- Propriedades de Navegação ---
        public virtual Categoria? Categoria { get; set; }
        public virtual UsuarioBase? UsuarioCriador { get; set; } // <-- CORREÇÃO

        public virtual ICollection<BaseConhecimentoPalavraChave> PalavrasChave { get; set; } = new List<BaseConhecimentoPalavraChave>();
    }
}