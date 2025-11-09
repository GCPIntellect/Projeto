using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sistema_Desktop_P4.Domain;
using Sistema_Desktop_P4.Infrastructure;

namespace Sistema_Desktop_P4
{
    public partial class FrmAdministracao : Form
    {
        private List<UsuarioDto> usuariosSimulados;

        public FrmAdministracao()
        {
            InitializeComponent();

            // Aplica tema
            ThemeManager.AplicarTema(this);
            ThemeManager.EstilizarBotaoPrimario(btnNovoUsuario);
            ThemeManager.EstilizarBotaoSecundario(btnEditarUsuario);
            ThemeManager.EstilizarBotaoErro(btnExcluirUsuario);
            ThemeManager.EstilizarBotaoSecundario(btnAtualizarUsuarios);
            ThemeManager.EstilizarBotaoPrimario(btnGerarRelatorio);

            // Verifica permissão
            if (!SessionManager.EhAdministrador)
            {
                MessageBox.Show("Acesso negado! Apenas administradores podem acessar esta área.",
                              "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void FrmAdministracao_Load(object sender, EventArgs e)
        {
            CarregarUsuarios();
            CarregarEstatisticas();
        }

        private void CarregarUsuarios()
        {
            // Simulação de dados - em produção viria da API
            usuariosSimulados = new List<UsuarioDto>
            {
                new UsuarioDto { Id = 1, Nome = "Administrador do Sistema", Email = "admin@sistema.com", NivelAcesso = NivelAcesso.Administrador, Ativo = true },
                new UsuarioDto { Id = 2, Nome = "João Silva", Email = "joao.silva@empresa.com", NivelAcesso = NivelAcesso.Tecnico, Ativo = true },
                new UsuarioDto { Id = 3, Nome = "Maria Santos", Email = "maria.santos@empresa.com", NivelAcesso = NivelAcesso.Colaborador, Ativo = true },
                new UsuarioDto { Id = 4, Nome = "Pedro Costa", Email = "pedro.costa@empresa.com", NivelAcesso = NivelAcesso.Tecnico, Ativo = true },
                new UsuarioDto { Id = 5, Nome = "Ana Oliveira", Email = "ana.oliveira@empresa.com", NivelAcesso = NivelAcesso.Colaborador, Ativo = false }
            };

            dgvUsuarios.DataSource = usuariosSimulados.Select(u => new
            {
                u.Id,
                u.Nome,
                u.Email,
                Perfil = u.NivelAcesso.ObterDescricao(),
                Status = u.Ativo ? "Ativo" : "Inativo"
            }).ToList();

            // Ajusta colunas
            if (dgvUsuarios.Columns.Count > 0)
            {
                dgvUsuarios.Columns["Id"].Width = 50;
                dgvUsuarios.Columns["Nome"].Width = 200;
                dgvUsuarios.Columns["Email"].Width = 220;
                dgvUsuarios.Columns["Perfil"].Width = 120;
                dgvUsuarios.Columns["Status"].Width = 80;
            }
        }

        private void CarregarEstatisticas()
        {
            // Simulação de estatísticas
            lblTotalChamados.Text = "Total de Chamados: 127";
            lblChamadosAbertos.Text = "Chamados Abertos: 23";
            lblChamadosFechados.Text = "Chamados Fechados: 104";
            lblTotalUsuarios.Text = $"Total de Usuários: {usuariosSimulados?.Count ?? 0}";
        }

        private void btnNovoUsuario_Click(object sender, EventArgs e)
        {
            using (var form = new FrmEditarUsuario(null))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Usuário criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarUsuarios();
                }
            }
        }

        private void btnEditarUsuario_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um usuário para editar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = (int)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
            var usuario = usuariosSimulados.FirstOrDefault(u => u.Id == userId);

            if (usuario != null)
            {
                using (var form = new FrmEditarUsuario(usuario))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Usuário atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CarregarUsuarios();
                    }
                }
            }
        }

        private void btnExcluirUsuario_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um usuário para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = (int)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
            string nomeUsuario = dgvUsuarios.SelectedRows[0].Cells["Nome"].Value.ToString();

            if (userId == SessionManager.UsuarioId)
            {
                MessageBox.Show("Você não pode excluir sua própria conta!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var resultado = MessageBox.Show(
                $"Tem certeza que deseja excluir o usuário '{nomeUsuario}'?\n\nEsta ação não pode ser desfeita!",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resultado == DialogResult.Yes)
            {
                // TODO: Chamar API para excluir
                usuariosSimulados.RemoveAll(u => u.Id == userId);
                MessageBox.Show("Usuário excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarUsuarios();
                CarregarEstatisticas();
            }
        }

        private void btnAtualizarUsuarios_Click(object sender, EventArgs e)
        {
            CarregarUsuarios();
            MessageBox.Show("Lista de usuários atualizada!", "Atualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            var relatorio = new System.Text.StringBuilder();
            relatorio.AppendLine("=== RELATÓRIO DO SISTEMA ===");
            relatorio.AppendLine($"Data: {DateTime.Now:dd/MM/yyyy HH:mm}");
            relatorio.AppendLine($"Gerado por: {SessionManager.NomeUsuario}");
            relatorio.AppendLine();
            relatorio.AppendLine("--- ESTATÍSTICAS DE CHAMADOS ---");
            relatorio.AppendLine("Total de Chamados: 127");
            relatorio.AppendLine("Chamados Abertos: 23");
            relatorio.AppendLine("Chamados em Andamento: 15");
            relatorio.AppendLine("Chamados Fechados: 104");
            relatorio.AppendLine();
            relatorio.AppendLine("--- ESTATÍSTICAS DE USUÁRIOS ---");
            relatorio.AppendLine($"Total de Usuários: {usuariosSimulados.Count}");
            relatorio.AppendLine($"Administradores: {usuariosSimulados.Count(u => u.NivelAcesso == NivelAcesso.Administrador)}");
            relatorio.AppendLine($"Técnicos: {usuariosSimulados.Count(u => u.NivelAcesso == NivelAcesso.Tecnico)}");
            relatorio.AppendLine($"Colaboradores: {usuariosSimulados.Count(u => u.NivelAcesso == NivelAcesso.Colaborador)}");
            relatorio.AppendLine($"Usuários Ativos: {usuariosSimulados.Count(u => u.Ativo)}");
            relatorio.AppendLine($"Usuários Inativos: {usuariosSimulados.Count(u => !u.Ativo)}");

            MessageBox.Show(relatorio.ToString(), "Relatório do Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    // DTO para usuário
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public NivelAcesso NivelAcesso { get; set; }
        public bool Ativo { get; set; }
    }
}
