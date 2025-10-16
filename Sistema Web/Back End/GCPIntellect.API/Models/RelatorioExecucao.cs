using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class RelatorioExecucao
    {
        [Key]
        public Guid Id { get; set; }
        public int IdRelatorioDefinicao { get; set; }
        public int IdUsuarioGerador { get; set; }
        public DateTime DataGeracao { get; set; }
        public string? Parametros { get; set; }
        [Required, StringLength(10)]
        public string FormatoSaida { get; set; }
        [StringLength(500)]
        public string? CaminhoArquivo { get; set; }

        // Propriedades de Navegação
        public virtual RelatorioDefinicao? RelatorioDefinicao { get; set; }
        public virtual Usuario? UsuarioGerador { get; set; }
    }
}