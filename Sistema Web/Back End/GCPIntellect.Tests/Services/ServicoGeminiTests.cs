using Xunit;
using Moq;
using FluentAssertions;
using GCPIntellect.API.Services;
using Microsoft.Extensions.Configuration;
using System; // Adicionado para ArgumentNullException

namespace GCPIntellect.Tests.Services
{
    public class ServicoGeminiTests
    {
        private readonly Mock<IConfiguration> _mockConfiguracao;

        public ServicoGeminiTests()
        {
            _mockConfiguracao = new Mock<IConfiguration>();
            _mockConfiguracao.Setup(c => c["GoogleCloudProjectId"]).Returns("test-project-id");
            _mockConfiguracao.Setup(c => c["GoogleCloudProjectLocation"]).Returns("us-central1");
        }

        [Fact]
        public void Construtor_SemJson_DeveLancarExcecao()
        {
            // --- Arrange ---
            // Este teste assume que o arquivo JSON não está na pasta de bin do teste,
            // o que é o comportamento esperado.

            // --- Act & Assert ---
            var excecao = Record.Exception(() => new ServicoGemini(_mockConfiguracao.Object));
            excecao.Should().NotBeNull();
            excecao.Should().BeOfType<System.IO.FileNotFoundException>();
        }

        [Fact]
        public void Construtor_SemProjectId_DeveLancarArgumentNullException()
        {
            // --- Arrange ---
            _mockConfiguracao.Setup(c => c["GoogleCloudProjectId"]).Returns((string)null);

            // --- Act & Assert ---
            var excecao = Record.Exception(() => new ServicoGemini(_mockConfiguracao.Object));
            excecao.Should().NotBeNull();
            excecao.Should().BeOfType<ArgumentNullException>();
        }
    }
}