using GCPIntellect.API.Data;
using GCPIntellect.API.Models;

namespace GCPIntellect.API.Services.Interfaces
{
    /// <summary>
    /// Define o contrato para um serviço de envio de emails.
    /// </summary>
    public interface IServicoEmail
    {
        /// <summary>
        /// Envia um email formatado sobre um chamado.
        /// </summary>
        /// <param name="chamado">O chamado com os dados.</param>
        /// <param name="tipoNotificacao">O template a usar ("novo", "atualizado", "fechado").</param>
        /// <param name="contexto">O ContextoBD para buscar destinatários (usuário, técnicos).</param>
        Task EnviarEmailChamadoAsync(Chamado chamado, string tipoNotificacao, ContextoBD contexto);
    }
}