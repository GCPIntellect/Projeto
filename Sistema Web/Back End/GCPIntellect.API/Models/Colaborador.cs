using GCPIntellect.API.ViewModels; // <-- CORREÇÃO: Adicionado o using que faltava

namespace GCPIntellect.API.Models
{
    // Classe filha de UsuarioBase
    public class Colaborador : UsuarioBase
    {
        // Implementação do método abstrato
        public override string ObterTipoAcesso() => "Colaborador";

        // Implementação da regra de negócio para Colaboradores (Criação)
        // Colaborador não pode definir prioridade ou status, usa o padrão.
        public override void DefinirStatusEPrioridade(Chamado chamado, ChamadoCriacaoModel model, Prioridade prioridadePadrao, StatusChamado statusPadrao)
        {
            chamado.IdPrioridade = prioridadePadrao.Id;
            chamado.IdStatus = statusPadrao.Id;
        }

        // Implementação da regra de negócio para Colaboradores (Atualização)
        // Colaborador não pode alterar prioridade ou status.
        public override void AplicarAtualizacao(Chamado chamado, ChamadoAtualizacaoModel model)
        {
            // Intencionalmente vazio. Colaborador só pode mudar Título, Descrição, Tipo e Categoria.
        }
    }
}