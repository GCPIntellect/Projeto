// ARQUIVO: Models/ExecucaoRelatorio.cs (Antigo RelatorioExecucao.cs)

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCPIntellect.API.Models
{
    public class ExecucaoRelatorio
    {
        [Key]
        public Guid Id { get; set; }
        
        [ForeignKey("DefinicaoRelatorio")] // Renomeado
        public int IdRelatorioDefinicao { get; set; }
        
        [ForeignKey("UsuarioGerador")]
        public int IdUsuarioGerador { get; set; }
        
        public DateTime DataGeracao { get; set; }
        public string? Parametros { get; set; }
        [Required, StringLength(10)]
        public string FormatoSaida { get; set; } = string.Empty;
        [StringLength(500)]
        public string? CaminhoArquivo { get; set; }
        
        // Propriedades de Navegação
        public virtual DefinicaoRelatorio? DefinicaoRelatorio { get; set; } // Renomeado
        public virtual UsuarioBase? UsuarioGerador { get; set; } // <-- CORREÇÃO
    }
}