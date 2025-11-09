using System;
using System.Windows.Forms;
using Sistema_Desktop_P4.Domain;
using Sistema_Desktop_P4.Infrastructure;

namespace Sistema_Desktop_P4
{
    public partial class FrmEditarUsuario : Form
    {
        private UsuarioDto usuarioAtual;
        private bool isNovoUsuario;

        public FrmEditarUsuario(UsuarioDto usuario)
        {
            InitializeComponent();

            // Aplica tema
            ThemeManager.AplicarTema(this);
            ThemeManager.EstilizarBotaoPrimario(btnSalvar);
            ThemeManager.EstilizarBotaoSecundario(btnCancelar);

            usuarioAtual = usuario;
            isNovoUsuario = usuario == null;

            // Popula ComboBox de níveis de acesso
            cmbNivelAcesso.Items.Add(new { Text = "Colaborador", Value = NivelAcesso.Colaborador });
            cmbNivelAcesso.Items.Add(new { Text = "Técnico", Value = NivelAcesso.Tecnico });
            cmbNivelAcesso.Items.Add(new { Text = "Administrador", Value = NivelAcesso.Administrador });
            cmbNivelAcesso.DisplayMember = "Text";
            cmbNivelAcesso.ValueMember = "Value";

            if (isNovoUsuario)
            {
                lblTitulo.Text = "Novo Usuário";
                this.Text = "Criar Novo Usuário";
                cmbNivelAcesso.SelectedIndex = 0; // Colaborador por padrão
                lblSenha.Text = "Senha:*";
            }
            else
            {
                lblTitulo.Text = "Editar Usuário";
                this.Text = "Editar Usuário";
                txtNome.Text = usuario.Nome;
                txtEmail.Text = usuario.Email;
                chkAtivo.Checked = usuario.Ativo;
                cmbNivelAcesso.SelectedIndex = (int)usuario.NivelAcesso - 1;
                lblSenha.Text = "Senha: (deixe em branco para manter a atual)";
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome é obrigatório.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("O email é obrigatório.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (!txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Digite um email válido.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            if (isNovoUsuario && string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("A senha é obrigatória para novos usuários.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSenha.Focus();
                return;
            }

            if (cmbNivelAcesso.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione o nível de acesso.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbNivelAcesso.Focus();
                return;
            }

            // TODO: Aqui você chamaria a API para salvar/atualizar o usuário
            // Em produção, enviaria os dados para o backend

            var nivelSelecionado = (dynamic)cmbNivelAcesso.SelectedItem;
            var novoNivel = (NivelAcesso)nivelSelecionado.Value;

            if (isNovoUsuario)
            {
                MessageBox.Show(
                    $"Usuário '{txtNome.Text}' criado com sucesso!\n\n" +
                    $"Email: {txtEmail.Text}\n" +
                    $"Nível: {novoNivel.ObterDescricao()}",
                    "Usuário Criado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    $"Usuário '{txtNome.Text}' atualizado com sucesso!",
                    "Usuário Atualizado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
