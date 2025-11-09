using GCPIntellect.API.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography; // Importado para o SHA256
using System.Text; // Importado para o Encoding.UTF8

namespace GCPIntellect.API.Models
{
    // Esta é a classe pai abstrata (TPH)
    [Table("Usuario")]
    public abstract class UsuarioBase
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required, StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] SenhaHash { get; set; } = System.Array.Empty<byte>();

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        
        public bool Ativo { get; set; } = true;
        
        [StringLength(20)]
        public string? Telefone { get; set; }

        // --- MÉTODOS DE MANIPULAÇÃO DE SENHA (MOVEMOS DO ANTIGO AuthController PARA CÁ) ---

        /// <summary>
        /// Define o SenhaHash a partir de uma senha de texto plano.
        /// (Usado ao criar ou resetar a senha de um usuário).
        /// </summary>
        public void DefinirSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                throw new ArgumentException("A senha não pode ser vazia.");

            using (var sha256 = SHA256.Create())
            {
                this.SenhaHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha.Trim()));
            }
        }

        /// <summary>
        /// Compara uma senha em texto plano com o hash armazenado nesta instância do usuário.
        /// (Usado pelo AuthController durante o login).
        /// </summary>
        public bool VerificarSenha(string senhaDigitada)
        {
            if (string.IsNullOrEmpty(senhaDigitada) || this.SenhaHash == null || this.SenhaHash.Length == 0)
                return false;

            using (var sha256 = SHA256.Create())
            {
                var hashSenhaDigitada = sha256.ComputeHash(Encoding.UTF8.GetBytes(senhaDigitada.Trim()));
                // Compara os dois arrays de bytes
                return hashSenhaDigitada.SequenceEqual(this.SenhaHash);
            }
        }
        
        // --- MÉTODOS ABSTRATOS (POLIMORFISMO) ---

        /// <summary>
        /// Retorna o nome do "Role" (Perfil) para o Token JWT.
        /// </summary>
        public abstract string ObterTipoAcesso();

        /// <summary>
        /// Define o Status e a Prioridade de um NOVO chamado com base no tipo de usuário.
        /// </summary>
        public abstract void DefinirStatusEPrioridade(Chamado chamado, ChamadoCriacaoModel model, Prioridade prioridadePadrao, StatusChamado statusPadrao);
        
        /// <summary>
        /// Aplica atualizações em um chamado existente com base no tipo de usuário.
        /// </summary>
        public abstract void AplicarAtualizacao(Chamado chamado, ChamadoAtualizacaoModel model);
    }
}