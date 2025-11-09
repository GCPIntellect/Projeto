namespace GCPIntellect.API.Models.Base
{
    public abstract class EntidadeAuditavel
    {
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
        public int IdUsuarioCriador { get; set; }
        public int? IdUsuarioAtualizador { get; set; }

        public virtual void AtualizarAuditoria(int idUsuario)
        {
            IdUsuarioAtualizador = idUsuario;
            DataAtualizacao = DateTime.UtcNow;
        }
    }
}