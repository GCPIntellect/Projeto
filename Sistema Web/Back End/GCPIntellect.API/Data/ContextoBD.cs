using GCPIntellect.API.Models;
using Microsoft.EntityFrameworkCore;
using GCPIntellect.API.Data.Configuracoes; // Namespace novo
using GCPIntellect.API.Models.Enums; // Para os Enums

namespace GCPIntellect.API.Data
{
    public class ContextoBD : DbContext
    {
        public ContextoBD(DbContextOptions<ContextoBD> options) : base(options)
        {
        }

        // --- TABELAS DE USUÁRIOS (TPH) ---
        public DbSet<UsuarioBase> Usuarios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }

        // --- TABELAS DE CHAMADOS ---
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<Anexo> Anexos { get; set; }
        public DbSet<ChamadoMensagem> ChamadoMensagens { get; set; }
        public DbSet<ChamadoTecnico> ChamadoTecnicos { get; set; }

        // --- TABELAS AUXILIARES (LOOKUP) ---
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<StatusChamado> StatusChamados { get; set; }
        public DbSet<Prioridade> Prioridades { get; set; }

        // --- TABELAS MÓDULO IA ---
        public DbSet<BaseConhecimento> BaseConhecimentos { get; set; }
        public DbSet<PalavraChave> PalavrasChave { get; set; }
        public DbSet<BaseConhecimentoPalavraChave> BaseConhecimentoPalavrasChave { get; set; }
        public DbSet<ConsultaIA> ConsultasIA { get; set; }

        // --- TABELAS MÓDULO RELATÓRIOS ---
        public DbSet<DefinicaoRelatorio> DefinicoesRelatorios { get; set; }
        public DbSet<ExecucaoRelatorio> ExecucoesRelatorios { get; set; }

        // --- TABELAS MÓDULO NOTIFICAÇÕES ---
        public DbSet<NotificacaoFila> NotificacoesFila { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. CONFIGURAÇÃO DO TPH (TABLE-PER-HIERARCHY) PARA USUÁRIOS ---
            modelBuilder.Entity<UsuarioBase>()
                .ToTable("Usuario")
                // CORREÇÃO: O nome da coluna no seu SQL é "Discriminator"
                .HasDiscriminator<string>("Discriminator") 
                .HasValue<Administrador>("Administrador")
                .HasValue<Tecnico>("Tecnico")
                .HasValue<Colaborador>("Colaborador");

            // --- 2. MAPEAMENTO EXPLÍCITO DAS OUTRAS TABELAS ---
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
            modelBuilder.Entity<BaseConhecimentoPalavraChave>().ToTable("BaseConhecimento_PalavraChave");
            modelBuilder.Entity<ConsultaIA>().ToTable("ConsultaIA");
            modelBuilder.Entity<DefinicaoRelatorio>().ToTable("RelatorioDefinicao");
            modelBuilder.Entity<ExecucaoRelatorio>().ToTable("RelatorioExecucao");
            modelBuilder.Entity<NotificacaoFila>().ToTable("NotificacaoFila");

            // --- 3. CONFIGURAÇÃO DE CHAVES COMPOSTAS ---
            modelBuilder.Entity<ChamadoTecnico>()
                .HasKey(ct => new { ct.IdChamado, ct.IdTecnico });

            modelBuilder.Entity<BaseConhecimentoPalavraChave>()
                .HasKey(bcp => new { bcp.IdBaseConhecimento, bcp.IdPalavraChave });

            // --- 4. CONFIGURAÇÕES ADICIONAIS (EX: ENUMS) ---
            // Converte o Enum de Notificação para String no banco
            modelBuilder.Entity<NotificacaoFila>()
                .Property(n => n.Status)
                .HasConversion<string>()
                .HasMaxLength(15);
            
            modelBuilder.Entity<NotificacaoFila>()
                .Property(n => n.TipoNotificacao)
                .HasConversion<string>()
                .HasMaxLength(5);

            // Aplicar configurações de arquivos separados (se existirem, como no exemplo da IA)
            // (O código da IA não incluiu o arquivo ConfiguracoesEntidades.cs, então vou remover a chamada)
            // ConfiguracoesEntidades.ConfigurarChamado(modelBuilder);
            // ConfiguracoesEntidades.ConfigurarNotificacaoFila(modelBuilder);
            // ConfiguracoesEntidades.ConfigurarChamadoTecnico(modelBuilder);
        }
    }
}