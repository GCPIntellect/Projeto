// ARQUIVO: Models/TabelasAuxiliares.cs (Antigo LookupModels.cs)

using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Nome { get; set; } = string.Empty;
    }

    public class Tipo
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Nome { get; set; } = string.Empty;
    }

    public class StatusChamado
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Nome { get; set; } = string.Empty;
    }

    public class Prioridade
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Nome { get; set; } = string.Empty;
    }
}