using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCPIntellect.API.Models
{
    public class Anexo
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Chamado")]
        public int IdChamado { get; set; }
        [Required, StringLength(255)]
        public string NomeArquivo { get; set; } = string.Empty;
        [Required, StringLength(500)]
        public string CaminhoArquivo { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string TipoArquivo { get; set; } = string.Empty;
        public long TamanhoBytes { get; set; }
        public DateTime DataUpload { get; set; }
        public virtual Chamado? Chamado { get; set; }
    }
}