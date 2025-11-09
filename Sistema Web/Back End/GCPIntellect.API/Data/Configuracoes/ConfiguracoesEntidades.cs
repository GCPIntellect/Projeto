using GCPIntellect.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GCPIntellect.API.Data.Configuracoes
{
    public static class ConfiguracoesEntidades
    {
        public static void ConfigurarChamado(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.StatusChamado)
                .WithMany()
                .HasForeignKey(c => c.IdStatus)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Categoria)
                .WithMany()
                .HasForeignKey(c => c.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Tipo)
                .WithMany()
                .HasForeignKey(c => c.IdTipo)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.Prioridade)
                .WithMany()
                .HasForeignKey(c => c.IdPrioridade)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public static void ConfigurarNotificacaoFila(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificacaoFila>()
                .HasOne(n => n.Chamado)
                .WithMany()
                .HasForeignKey(n => n.IdChamado)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NotificacaoFila>()
                .Property(n => n.TipoNotificacao)
                .HasConversion<string>();

            modelBuilder.Entity<NotificacaoFila>()
                .Property(n => n.Status)
                .HasConversion<string>();
        }

        public static void ConfigurarChamadoTecnico(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChamadoTecnico>()
                .HasOne(ct => ct.Chamado)
                .WithMany(c => c.TecnicosAtribuidos)
                .HasForeignKey(ct => ct.IdChamado)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChamadoTecnico>()
                .HasOne(ct => ct.Tecnico)
                .WithMany()
                .HasForeignKey(ct => ct.IdTecnico)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}