// ARQUIVO: ViewModels/AutenticacaoModels.cs (NOVO ARQUIVO)

using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.ViewModels
{
    // DTO para receber dados do login
    public class ModeloLogin 
    { 
        [Required]
        public string Login { get; set; } = string.Empty;
        
        [Required]
        public string Senha { get; set; } = string.Empty;
    }

    // DTO para receber dados da solicitação de reset
    public class ModeloResetSenha 
    { 
        [Required]
        public string LoginOuEmail { get; set; } = string.Empty;
    }
}