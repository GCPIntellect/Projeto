using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.Models
{
    public class Chamado
    {
        [Key]
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdStatus { get; set; }
        public int IdCategoria { get; set; }
        public int IdTipo { get; set; }
        public int IdPrioridade { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }
        [Required, StringLength(200)]
        public string Titulo { get; set; }
        [Required]
        public string Descricao { get; set; }

        // Propriedades de Navegação (para o EF Core ligar as tabelas)
        public virtual Usuario? Usuario { get; set; }
        public virtual StatusChamado? StatusChamado { get; set; }
        public virtual Categoria? Categoria { get; set; }
        public virtual Tipo? Tipo { get; set; }
        public virtual Prioridade? Prioridade { get; set; }
        public virtual ICollection<Anexo>? Anexos { get; set; }
        public virtual ICollection<ChamadoMensagem>? Mensagens { get; set; }
        public virtual ICollection<ChamadoTecnico>? TecnicosAtribuidos { get; set; }
    }
}