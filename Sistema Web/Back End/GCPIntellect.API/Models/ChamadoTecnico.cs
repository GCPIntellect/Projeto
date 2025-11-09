// ARQUIVO: Models/ChamadoTecnico.cs (VERSÃO CORRIGIDA)

using System.ComponentModel.DataAnnotations.Schema;

namespace GCPIntellect.API.Models
{
    public class ChamadoTecnico
    {
        [ForeignKey("Chamado")]
        public int IdChamado { get; set; }
        
        [ForeignKey("Tecnico")]
        public int IdTecnico { get; set; }

        // Propriedades de Navegação
        public virtual Chamado? Chamado { get; set; }
        public virtual UsuarioBase? Tecnico { get; set; } // <-- CORREÇÃO
    }
}