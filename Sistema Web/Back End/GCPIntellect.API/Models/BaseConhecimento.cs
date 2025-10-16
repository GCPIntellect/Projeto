using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class BaseConhecimento
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(300)]
        public string Titulo { get; set; }
        [Required]
        public string Resposta { get; set; }
        public int? IdCategoria { get; set; }
        public int IdUsuarioCriador { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }

        // Propriedades de Navegação
        public virtual Categoria? Categoria { get; set; }
        public virtual Usuario? UsuarioCriador { get; set; }
    }
}