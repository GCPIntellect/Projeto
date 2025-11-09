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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();

            // Aplica o tema escuro
            ThemeManager.AplicarTema(this);
            ThemeManager.EstilizarBotaoPrimario(this.btnAcessar);

            // Atribuições de eventos (garante que os nomes batem com o Designer)
            this.txtSenha.TextChanged += txtSenha_TextChanged;
            this.btnAcessar.Click += btnAcessar_Click;

            // Usabilidade: ENTER aciona o botão Acessar
            this.AcceptButton = this.btnAcessar;

            // Inicializa visibilidade dos labels conforme o conteúdo atual dos TextBoxes
            this.labelUser.Visible = string.IsNullOrEmpty(this.txtUsuario.Text);
            this.labelSenha.Visible = string.IsNullOrEmpty(this.txtSenha.Text);
        }

        private void Login_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Esconde labelUser assim que o usuário começar a digitar.
            labelUser.Visible = string.IsNullOrEmpty(txtUsuario.Text);
        }

        private void txtSenha_TextChanged(object sender, EventArgs e)
        {
            // Esconde labelSenha quando o usuario começar a digitar.
            labelSenha.Visible = string.IsNullOrEmpty(txtSenha.Text);
        }

        private void Login_Load_1(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void btnAcessar_Click(object sender, EventArgs e)
        {
            // 1. Coleta os dados digitados
            string usuario = txtUsuario.Text.ToLower().Trim();
            string senha = txtSenha.Text;

            // 2. Lógica de Autenticação Simples com diferentes níveis
            // Em produção, isso viria da API
            NivelAcesso nivelAcesso;
            string nomeCompleto;
            int userId;
            string email;

            if (usuario == "admin" && senha == "admin123")
            {
                nivelAcesso = NivelAcesso.Administrador;
                nomeCompleto = "Administrador do Sistema";
                userId = 1;
                email = "admin@sistema.com";
            }
            else if (usuario == "tecnico" && senha == "tec123")
            {
                nivelAcesso = NivelAcesso.Tecnico;
                nomeCompleto = "João Silva - Técnico";
                userId = 2;
                email = "tecnico@sistema.com";
            }
            else if (usuario == "colaborador" && senha == "colab123")
            {
                nivelAcesso = NivelAcesso.Colaborador;
                nomeCompleto = "Maria Santos - Colaborador";
                userId = 3;
                email = "colaborador@sistema.com";
            }
            else
            {
                MessageBox.Show(
                    "Usuário ou senha incorretos.\n\n" +
                    "Usuários de teste:\n" +
                    "• admin / admin123 (Administrador)\n" +
                    "• tecnico / tec123 (Técnico)\n" +
                    "• colaborador / colab123 (Colaborador)",
                    "Erro de Login",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtSenha.Clear();
                txtUsuario.Focus();
                return;
            }

            // Salva informações da sessão
            SessionManager.NomeUsuario = nomeCompleto;
            SessionManager.UsuarioId = userId;
            SessionManager.NivelAcessoUsuario = nivelAcesso;
            SessionManager.Email = email;
            SessionManager.AccessToken = "token_simulado_" + DateTime.Now.Ticks;

            MessageBox.Show(
                $"Login realizado com sucesso!\n\n" +
                $"Bem-vindo, {nomeCompleto}\n" +
                $"Nível de acesso: {nivelAcesso.ObterDescricao()}\n" +
                $"Permissões: {nivelAcesso.ObterPermissoes()}",
                "Bem-vindo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Abre o formulário principal
            this.Hide();
            using (var frmPrincipal = new FrmPrincipal())
            {
                frmPrincipal.ShowDialog();
            }
            this.Close();
        }
    }
}
