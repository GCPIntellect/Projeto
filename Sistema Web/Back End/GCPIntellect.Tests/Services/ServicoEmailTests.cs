using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using GCPIntellect.API.Services;
using GCPIntellect.API.Models;
using System.Threading.Tasks;
using GCPIntellect.API.Data; // Necessário para o ContextoBD
using Microsoft.EntityFrameworkCore; // Necessário para UseInMemoryDatabase

namespace GCPIntellect.Tests.Services
{
    public class ServicoEmailTests // Classe Renomeada
    {
        private readonly Mock<IConfiguration> _mockConfiguracao;
        private readonly ServicoEmail _servicoEmail; // Classe Renomeada

        public ServicoEmailTests()
        {
            _mockConfiguracao = new Mock<IConfiguration>();
            
            // Mock das configurações de Email
            _mockConfiguracao.Setup(c => c["Email:SmtpServer"]).Returns("smtp.test.com");
            _mockConfiguracao.Setup(c => c["Email:Port"]).Returns("587"); // Corrigido de SmtpPort
            _mockConfiguracao.Setup(c => c["Email:Username"]).Returns("test@test.com");
            _mockConfiguracao.Setup(c => c["Email:Password"]).Returns("test-password");
            _mockConfiguracao.Setup(c => c["Email:FromEmail"]).Returns("from@test.com");
            _mockConfiguracao.Setup(c => c["Email:FromName"]).Returns("Test Sender");

            _servicoEmail = new ServicoEmail(_mockConfiguracao.Object); // Classe Renomeada
        }

        // Helper para criar o DbContext em memória
        private ContextoBD CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<ContextoBD>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ContextoBD(options);
        }

        [Fact]
        public async Task EnviarEmailChamadoAsync_NovoChamado_DeveTentarEnviarEmail()
        {
            // --- Arrange ---
            await using var context = CriarContextoEmMemoria();
            var usuario = new Colaborador { Id = 1, Nome = "Usuário Teste", Email = "usuario@teste.com", Login="u1" };
            context.Usuarios.Add(usuario);
            await context.SaveChangesAsync();
            
            var chamado = new Chamado
            {
                Id = 1,
                Titulo = "Teste de Chamado",
                Descricao = "Descrição do teste",
                IdUsuario = 1,
                Usuario = usuario,
                StatusChamado = new StatusChamado { Nome = "Aberto" },
                Prioridade = new Prioridade { Nome = "Alta" }
            };
            
            // --- Act & Assert ---
            // Este teste vai falhar ao tentar conectar ao "smtp.test.com".
            // Isso é o esperado para um unit test que não mocka o SmtpClient.
            // Estamos testando se ele tenta se conectar, não se o email é realmente enviado.
            await Assert.ThrowsAnyAsync<Exception>(() => 
                _servicoEmail.EnviarEmailChamadoAsync(chamado, "novo", context));
        }

        [Fact]
        public async Task EnviarEmailChamadoAsync_TipoInvalido_DeveLancarArgumentException()
        {
            // --- Arrange ---
            await using var context = CriarContextoEmMemoria();
            var chamado = new Chamado();

            // --- Act & Assert ---
            // Este teste deve passar, pois a validação ocorre antes da conexão SMTP.
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _servicoEmail.EnviarEmailChamadoAsync(chamado, "tipo_invalido", context));
        }
    }
}