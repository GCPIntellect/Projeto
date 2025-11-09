using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sistema_Desktop_P4.Infrastructure;
using Sistema_Desktop_P4.Domain;

namespace Sistema_Desktop_P4
{
    public partial class FrmPrincipal : Form
    {
        private Button activeButton = null;

        public FrmPrincipal()
        {
            InitializeComponent();

            // Aplica o tema escuro customizado
            AplicarTemaCustomizado();

            this.IsMdiContainer = true;

            // Configura visibilidade dos menus baseado no nível de acesso
            ConfigurarMenusPorNivelAcesso();

            // Atualiza informações do usuário
            AtualizarInfoUsuario();

            // Atualiza status da sessão
            CarregarStatusDaSessao();

            // Seleciona Dashboard por padrão
            SelecionarBotao(btnDashboard);
        }

        private void AplicarTemaCustomizado()
        {
            // Estilo dos botões do menu quando hover
            foreach (Button btn in panelMenuItems.Controls.OfType<Button>())
            {
                btn.MouseEnter += MenuButton_MouseEnter;
                btn.MouseLeave += MenuButton_MouseLeave;
            }

            // Estilo dos botões da barra superior (minimizar, maximizar, fechar)
            btnMinimizar.MouseEnter += TopButton_MouseEnter;
            btnMinimizar.MouseLeave += TopButton_MouseLeave;
            btnMaximizar.MouseEnter += TopButton_MouseEnter;
            btnMaximizar.MouseLeave += TopButton_MouseLeave;
            btnFechar.MouseEnter += CloseButton_MouseEnter;
            btnFechar.MouseLeave += TopButton_MouseLeave;
        }

        private void MenuButton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != activeButton)
            {
                btn.BackColor = ThemeManager.CorFundoEscuro3;
            }
        }

        private void MenuButton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != activeButton)
            {
                btn.BackColor = Color.Transparent;
            }
        }

        private void TopButton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = ThemeManager.CorFundoEscuro3;
        }

        private void TopButton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.Transparent;
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            btnFechar.BackColor = ThemeManager.CorErro;
        }

        private void SelecionarBotao(Button button)
        {
            // Remove seleção anterior
            if (activeButton != null)
            {
                activeButton.BackColor = Color.Transparent;
            }

            // Aplica seleção no novo botão
            activeButton = button;
            button.BackColor = ThemeManager.CorPrimaria;
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            CarregarStatusDaSessao();
            MostrarDashboard();
        }

        private void ConfigurarMenusPorNivelAcesso()
        {
            // Todos podem abrir chamados e ver seus próprios chamados
            btnAbrirChamado.Visible = true;
            btnMeusChamados.Visible = true;

            // Apenas Técnicos e Administradores veem "Consultar Todos"
            btnConsultarChamados.Visible = SessionManager.EhTecnico || SessionManager.EhAdministrador;

            // Menu de Administração visível APENAS para Administradores
            bool isAdmin = SessionManager.EhAdministrador;
            panelAdminSeparator.Visible = isAdmin;
            btnGerenciarUsuarios.Visible = isAdmin;
            btnRelatorios.Visible = isAdmin;
        }

        private void AtualizarInfoUsuario()
        {
            lblUserName.Text = SessionManager.NomeUsuario ?? "Usuário";
            lblUserRole.Text = SessionManager.NivelAcessoUsuario.ObterDescricao();

            // Cor do perfil baseado no nível
            if (SessionManager.EhAdministrador)
                lblUserRole.ForeColor = ThemeManager.CorPrimaria;
            else if (SessionManager.EhTecnico)
                lblUserRole.ForeColor = ThemeManager.CorSucesso;
            else
                lblUserRole.ForeColor = ThemeManager.CorAlerta;
        }

        public void CarregarStatusDaSessao()
        {
            bool tokenValido = SessionManager.SessaoAtiva;
            string status = tokenValido ? "CONECTADO" : "OFFLINE";
            string usuarioExibido = tokenValido ? SessionManager.NomeUsuario : "N/A";
            string nivelAcesso = tokenValido ? SessionManager.NivelAcessoUsuario.ObterDescricao() : "N/A";

            if (this.toolStripStatusLabel1 != null)
            {
                this.toolStripStatusLabel1.Text =
                    $"  {usuarioExibido} • {nivelAcesso} • {status}";

                // Cor baseada no nível de acesso
                if (tokenValido)
                {
                    if (SessionManager.EhAdministrador)
                        this.toolStripStatusLabel1.ForeColor = ThemeManager.CorPrimaria;
                    else if (SessionManager.EhTecnico)
                        this.toolStripStatusLabel1.ForeColor = ThemeManager.CorSucesso;
                    else
                        this.toolStripStatusLabel1.ForeColor = ThemeManager.CorAlerta;
                }
                else
                {
                    this.toolStripStatusLabel1.ForeColor = ThemeManager.CorErro;
                }
            }
        }

        private void AbrirFormularioFilho(Form formFilho, string tituloPage)
        {
            // Fecha formulários MDI filhos anteriores
            foreach (Form child in this.MdiChildren)
            {
                child.Close();
            }

            // Limpa o panelContent
            panelContent.Controls.Clear();

            // Configura o novo formulário
            formFilho.TopLevel = false;
            formFilho.FormBorderStyle = FormBorderStyle.None;
            formFilho.Dock = DockStyle.Fill;
            panelContent.Controls.Add(formFilho);
            formFilho.Show();

            // Atualiza título da página
            lblPageTitle.Text = tituloPage;
        }

        private void MostrarDashboard()
        {
            panelContent.Controls.Clear();
            lblPageTitle.Text = "Dashboard";

            // Cria um painel de boas-vindas
            Panel welcomePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40)
            };

            Label welcomeLabel = new Label
            {
                Text = $"Bem-vindo, {SessionManager.NomeUsuario}! 👋",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(40, 40)
            };

            Label subtitleLabel = new Label
            {
                Text = $"Você está conectado como {SessionManager.NivelAcessoUsuario.ObterDescricao()}",
                Font = new Font("Segoe UI", 12F),
                ForeColor = ThemeManager.CorTextoSecundario,
                AutoSize = true,
                Location = new Point(40, 80)
            };

            welcomePanel.Controls.Add(welcomeLabel);
            welcomePanel.Controls.Add(subtitleLabel);
            panelContent.Controls.Add(welcomePanel);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SelecionarBotao(btnDashboard);
            MostrarDashboard();
        }

        private void menuAbrirChamado_Click(object sender, EventArgs e)
        {
            SelecionarBotao(btnAbrirChamado);
            
            if (!SessionManager.SessaoAtiva)
            {
                MessageBox.Show("Sua sessão expirou. Por favor, faça login novamente.", "Sessão Expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FrmAbrirChamado frmAbrir = new FrmAbrirChamado();
            AbrirFormularioFilho(frmAbrir, "Abrir Novo Chamado");
        }

        private void menuConsultarChamados_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;
            if (senderButton != null)
            {
                SelecionarBotao(senderButton);
            }

            if (!SessionManager.SessaoAtiva)
            {
                MessageBox.Show("Sua sessão expirou. Por favor, faça login novamente.", "Sessão Expirada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FrmConsultarChamados frmConsultar = new FrmConsultarChamados();
            string titulo = SessionManager.EhColaborador ? "Meus Chamados" : "Consultar Chamados";
            AbrirFormularioFilho(frmConsultar, titulo);
        }

        private void menuAdminUsuarios_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;
            if (senderButton != null)
            {
                SelecionarBotao(senderButton);
            }

            if (!SessionManager.EhAdministrador)
            {
                MessageBox.Show("Acesso negado! Apenas administradores podem acessar esta área.",
                              "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FrmAdministracao frmAdmin = new FrmAdministracao();
            AbrirFormularioFilho(frmAdmin, "Administração do Sistema");
        }

        private void menuSessaoLogout_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show(
                $"Deseja realmente sair da sessão de {SessionManager.NomeUsuario}?",
                "Confirmar Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                SessionManager.LimparSessao();
                MessageBox.Show("Você foi desconectado com sucesso.", "Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
                var loginForm = new login();
                loginForm.ShowDialog();
            }
        }

        private void menuSessaoSair_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja realmente sair do sistema?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                btnMaximizar.Text = "□";
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                btnMaximizar.Text = "❐";
            }
        }
    }
}