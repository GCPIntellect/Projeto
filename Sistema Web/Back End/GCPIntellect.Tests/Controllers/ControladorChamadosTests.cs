using Xunit;
using Moq;
using FluentAssertions;
using GCPIntellect.API.Controllers;
using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using GCPIntellect.API.Services.Interfaces; // <-- CORREÇÃO: Usando a interface correta
using GCPIntellect.API.ViewModels; // <-- ADICIONADO: Para os ViewModels
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GCPIntellect.Tests.Controllers
{
    public class ControladorChamadosTests // Classe Renomeada
    {
        private readonly Mock<ContextoBD> _mockContexto;
        private readonly Mock<IServicoEmail> _mockServicoEmail; // <-- CORREÇÃO
        private readonly ControladorChamados _controlador; // <-- CORREÇÃO
        private readonly DbContextOptions<ContextoBD> _opcoesDb;

        // Helper para criar o DbContext em memória
        private ContextoBD CriarContextoEmMemoria()
        {
            var options = new DbContextOptionsBuilder<ContextoBD>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // DB único por teste
                .Options;
            var context = new ContextoBD(options);
            context.Database.EnsureCreated();
            return context;
        }

        // Helper para mockar o usuário logado
        private void MockUsuarioLogado(string id, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controlador.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        public ControladorChamadosTests()
        {
            // Mock do DbContext é complexo, é melhor usar o InMemoryDatabase
            _opcoesDb = new DbContextOptionsBuilder<ContextoBD>()
                .UseInMemoryDatabase(databaseName: "TestDB_Chamados")
                .Options;
            
            _mockContexto = new Mock<ContextoBD>(_opcoesDb);
            _mockServicoEmail = new Mock<IServicoEmail>();
            
            // Instancia o controlador real, mas com dependências mockadas/em memória
            _controlador = new ControladorChamados(new ContextoBD(_opcoesDb), _mockServicoEmail.Object);

            // Setup padrão do usuário logado
            MockUsuarioLogado("1", "Colaborador");
        }

        [Fact]
        public async Task ObterChamados_ComoColaborador_DeveRetornarApenasSeusChamados()
        {
            // --- Arrange ---
            await using var context = new ContextoBD(_opcoesDb);
            var usuario1 = new Colaborador { Id = 1, Nome = "Usuario 1", Login = "u1", Email = "u1@t.com" };
            var usuario2 = new Colaborador { Id = 2, Nome = "Usuario 2", Login = "u2", Email = "u2@t.com" };
            var status = new StatusChamado { Id = 1, Nome = "Aberto" };
            var prio = new Prioridade { Id = 1, Nome = "Baixa" };

            context.Usuarios.AddRange(usuario1, usuario2);
            context.StatusChamados.Add(status);
            context.Prioridades.Add(prio);

            context.Chamados.AddRange(
                new Chamado { Id = 1, Titulo = "Chamado 1", Descricao = "D1", IdUsuario = 1, IdStatus = 1, IdPrioridade = 1, IdCategoria = 1, IdTipo = 1 },
                new Chamado { Id = 2, Titulo = "Chamado 2", Descricao = "D2", IdUsuario = 2, IdStatus = 1, IdPrioridade = 1, IdCategoria = 1, IdTipo = 1 }
            );
            await context.SaveChangesAsync();

            var controller = new ControladorChamados(context, _mockServicoEmail.Object);
            MockUsuarioLogado("1", "Colaborador"); // Logado como Usuário 1
            controller.ControllerContext = _controlador.ControllerContext;

            // --- Act ---
            var resultado = await controller.ObterChamados();

            // --- Assert ---
            var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
            var valor = okResult.Value;
            
            // Extrai os dados da resposta anônima
            int totalItens = (int)valor.GetType().GetProperty("TotalItens").GetValue(valor, null);
            var dados = valor.GetType().GetProperty("Dados").GetValue(valor, null) as IEnumerable<object>;

            totalItens.Should().Be(1);
            dados.Should().HaveCount(1);
            // (Não podemos testar o 'Titulo' diretamente pois é um tipo anônimo, mas o Count(1) prova o filtro)
        }
    }
}