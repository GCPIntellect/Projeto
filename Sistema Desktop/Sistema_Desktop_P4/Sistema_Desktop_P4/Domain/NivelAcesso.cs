using System;

namespace Sistema_Desktop_P4.Domain
{
   
    /// Define os níveis de acesso/perfis de usuário do sistema.

    public enum NivelAcesso
    {

        /// Colaborador - Pode apenas abrir chamados e visualizar seus próprios chamados.
       
        Colaborador = 1,

       
        /// Técnico - Pode visualizar todos os chamados, atualizar status e responder.
        
        Tecnico = 2,

      
        /// Administrador - Acesso total ao sistema, incluindo gerenciamento de usuários.
        
        Administrador = 3
    }

    
    /// Extensões para o enum NivelAcesso.
    
    public static class NivelAcessoExtensions
    {
       
        /// Retorna a descrição textual do nível de acesso.
       
        public static string ObterDescricao(this NivelAcesso nivel)
        {
            switch (nivel)
            {
                case NivelAcesso.Colaborador:
                    return "Colaborador";
                case NivelAcesso.Tecnico:
                    return "Técnico";
                case NivelAcesso.Administrador:
                    return "Administrador";
                default:
                    return "Desconhecido";
            }
        }

        
        /// Retorna uma breve descrição das permissões do nível.
        
        public static string ObterPermissoes(this NivelAcesso nivel)
        {
            switch (nivel)
            {
                case NivelAcesso.Colaborador:
                    return "Abrir e visualizar próprios chamados";
                case NivelAcesso.Tecnico:
                    return "Gerenciar e responder todos os chamados";
                case NivelAcesso.Administrador:
                    return "Acesso total ao sistema";
                default:
                    return "";
            }
        }
    }
}
