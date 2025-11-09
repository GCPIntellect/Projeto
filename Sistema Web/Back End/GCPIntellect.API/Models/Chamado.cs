// ARQUIVO: Models/Chamado.cs (VERSÃO CORRIGIDA)

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GCPIntellect.API.Models.Base;

namespace GCPIntellect.API.Models
{
    public class Chamado : EntidadeAuditavel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey("StatusChamado")]
        public int IdStatus { get; set; }

        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }

        [ForeignKey("Tipo")]
        public int IdTipo { get; set; }

        [ForeignKey("Prioridade")]
        public int IdPrioridade { get; set; }
        
        public DateTime DataAbertura { get; set; }
        public DateTime? DataConclusao { get; set; }
        
        [Required, StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        // --- Propriedades de Navegação ---
        public virtual UsuarioBase? Usuario { get; set; } // <-- CORREÇÃO
        public virtual StatusChamado? StatusChamado { get; set; }
        public virtual Categoria? Categoria { get; set; }
        public virtual Tipo? Tipo { get; set; }
        public virtual Prioridade? Prioridade { get; set; }
        
        public virtual ICollection<Anexo> Anexos { get; set; } = new List<Anexo>();
        public virtual ICollection<ChamadoMensagem> Mensagens { get; set; } = new List<ChamadoMensagem>();
        public virtual ICollection<ChamadoTecnico> TecnicosAtribuidos { get; set; } = new List<ChamadoTecnico>();
    }
}