// ARQUIVO: Data/ApplicationDbContext.cs (VERSÃO FINAL RECOMENDADA)

using GCPIntellect.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GCPIntellect.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Mapeamento das Tabelas (DbSet) - Nomes no PLURAL, seguindo a convenção do C#
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<StatusChamado> StatusChamados { get; set; }
        public DbSet<Prioridade> Prioridades { get; set; }
        public DbSet<Anexo> Anexos { get; set; }
        public DbSet<ChamadoTecnico> ChamadoTecnicos { get; set; }
        public DbSet<ChamadoMensagem> ChamadoMensagens { get; set; }
        public DbSet<BaseConhecimento> BaseConhecimentos { get; set; }
        public DbSet<PalavraChave> PalavrasChave { get; set; }
        public DbSet<BaseConhecimento_PalavraChave> BaseConhecimento_PalavrasChave { get; set; }
        public DbSet<ConsultaIA> ConsultasIA { get; set; }
        public DbSet<RelatorioDefinicao> RelatorioDefinicoes { get; set; }
        public DbSet<RelatorioExecucao> RelatorioExecucoes { get; set; }
        public DbSet<NotificacaoFila> NotificacaoFilas { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- MAPEAMENTO EXPLÍCITO DOS NOMES DAS TABELAS (A SOLUÇÃO) ---
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Chamado>().ToTable("Chamado");
            modelBuilder.Entity<Categoria>().ToTable("Categoria");
            modelBuilder.Entity<Tipo>().ToTable("Tipo");
            modelBuilder.Entity<StatusChamado>().ToTable("StatusChamado");
            modelBuilder.Entity<Prioridade>().ToTable("Prioridade");
            modelBuilder.Entity<Anexo>().ToTable("Anexo");
            modelBuilder.Entity<ChamadoTecnico>().ToTable("ChamadoTecnico");
            modelBuilder.Entity<ChamadoMensagem>().ToTable("ChamadoMensagem");
            modelBuilder.Entity<BaseConhecimento>().ToTable("BaseConhecimento");
            modelBuilder.Entity<PalavraChave>().ToTable("PalavraChave");
            modelBuilder.Entity<BaseConhecimento_PalavraChave>().ToTable("BaseConhecimento_PalavraChave");
            modelBuilder.Entity<ConsultaIA>().ToTable("ConsultaIA");
            modelBuilder.Entity<RelatorioDefinicao>().ToTable("RelatorioDefinicao");
            modelBuilder.Entity<RelatorioExecucao>().ToTable("RelatorioExecucao");
            modelBuilder.Entity<NotificacaoFila>().ToTable("NotificacaoFila");

            // Configuração das chaves primárias compostas
            modelBuilder.Entity<ChamadoTecnico>()
                .HasKey(ct => new { ct.IdChamado, ct.IdTecnico });

            modelBuilder.Entity<BaseConhecimento_PalavraChave>()
                .HasKey(bcp => new { bcp.IdBaseConhecimento, bcp.IdPalavraChave });
        }
    }
}