using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class PalavraChave
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Texto { get; set; }
    }
}