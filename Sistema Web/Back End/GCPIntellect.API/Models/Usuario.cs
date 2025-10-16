using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(150)]
        public string Nome { get; set; }
        [Required, StringLength(50)]
        public string Login { get; set; }
        [Required, StringLength(255)]
        public string Email { get; set; }
        [Required]
        public byte[] SenhaHash { get; set; }
        [Required, StringLength(20)]
        public string TipoAcesso { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        [StringLength(20)]
        public string? Telefone { get; set; }
    }
}