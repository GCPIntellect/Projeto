using GCPIntellect.API.Data;
using GCPIntellect.API.Models;
using GCPIntellect.API.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace GCPIntellect.API.Services
{
    /// <summary>
    /// Serviço responsável por formatar e enviar emails usando SMTP.
    /// </summary>
    public class ServicoEmail : IServicoEmail
    {
        private readonly IConfiguration _configuracao;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public ServicoEmail(IConfiguration configuracao)
        {
            _configuracao = configuracao ?? throw new ArgumentNullException(nameof(configuracao));

            // Lê as configurações do appsettings (agora do appsettings.Development.json)
            _smtpServer = configuracao.GetValue<string>("Email:SmtpServer")
                ?? throw new InvalidOperationException("SMTP Server não configurado");

            // CORREÇÃO: A chave no JSON é "SmtpPort", não "Port"
            var smtpPortStr = configuracao.GetValue<string>("Email:SmtpPort")
                ?? throw new InvalidOperationException("SMTP Port não configurada");
            
            if (!int.TryParse(smtpPortStr, out _smtpPort))
                throw new InvalidOperationException("SMTP Port inválida (não é um número)");

            _smtpUsername = configuracao.GetValue<string>("Email:Username")
                ?? throw new InvalidOperationException("SMTP Username não configurado");
            _smtpPassword = configuracao.GetValue<string>("Email:Password")
                ?? throw new InvalidOperationException("SMTP Password não configurado");
            _fromEmail = configuracao.GetValue<string>("Email:FromEmail")
                ?? throw new InvalidOperationException("FromEmail não configurado");
            _fromName = configuracao.GetValue<string>("Email:FromName")
                ?? throw new InvalidOperationException("FromName não configurado");
        }

        /// <summary>
        /// Busca o email do usuário do chamado e dos técnicos atribuídos.
        /// </summary>
        private async Task<List<MailboxAddress>> ObterDestinatariosAsync(Chamado chamado, ContextoBD contexto)
        {
            var destinatarios = new List<MailboxAddress>();

            // 1. Adiciona o usuário que abriu o chamado
            // (Usa a propriedade de navegação se já estiver carregada)
            var usuario = chamado.Usuario ?? await contexto.Usuarios.FindAsync(chamado.IdUsuario);
            if (usuario != null && !string.IsNullOrEmpty(usuario.Email))
            {
                destinatarios.Add(new MailboxAddress(usuario.Nome, usuario.Email));
            }

            // 2. Adiciona os técnicos atribuídos
            var tecnicos = await contexto.ChamadoTecnicos
                .Where(ct => ct.IdChamado == chamado.Id)
                .Include(ct => ct.Tecnico)
                .Select(ct => ct.Tecnico)
                .Where(t => t != null && !string.IsNullOrEmpty(t.Email))
                .ToListAsync();

            foreach (var tecnico in tecnicos)
            {
                if (tecnico != null && !string.IsNullOrEmpty(tecnico.Email))
                {
                    // Evita adicionar o mesmo email duas vezes (caso o técnico abra o chamado para si mesmo)
                    if (!destinatarios.Any(d => d.Address == tecnico.Email))
                    {
                        destinatarios.Add(new MailboxAddress(tecnico.Nome, tecnico.Email));
                    }
                }
            }

            return destinatarios;
        }

        /// <summary>
        /// Método principal para enviar um email.
        /// </summary>
        public async Task EnviarEmailChamadoAsync(Chamado chamado, string tipoNotificacao, ContextoBD contexto)
        {
            var destinatarios = await ObterDestinatariosAsync(chamado, contexto);
            if (!destinatarios.Any())
            {
                Console.WriteLine($"[ServicoEmail] Nenhum destinatário encontrado para o chamado #{chamado.Id}. Email não enviado.");
                return; // Não lança exceção, apenas loga e retorna.
            }
            
            var mensagem = new MimeMessage();
            mensagem.From.Add(new MailboxAddress(_fromName, _fromEmail));
            mensagem.To.AddRange(destinatarios);

            // Define o Assunto e o Corpo do email
            switch (tipoNotificacao.ToLower())
            {
                case "novo":
                    ConfigurarEmailNovoChamado(mensagem, chamado);
                    break;
                case "atualizado":
                    ConfigurarEmailAtualizacaoChamado(mensagem, chamado);
                    break;
                case "fechado":
                    ConfigurarEmailFechamentoChamado(mensagem, chamado);
                    break;
                default:
                    throw new ArgumentException("Tipo de notificação inválido", nameof(tipoNotificacao));
            }

            // Conecta e envia o email
            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
            await client.SendAsync(mensagem);
            await client.DisconnectAsync(true);
        }

        // --- Métodos Privados de Template ---

        private void ConfigurarEmailNovoChamado(MimeMessage mensagem, Chamado chamado)
        {
            mensagem.Subject = $"Novo Chamado #{chamado.Id} (Prioridade: {chamado.Prioridade?.Nome}) - {chamado.Titulo}";
            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h1>Novo Chamado Criado: #{chamado.Id}</h1>
                    <p>Um novo chamado foi aberto no sistema e requer atenção.</p>
                    <hr>
                    <p><strong>Título:</strong> {chamado.Titulo}</p>
                    <p><strong>Solicitante:</strong> {chamado.Usuario?.Nome}</p>
                    <p><strong>Prioridade:</strong> {chamado.Prioridade?.Nome}</p>
                    <p><strong>Data de Abertura:</strong> {chamado.DataAbertura:dd/MM/yyyy HH:mm}</p>
                    <hr>
                    <h3>Descrição</h3>
                    <p>{chamado.Descricao.Replace("\n", "<br>")}</p>
                "
            };
            mensagem.Body = builder.ToMessageBody();
        }

        private void ConfigurarEmailAtualizacaoChamado(MimeMessage mensagem, Chamado chamado)
        {
            mensagem.Subject = $"Atualização do Chamado #{chamado.Id} - {chamado.Titulo}";
            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h1>Atualização no Chamado #{chamado.Id}</h1>
                    <p>O chamado que você acompanha foi atualizado.</p>
                    <hr>
                    <p><strong>Título:</strong> {chamado.Titulo}</p>
                    <p><strong>Novo Status:</strong> {chamado.StatusChamado?.Nome}</p>
                    <p><strong>Prioridade:</strong> {chamado.Prioridade?.Nome}</p>
                    <p><strong>Data da Atualização:</strong> {DateTime.UtcNow:dd/MM/yyyy HH:mm}</p>
  _             "
            };
            mensagem.Body = builder.ToMessageBody();
        }

        private void ConfigurarEmailFechamentoChamado(MimeMessage mensagem, Chamado chamado)
        {
            mensagem.Subject = $"Chamado #{chamado.Id} Fechado - {chamado.Titulo}";
            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h1>Chamado #{chamado.Id} foi Fechado</h1>
                    <p>O chamado foi marcado como 'Fechado' no sistema.</p>
                    <hr>
                    <p><strong>Título:</strong> {chamado.Titulo}</p>
                    <p><strong>Data de Abertura:</strong> {chamado.DataAbertura:dd/MM/yyyy HH:mm}</p>
                    <p><strong>Data de Conclusão:</strong> {chamado.DataConclusao ?? DateTime.UtcNow:dd/MM/yyyy HH:mm}</p>
                "
            };
          Desta forma, o erro `InvalidOperationException` não deve ocorrer mais.