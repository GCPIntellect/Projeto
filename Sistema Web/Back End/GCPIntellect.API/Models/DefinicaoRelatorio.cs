// ARQUIVO: Models/DefinicaoRelatorio.cs (Antigo RelatorioDefinicao.cs)

using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class DefinicaoRelatorio
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(150)]
        public string Nome { get; set; } = string.Empty;
        [StringLength(500)]
        public string? Descricao { get; set; }
        [Required, StringLength(100)]
        public string IdentificadorLogica { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string PapeisPermitidos { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}