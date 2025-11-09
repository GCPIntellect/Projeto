using GCPIntellect.API.ViewModels; // <-- CORREÇÃO: Adicionado o using que faltava

namespace GCPIntellect.API.Models
{
    // Classe filha de UsuarioBase
    public class Tecnico : UsuarioBase
    {
        // Implementação do método abstrato
        public override string ObterTipoAcesso() => "Tecnico";

        // Implementação da regra de negócio para Tecnicos (Criação)
        public override void DefinirStatusEPrioridade(Chamado chamado, ChamadoCriacaoModel model, Prioridade prioridadePadrao, StatusChamado statusPadrao)
        {
            chamado.IdPrioridade = model.IdPrioridade ?? prioridadePadrao.Id;
            chamado.IdStatus = model.IdStatus ?? statusPadrao.Id;
        }

        // Implementação da regra de negócio para Tecnicos (Atualização)
        public override void AplicarAtualizacao(Chamado chamado, ChamadoAtualizacaoModel model)
        {
            if (model.IdPrioridade.HasValue)
            {
                chamado.IdPrioridade = model.IdPrioridade.Value;
            }
            if (model.IdStatus.HasValue)
            {
                chamado.IdStatus = model.IdStatus.Value;
            }
        }
    }
}