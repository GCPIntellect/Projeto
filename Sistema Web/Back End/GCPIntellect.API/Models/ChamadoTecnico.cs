namespace GCPIntellect.API.Models
{
    // Tabela de Junção. A chave primária é composta e será definida no DbContext.
    public class ChamadoTecnico
    {
        public int IdChamado { get; set; }
        public int IdTecnico { get; set; }

        // Propriedades de Navegação
        public virtual Chamado? Chamado { get; set; }
        public virtual Usuario? Tecnico { get; set; }
    }
}