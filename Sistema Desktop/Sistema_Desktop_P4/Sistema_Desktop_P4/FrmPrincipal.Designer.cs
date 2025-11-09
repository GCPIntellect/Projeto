using System.Windows.Forms;
using System.Drawing;

namespace Sistema_Desktop_P4
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        private void InitializeComponent()
        {
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblAppSubtitle = new System.Windows.Forms.Label();
            this.panelMenuItems = new System.Windows.Forms.Panel();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnAbrirChamado = new System.Windows.Forms.Button();
            this.btnMeusChamados = new System.Windows.Forms.Button();
            this.btnConsultarChamados = new System.Windows.Forms.Button();
            this.panelAdminSeparator = new System.Windows.Forms.Panel();
            this.lblAdminSection = new System.Windows.Forms.Label();
            this.btnGerenciarUsuarios = new System.Windows.Forms.Button();
            this.btnRelatorios = new System.Windows.Forms.Button();
            this.panelUserInfo = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.btnMaximizar = new System.Windows.Forms.Button();
            this.btnFechar = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelSidebar.SuspendLayout();
            this.panelLogo.SuspendLayout();
            this.panelMenuItems.SuspendLayout();
            this.panelUserInfo.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSidebar
            // 
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.panelSidebar.Controls.Add(this.panelUserInfo);
            this.panelSidebar.Controls.Add(this.panelMenuItems);
            this.panelSidebar.Controls.Add(this.panelLogo);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Location = new System.Drawing.Point(0, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(250, 700);
            this.panelSidebar.TabIndex = 0;
            // 
            // panelLogo
            // 
            this.panelLogo.Controls.Add(this.lblAppSubtitle);
            this.panelLogo.Controls.Add(this.lblAppName);
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogo.Location = new System.Drawing.Point(0, 0);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Padding = new System.Windows.Forms.Padding(15, 20, 15, 15);
            this.panelLogo.Size = new System.Drawing.Size(250, 100);
            this.panelLogo.TabIndex = 0;
            // 
            // lblAppName
            // 
            this.lblAppName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.White;
            this.lblAppName.Location = new System.Drawing.Point(15, 20);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(220, 35);
            this.lblAppName.TabIndex = 0;
            this.lblAppName.Text = "🎫 Chamados";
            this.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAppSubtitle
            // 
            this.lblAppSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAppSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAppSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.lblAppSubtitle.Location = new System.Drawing.Point(15, 55);
            this.lblAppSubtitle.Name = "lblAppSubtitle";
            this.lblAppSubtitle.Size = new System.Drawing.Size(220, 20);
            this.lblAppSubtitle.TabIndex = 1;
            this.lblAppSubtitle.Text = "Sistema de Gestão";
            this.lblAppSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panelMenuItems
            // 
            this.panelMenuItems.Controls.Add(this.btnDashboard);
            this.panelMenuItems.Controls.Add(this.btnAbrirChamado);
            this.panelMenuItems.Controls.Add(this.btnMeusChamados);
            this.panelMenuItems.Controls.Add(this.btnConsultarChamados);
            this.panelMenuItems.Controls.Add(this.panelAdminSeparator);
            this.panelMenuItems.Controls.Add(this.btnGerenciarUsuarios);
            this.panelMenuItems.Controls.Add(this.btnRelatorios);
            this.panelMenuItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMenuItems.Location = new System.Drawing.Point(0, 100);
            this.panelMenuItems.Name = "panelMenuItems";
            this.panelMenuItems.Padding = new System.Windows.Forms.Padding(10, 20, 10, 10);
            this.panelMenuItems.Size = new System.Drawing.Size(250, 600);
            this.panelMenuItems.TabIndex = 1;
            // 
            // btnDashboard
            // 
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.Location = new System.Drawing.Point(10, 20);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnDashboard.Size = new System.Drawing.Size(230, 45);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "📊  Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = true;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // btnAbrirChamado
            // 
            this.btnAbrirChamado.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAbrirChamado.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAbrirChamado.FlatAppearance.BorderSize = 0;
            this.btnAbrirChamado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirChamado.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnAbrirChamado.ForeColor = System.Drawing.Color.White;
            this.btnAbrirChamado.Location = new System.Drawing.Point(10, 65);
            this.btnAbrirChamado.Name = "btnAbrirChamado";
            this.btnAbrirChamado.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnAbrirChamado.Size = new System.Drawing.Size(230, 45);
            this.btnAbrirChamado.TabIndex = 1;
            this.btnAbrirChamado.Text = "➕  Abrir Chamado";
            this.btnAbrirChamado.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAbrirChamado.UseVisualStyleBackColor = true;
            this.btnAbrirChamado.Click += new System.EventHandler(this.menuAbrirChamado_Click);
            // 
            // btnMeusChamados
            // 
            this.btnMeusChamados.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMeusChamados.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMeusChamados.FlatAppearance.BorderSize = 0;
            this.btnMeusChamados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMeusChamados.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnMeusChamados.ForeColor = System.Drawing.Color.White;
            this.btnMeusChamados.Location = new System.Drawing.Point(10, 110);
            this.btnMeusChamados.Name = "btnMeusChamados";
            this.btnMeusChamados.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnMeusChamados.Size = new System.Drawing.Size(230, 45);
            this.btnMeusChamados.TabIndex = 2;
            this.btnMeusChamados.Text = "📋  Meus Chamados";
            this.btnMeusChamados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMeusChamados.UseVisualStyleBackColor = true;
            this.btnMeusChamados.Click += new System.EventHandler(this.menuConsultarChamados_Click);
            // 
            // btnConsultarChamados
            // 
            this.btnConsultarChamados.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConsultarChamados.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConsultarChamados.FlatAppearance.BorderSize = 0;
            this.btnConsultarChamados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsultarChamados.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnConsultarChamados.ForeColor = System.Drawing.Color.White;
            this.btnConsultarChamados.Location = new System.Drawing.Point(10, 155);
            this.btnConsultarChamados.Name = "btnConsultarChamados";
            this.btnConsultarChamados.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnConsultarChamados.Size = new System.Drawing.Size(230, 45);
            this.btnConsultarChamados.TabIndex = 3;
            this.btnConsultarChamados.Text = "🔍  Consultar Todos";
            this.btnConsultarChamados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConsultarChamados.UseVisualStyleBackColor = true;
            this.btnConsultarChamados.Visible = false;
            this.btnConsultarChamados.Click += new System.EventHandler(this.menuConsultarChamados_Click);
            // 
            // panelAdminSeparator
            // 
            this.panelAdminSeparator.Controls.Add(this.lblAdminSection);
            this.panelAdminSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAdminSeparator.Location = new System.Drawing.Point(10, 200);
            this.panelAdminSeparator.Name = "panelAdminSeparator";
            this.panelAdminSeparator.Size = new System.Drawing.Size(230, 40);
            this.panelAdminSeparator.TabIndex = 4;
            this.panelAdminSeparator.Visible = false;
            // 
            // lblAdminSection
            // 
            this.lblAdminSection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblAdminSection.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblAdminSection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.lblAdminSection.Location = new System.Drawing.Point(0, 20);
            this.lblAdminSection.Name = "lblAdminSection";
            this.lblAdminSection.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.lblAdminSection.Size = new System.Drawing.Size(230, 20);
            this.lblAdminSection.TabIndex = 0;
            this.lblAdminSection.Text = "ADMINISTRAÇÃO";
            this.lblAdminSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnGerenciarUsuarios
            // 
            this.btnGerenciarUsuarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGerenciarUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGerenciarUsuarios.FlatAppearance.BorderSize = 0;
            this.btnGerenciarUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGerenciarUsuarios.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnGerenciarUsuarios.ForeColor = System.Drawing.Color.White;
            this.btnGerenciarUsuarios.Location = new System.Drawing.Point(10, 240);
            this.btnGerenciarUsuarios.Name = "btnGerenciarUsuarios";
            this.btnGerenciarUsuarios.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnGerenciarUsuarios.Size = new System.Drawing.Size(230, 45);
            this.btnGerenciarUsuarios.TabIndex = 5;
            this.btnGerenciarUsuarios.Text = "👥  Gerenciar Usuários";
            this.btnGerenciarUsuarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGerenciarUsuarios.UseVisualStyleBackColor = true;
            this.btnGerenciarUsuarios.Visible = false;
            this.btnGerenciarUsuarios.Click += new System.EventHandler(this.menuAdminUsuarios_Click);
            // 
            // btnRelatorios
            // 
            this.btnRelatorios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRelatorios.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRelatorios.FlatAppearance.BorderSize = 0;
            this.btnRelatorios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorios.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRelatorios.ForeColor = System.Drawing.Color.White;
            this.btnRelatorios.Location = new System.Drawing.Point(10, 285);
            this.btnRelatorios.Name = "btnRelatorios";
            this.btnRelatorios.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnRelatorios.Size = new System.Drawing.Size(230, 45);
            this.btnRelatorios.TabIndex = 6;
            this.btnRelatorios.Text = "📈  Relatórios";
            this.btnRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRelatorios.UseVisualStyleBackColor = true;
            this.btnRelatorios.Visible = false;
            this.btnRelatorios.Click += new System.EventHandler(this.menuAdminUsuarios_Click);
            // 
            // panelUserInfo
            // 
            this.panelUserInfo.Controls.Add(this.btnLogout);
            this.panelUserInfo.Controls.Add(this.lblUserRole);
            this.panelUserInfo.Controls.Add(this.lblUserName);
            this.panelUserInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelUserInfo.Location = new System.Drawing.Point(0, 600);
            this.panelUserInfo.Name = "panelUserInfo";
            this.panelUserInfo.Padding = new System.Windows.Forms.Padding(15, 10, 15, 15);
            this.panelUserInfo.Size = new System.Drawing.Size(250, 100);
            this.panelUserInfo.TabIndex = 2;
            // 
            // lblUserName
            // 
            this.lblUserName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblUserName.ForeColor = System.Drawing.Color.White;
            this.lblUserName.Location = new System.Drawing.Point(15, 10);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(220, 20);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Text = "Nome do Usuário";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUserRole
            // 
            this.lblUserRole.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUserRole.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblUserRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.lblUserRole.Location = new System.Drawing.Point(15, 30);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(220, 18);
            this.lblUserRole.TabIndex = 1;
            this.lblUserRole.Text = "Colaborador";
            this.lblUserRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLogout
            // 
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLogout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnLogout.Location = new System.Drawing.Point(15, 48);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(220, 30);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "🚪 Sair";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.menuSessaoLogout_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(32)))), ((int)(((byte)(33)))));
            this.panelTop.Controls.Add(this.btnFechar);
            this.panelTop.Controls.Add(this.btnMaximizar);
            this.panelTop.Controls.Add(this.btnMinimizar);
            this.panelTop.Controls.Add(this.lblPageTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(250, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(950, 60);
            this.panelTop.TabIndex = 1;
            // 
            // lblPageTitle
            // 
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.White;
            this.lblPageTitle.Location = new System.Drawing.Point(20, 15);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(400, 30);
            this.lblPageTitle.TabIndex = 0;
            this.lblPageTitle.Text = "Dashboard";
            this.lblPageTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnMinimizar.ForeColor = System.Drawing.Color.White;
            this.btnMinimizar.Location = new System.Drawing.Point(820, 10);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(40, 40);
            this.btnMinimizar.TabIndex = 1;
            this.btnMinimizar.Text = "─";
            this.btnMinimizar.UseVisualStyleBackColor = true;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMinimizar_Click);
            // 
            // btnMaximizar
            // 
            this.btnMaximizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximizar.FlatAppearance.BorderSize = 0;
            this.btnMaximizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximizar.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnMaximizar.ForeColor = System.Drawing.Color.White;
            this.btnMaximizar.Location = new System.Drawing.Point(865, 10);
            this.btnMaximizar.Name = "btnMaximizar";
            this.btnMaximizar.Size = new System.Drawing.Size(40, 40);
            this.btnMaximizar.TabIndex = 2;
            this.btnMaximizar.Text = "□";
            this.btnMaximizar.UseVisualStyleBackColor = true;
            this.btnMaximizar.Click += new System.EventHandler(this.btnMaximizar_Click);
            // 
            // btnFechar
            // 
            this.btnFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFechar.FlatAppearance.BorderSize = 0;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnFechar.ForeColor = System.Drawing.Color.White;
            this.btnFechar.Location = new System.Drawing.Point(910, 10);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(40, 40);
            this.btnFechar.TabIndex = 3;
            this.btnFechar.Text = "✕";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.menuSessaoSair_Click);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(32)))), ((int)(((byte)(33)))));
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(250, 60);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(20);
            this.panelContent.Size = new System.Drawing.Size(950, 618);
            this.panelContent.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(37)))), ((int)(((byte)(37)))));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(250, 678);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(950, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 16);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Gestão de Chamados";
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.panelSidebar.ResumeLayout(false);
            this.panelLogo.ResumeLayout(false);
            this.panelMenuItems.ResumeLayout(false);
            this.panelUserInfo.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblAppSubtitle;
        private System.Windows.Forms.Panel panelMenuItems;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnAbrirChamado;
        private System.Windows.Forms.Button btnMeusChamados;
        private System.Windows.Forms.Button btnConsultarChamados;
        private System.Windows.Forms.Panel panelAdminSeparator;
        private System.Windows.Forms.Label lblAdminSection;
        private System.Windows.Forms.Button btnGerenciarUsuarios;
        private System.Windows.Forms.Button btnRelatorios;
        private System.Windows.Forms.Panel panelUserInfo;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblUserRole;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnMaximizar;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}