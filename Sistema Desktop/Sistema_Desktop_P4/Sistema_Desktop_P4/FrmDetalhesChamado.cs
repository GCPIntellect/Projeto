using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Sistema_Desktop_P4.Domain;
using Sistema_Desktop_P4.Infrastructure;

namespace Sistema_Desktop_P4
{
    public partial class FrmDetalhesChamado : Form
    {
        private ChamadoDto chamadoAtual;
        private List<string> historico;

        public FrmDetalhesChamado(ChamadoDto chamado)
        {
            InitializeComponent();

            // Aplica tema
            ThemeManager.AplicarTema(this);
            ThemeManager.EstilizarBotaoPrimario(btnAtualizar);
            ThemeManager.EstilizarBotaoSecundario(btnFechar);

            chamadoAtual = chamado;
            historico = new List<string>();

            // Controla visibilidade baseado no nível de acesso
            groupBox2.Enabled = SessionManager.EhTecnico || SessionManager.EhAdministrador;

            if (!groupBox2.Enabled)
            {
                groupBox2.Text = "Atualizar Chamado (Disponível apenas para Técnicos/Admins)";
            }

            CarregarDadosChamado();
            CarregarHistorico();
        }

        private void CarregarDadosChamado()
        {
            lblIdValor.Text = $"ID: #{chamadoAtual.Id}";
            lblTituloValor.Text = chamadoAtual.Titulo;
            lblDescricaoValor.Text = chamadoAtual.Descricao;
            lblStatusValor.Text = $"Status: {chamadoAtual.Status}";
            lblPrioridadeValor.Text = $"Prioridade: {chamadoAtual.Prioridade}";
            lblCategoriaValor.Text = $"Categoria: {chamadoAtual.GetType().GetProperty("Categoria")?.GetValue(chamadoAtual) ?? "N/A"}";
            lblDataAberturaValor.Text = $"Aberto em: {chamadoAtual.DataAbertura:dd/MM/yyyy HH:mm}";
            lblUsuarioValor.Text = $"Solicitante: {chamadoAtual.Usuario}";

            // Seleciona status atual no combo
            cmbNovoStatus.SelectedItem = chamadoAtual.Status;
        }

        private void CarregarHistorico()
        {
            // Simulação de histórico
            historico.Add($"[{DateTime.Now.AddDays(-2):dd/MM HH:mm}] Chamado aberto por {chamadoAtual.Usuario}");
            historico.Add($"[{DateTime.Now.AddDays(-1):dd/MM HH:mm}] Status alterado para 'Em Andamento' por João Silva");
            historico.Add($"[{DateTime.Now.AddHours(-2):dd/MM HH:mm}] Resposta adicionada por João Silva");

            listBoxHistorico.Items.Clear();
            foreach (var item in historico)
            {
                listBoxHistorico.Items.Add(item);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (cmbNovoStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um status.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string novoStatus = cmbNovoStatus.SelectedItem.ToString();
            string resposta = txtResposta.Text.Trim();

            // TODO: Chamar API para atualizar chamado

            // Adiciona ao histórico
            string acao = $"[{DateTime.Now:dd/MM HH:mm}] Status alterado para '{novoStatus}' por {SessionManager.NomeUsuario}";
            if (!string.IsNullOrEmpty(resposta))
            {
                acao += $" - Nota: {resposta}";
            }
            historico.Add(acao);
            listBoxHistorico.Items.Add(acao);

            // Atualiza status atual
            chamadoAtual.Status = novoStatus;
            lblStatusValor.Text = $"Status: {novoStatus}";

            MessageBox.Show("Chamado atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Limpa campos
            txtResposta.Clear();

            // Notificação simulada
            if (SessionManager.EhTecnico || SessionManager.EhAdministrador)
            {
                MessageBox.Show($"?? Notificação enviada para {chamadoAtual.Usuario}", "Notificação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
