namespace Sistema_Desktop_P4
{
    partial class FrmAdministracao
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabUsuarios = new System.Windows.Forms.TabPage();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNovoUsuario = new System.Windows.Forms.Button();
            this.btnEditarUsuario = new System.Windows.Forms.Button();
            this.btnExcluirUsuario = new System.Windows.Forms.Button();
            this.btnAtualizarUsuarios = new System.Windows.Forms.Button();
            this.tabRelatorios = new System.Windows.Forms.TabPage();
            this.lblTotalChamados = new System.Windows.Forms.Label();
            this.lblChamadosAbertos = new System.Windows.Forms.Label();
            this.lblChamadosFechados = new System.Windows.Forms.Label();
            this.lblTotalUsuarios = new System.Windows.Forms.Label();
            this.btnGerarRelatorio = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabUsuarios.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabRelatorios.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(262, 32);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Painel de Administração";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabUsuarios);
            this.tabControl1.Controls.Add(this.tabRelatorios);
            this.tabControl1.Location = new System.Drawing.Point(20, 70);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(960, 530);
            this.tabControl1.TabIndex = 1;
            // 
            // tabUsuarios
            // 
            this.tabUsuarios.Controls.Add(this.dgvUsuarios);
            this.tabUsuarios.Controls.Add(this.panel1);
            this.tabUsuarios.Location = new System.Drawing.Point(4, 25);
            this.tabUsuarios.Name = "tabUsuarios";
            this.tabUsuarios.Padding = new System.Windows.Forms.Padding(3);
            this.tabUsuarios.Size = new System.Drawing.Size(952, 501);
            this.tabUsuarios.TabIndex = 0;
            this.tabUsuarios.Text = "Gerenciar Usuários";
            this.tabUsuarios.UseVisualStyleBackColor = true;
            // 
            // dgvUsuarios
            // 
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsuarios.Location = new System.Drawing.Point(15, 70);
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.RowHeadersWidth = 51;
            this.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Size = new System.Drawing.Size(920, 415);
            this.dgvUsuarios.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnNovoUsuario);
            this.panel1.Controls.Add(this.btnEditarUsuario);
            this.panel1.Controls.Add(this.btnExcluirUsuario);
            this.panel1.Controls.Add(this.btnAtualizarUsuarios);
            this.panel1.Location = new System.Drawing.Point(15, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(920, 45);
            this.panel1.TabIndex = 1;
            // 
            // btnNovoUsuario
            // 
            this.btnNovoUsuario.Location = new System.Drawing.Point(10, 10);
            this.btnNovoUsuario.Name = "btnNovoUsuario";
            this.btnNovoUsuario.Size = new System.Drawing.Size(120, 30);
            this.btnNovoUsuario.TabIndex = 0;
            this.btnNovoUsuario.Text = "? Novo Usuário";
            this.btnNovoUsuario.UseVisualStyleBackColor = true;
            this.btnNovoUsuario.Click += new System.EventHandler(this.btnNovoUsuario_Click);
            // 
            // btnEditarUsuario
            // 
            this.btnEditarUsuario.Location = new System.Drawing.Point(140, 10);
            this.btnEditarUsuario.Name = "btnEditarUsuario";
            this.btnEditarUsuario.Size = new System.Drawing.Size(100, 30);
            this.btnEditarUsuario.TabIndex = 1;
            this.btnEditarUsuario.Text = "?? Editar";
            this.btnEditarUsuario.UseVisualStyleBackColor = true;
            this.btnEditarUsuario.Click += new System.EventHandler(this.btnEditarUsuario_Click);
            // 
            // btnExcluirUsuario
            // 
            this.btnExcluirUsuario.Location = new System.Drawing.Point(250, 10);
            this.btnExcluirUsuario.Name = "btnExcluirUsuario";
            this.btnExcluirUsuario.Size = new System.Drawing.Size(100, 30);
            this.btnExcluirUsuario.TabIndex = 2;
            this.btnExcluirUsuario.Text = "??? Excluir";
            this.btnExcluirUsuario.UseVisualStyleBackColor = true;
            this.btnExcluirUsuario.Click += new System.EventHandler(this.btnExcluirUsuario_Click);
            // 
            // btnAtualizarUsuarios
            // 
            this.btnAtualizarUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizarUsuarios.Location = new System.Drawing.Point(800, 10);
            this.btnAtualizarUsuarios.Name = "btnAtualizarUsuarios";
            this.btnAtualizarUsuarios.Size = new System.Drawing.Size(110, 30);
            this.btnAtualizarUsuarios.TabIndex = 3;
            this.btnAtualizarUsuarios.Text = "?? Atualizar";
            this.btnAtualizarUsuarios.UseVisualStyleBackColor = true;
            this.btnAtualizarUsuarios.Click += new System.EventHandler(this.btnAtualizarUsuarios_Click);
            // 
            // tabRelatorios
            // 
            this.tabRelatorios.Controls.Add(this.btnGerarRelatorio);
            this.tabRelatorios.Controls.Add(this.lblTotalUsuarios);
            this.tabRelatorios.Controls.Add(this.lblChamadosFechados);
            this.tabRelatorios.Controls.Add(this.lblChamadosAbertos);
            this.tabRelatorios.Controls.Add(this.lblTotalChamados);
            this.tabRelatorios.Location = new System.Drawing.Point(4, 25);
            this.tabRelatorios.Name = "tabRelatorios";
            this.tabRelatorios.Padding = new System.Windows.Forms.Padding(3);
            this.tabRelatorios.Size = new System.Drawing.Size(952, 501);
            this.tabRelatorios.TabIndex = 1;
            this.tabRelatorios.Text = "Relatórios e Estatísticas";
            this.tabRelatorios.UseVisualStyleBackColor = true;
            // 
            // lblTotalChamados
            // 
            this.lblTotalChamados.AutoSize = true;
            this.lblTotalChamados.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTotalChamados.Location = new System.Drawing.Point(30, 30);
            this.lblTotalChamados.Name = "lblTotalChamados";
            this.lblTotalChamados.Size = new System.Drawing.Size(200, 28);
            this.lblTotalChamados.TabIndex = 0;
            this.lblTotalChamados.Text = "Total de Chamados: 0";
            // 
            // lblChamadosAbertos
            // 
            this.lblChamadosAbertos.AutoSize = true;
            this.lblChamadosAbertos.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblChamadosAbertos.Location = new System.Drawing.Point(30, 70);
            this.lblChamadosAbertos.Name = "lblChamadosAbertos";
            this.lblChamadosAbertos.Size = new System.Drawing.Size(180, 28);
            this.lblChamadosAbertos.TabIndex = 1;
            this.lblChamadosAbertos.Text = "Chamados Abertos: 0";
            // 
            // lblChamadosFechados
            // 
            this.lblChamadosFechados.AutoSize = true;
            this.lblChamadosFechados.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblChamadosFechados.Location = new System.Drawing.Point(30, 110);
            this.lblChamadosFechados.Name = "lblChamadosFechados";
            this.lblChamadosFechados.Size = new System.Drawing.Size(195, 28);
            this.lblChamadosFechados.TabIndex = 2;
            this.lblChamadosFechados.Text = "Chamados Fechados: 0";
            // 
            // lblTotalUsuarios
            // 
            this.lblTotalUsuarios.AutoSize = true;
            this.lblTotalUsuarios.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTotalUsuarios.Location = new System.Drawing.Point(30, 150);
            this.lblTotalUsuarios.Name = "lblTotalUsuarios";
            this.lblTotalUsuarios.Size = new System.Drawing.Size(180, 28);
            this.lblTotalUsuarios.TabIndex = 3;
            this.lblTotalUsuarios.Text = "Total de Usuários: 0";
            // 
            // btnGerarRelatorio
            // 
            this.btnGerarRelatorio.Location = new System.Drawing.Point(35, 200);
            this.btnGerarRelatorio.Name = "btnGerarRelatorio";
            this.btnGerarRelatorio.Size = new System.Drawing.Size(200, 40);
            this.btnGerarRelatorio.TabIndex = 4;
            this.btnGerarRelatorio.Text = "?? Gerar Relatório Completo";
            this.btnGerarRelatorio.UseVisualStyleBackColor = true;
            this.btnGerarRelatorio.Click += new System.EventHandler(this.btnGerarRelatorio_Click);
            // 
            // FrmAdministracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FrmAdministracao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administração do Sistema";
            this.Load += new System.EventHandler(this.FrmAdministracao_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabUsuarios.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabRelatorios.ResumeLayout(false);
            this.tabRelatorios.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabUsuarios;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnNovoUsuario;
        private System.Windows.Forms.Button btnEditarUsuario;
        private System.Windows.Forms.Button btnExcluirUsuario;
        private System.Windows.Forms.Button btnAtualizarUsuarios;
        private System.Windows.Forms.TabPage tabRelatorios;
        private System.Windows.Forms.Label lblTotalChamados;
        private System.Windows.Forms.Label lblChamadosAbertos;
        private System.Windows.Forms.Label lblChamadosFechados;
        private System.Windows.Forms.Label lblTotalUsuarios;
        private System.Windows.Forms.Button btnGerarRelatorio;
    }
}
