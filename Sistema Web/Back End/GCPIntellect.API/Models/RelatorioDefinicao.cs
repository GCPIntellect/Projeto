using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class RelatorioDefinicao
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(150)]
        public string Nome { get; set; }
        [StringLength(500)]
        public string? Descricao { get; set; }
        [Required, StringLength(100)]
        public string IdentificadorLogica { get; set; }
        [Required, StringLength(100)]
        public string PapeisPermitidos { get; set; }
        public bool Ativo { get; set; }
    }
}