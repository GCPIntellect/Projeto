// ARQUIVO: Models/ChamadoMensagem.cs (VERSÃO CORRIGIDA)

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCPIntellect.API.Models
{
    public class ChamadoMensagem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Chamado")] 
        public int IdChamado { get; set; }

        [ForeignKey("Autor")] 
        public int IdUsuarioAutor { get; set; }

        [Required]
        public string Conteudo { get; set; } = string.Empty;

        public DateTime DataEnvio { get; set; } = DateTime.UtcNow;

        // Propriedades de Navegação
        public virtual Chamado? Chamado { get; set; }
        public virtual UsuarioBase? Autor { get; set; } // <-- CORREÇÃO
    }
}