using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class ChamadoMensagem
    {
        [Key]
        public int Id { get; set; }
        public int IdChamado { get; set; }
        public int IdUsuarioAutor { get; set; }
        [Required]
        public string Conteudo { get; set; }
        public DateTime DataEnvio { get; set; }

        // Propriedades de Navegação
        public virtual Chamado? Chamado { get; set; }
        public virtual Usuario? Autor { get; set; }
    }
}