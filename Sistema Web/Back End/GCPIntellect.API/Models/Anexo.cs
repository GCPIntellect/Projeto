using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class Anexo
    {
        [Key]
        public int Id { get; set; }
        public int IdChamado { get; set; }
        [Required, StringLength(255)]
        public string NomeArquivo { get; set; }
        [Required, StringLength(500)]
        public string CaminhoArquivo { get; set; }
        [Required, StringLength(100)]
        public string TipoArquivo { get; set; }
        public long TamanhoBytes { get; set; }
        public DateTime DataUpload { get; set; }

        // Propriedade de Navegação
        public virtual Chamado? Chamado { get; set; }
    }
}