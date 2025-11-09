using System.Drawing;
using System.Windows.Forms;

namespace Sistema_Desktop_P4.Infrastructure
{
    /// <summary>
    /// Gerencia o tema escuro moderno da aplicação baseado na paleta de cores fornecida.
    /// </summary>
    public static class ThemeManager
    {
        // Paleta de Cores
        public static readonly Color CorPrimaria = ColorTranslator.FromHtml("#0d6efd");
        public static readonly Color CorSucesso = ColorTranslator.FromHtml("#10b981");
        public static readonly Color CorAlerta = ColorTranslator.FromHtml("#f59e0b");
        public static readonly Color CorErro = ColorTranslator.FromHtml("#ef4444");

        // Tons de Fundo (Escuro)
        public static readonly Color CorFundoEscuro1 = ColorTranslator.FromHtml("#222021"); // Fundo principal
        public static readonly Color CorFundoEscuro2 = ColorTranslator.FromHtml("#282525"); // Cards e sidebar
        public static readonly Color CorFundoEscuro3 = ColorTranslator.FromHtml("#3B3A3B"); // Inputs e hover

        // Tons de Texto e Bordas
        public static readonly Color CorTextoClaro = ColorTranslator.FromHtml("#EAEAEA");
        public static readonly Color CorTextoSecundario = ColorTranslator.FromHtml("#A9A9A9");
        public static readonly Color CorBorda = ColorTranslator.FromHtml("#374151");

        // Fonte principal
        public static readonly Font FontePrincipal = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static readonly Font FonteTitulo = new Font("Segoe UI", 12F, FontStyle.Bold);
        public static readonly Font FonteBotao = new Font("Segoe UI", 9F, FontStyle.Bold);

        /// <summary>
        /// Aplica o tema escuro a um formulário e todos os seus controles.
        /// </summary>
        public static void AplicarTema(Form form)
        {
            form.BackColor = CorFundoEscuro1;
            form.ForeColor = CorTextoClaro;
            form.Font = FontePrincipal;

            AplicarTemaRecursivo(form.Controls);
        }

        private static void AplicarTemaRecursivo(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // Labels
                if (control is Label label)
                {
                    label.ForeColor = CorTextoSecundario;
                    label.BackColor = Color.Transparent;
                }
                // TextBoxes
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = CorFundoEscuro3;
                    textBox.ForeColor = CorTextoClaro;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                }
                // ComboBoxes
                else if (control is ComboBox comboBox)
                {
                    comboBox.BackColor = CorFundoEscuro3;
                    comboBox.ForeColor = CorTextoClaro;
                    comboBox.FlatStyle = FlatStyle.Flat;
                }
                // Buttons
                else if (control is Button button)
                {
                    EstilizarBotao(button);
                }
                // DataGridView
                else if (control is DataGridView dgv)
                {
                    EstilizarDataGridView(dgv);
                }
                // Panels e GroupBoxes
                else if (control is Panel panel)
                {
                    panel.BackColor = CorFundoEscuro2;
                    panel.ForeColor = CorTextoClaro;
                }
                else if (control is GroupBox groupBox)
                {
                    groupBox.BackColor = CorFundoEscuro2;
                    groupBox.ForeColor = CorTextoClaro;
                }
                // MenuStrip
                else if (control is MenuStrip menuStrip)
                {
                    EstilizarMenuStrip(menuStrip);
                }
                // StatusStrip
                else if (control is StatusStrip statusStrip)
                {
                    EstilizarStatusStrip(statusStrip);
                }

                // Recursão para controles filhos
                if (control.HasChildren)
                {
                    AplicarTemaRecursivo(control.Controls);
                }
            }
        }

        /// <summary>
        /// Estiliza um botão como primário (azul).
        /// </summary>
        public static void EstilizarBotaoPrimario(Button button)
        {
            button.BackColor = CorPrimaria;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = FonteBotao;
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Estiliza um botão como secundário (contorno).
        /// </summary>
        public static void EstilizarBotaoSecundario(Button button)
        {
            button.BackColor = Color.Transparent;
            button.ForeColor = CorTextoClaro;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = CorBorda;
            button.FlatAppearance.BorderSize = 1;
            button.Font = FonteBotao;
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Estiliza um botão de sucesso (verde).
        /// </summary>
        public static void EstilizarBotaoSucesso(Button button)
        {
            button.BackColor = CorSucesso;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = FonteBotao;
            button.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Estiliza um botão de erro (vermelho).
        /// </summary>
        public static void EstilizarBotaoErro(Button button)
        {
            button.BackColor = CorErro;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = FonteBotao;
            button.Cursor = Cursors.Hand;
        }

        private static void EstilizarBotao(Button button)
        {
            // Por padrão, botões são secundários
            EstilizarBotaoSecundario(button);
        }

        private static void EstilizarDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = CorFundoEscuro2;
            dgv.ForeColor = CorTextoClaro;
            dgv.GridColor = CorBorda;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.EnableHeadersVisualStyles = false;

            // Header
            dgv.ColumnHeadersDefaultCellStyle.BackColor = CorFundoEscuro3;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = CorTextoSecundario;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = CorFundoEscuro3;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Rows
            dgv.DefaultCellStyle.BackColor = CorFundoEscuro2;
            dgv.DefaultCellStyle.ForeColor = CorTextoClaro;
            dgv.DefaultCellStyle.SelectionBackColor = CorFundoEscuro3;
            dgv.DefaultCellStyle.SelectionForeColor = CorTextoClaro;
            dgv.RowHeadersVisible = false;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#252223");
        }

        private static void EstilizarMenuStrip(MenuStrip menuStrip)
        {
            menuStrip.BackColor = CorFundoEscuro2;
            menuStrip.ForeColor = CorTextoClaro;
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new MenuStripColorTable());
        }

        private static void EstilizarStatusStrip(StatusStrip statusStrip)
        {
            statusStrip.BackColor = CorFundoEscuro2;
            statusStrip.ForeColor = CorTextoClaro;
            statusStrip.Renderer = new ToolStripProfessionalRenderer(new MenuStripColorTable());
        }

        // Classe auxiliar para colorir MenuStrip
        private class MenuStripColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected => ThemeManager.CorFundoEscuro3;
            public override Color MenuItemSelectedGradientBegin => ThemeManager.CorFundoEscuro3;
            public override Color MenuItemSelectedGradientEnd => ThemeManager.CorFundoEscuro3;
            public override Color MenuItemPressedGradientBegin => ThemeManager.CorFundoEscuro3;
            public override Color MenuItemPressedGradientEnd => ThemeManager.CorFundoEscuro3;
            public override Color MenuItemBorder => ThemeManager.CorBorda;
            public override Color MenuBorder => ThemeManager.CorBorda;
            public override Color ToolStripDropDownBackground => ThemeManager.CorFundoEscuro2;
            public override Color ImageMarginGradientBegin => ThemeManager.CorFundoEscuro2;
            public override Color ImageMarginGradientMiddle => ThemeManager.CorFundoEscuro2;
            public override Color ImageMarginGradientEnd => ThemeManager.CorFundoEscuro2;
        }
    }
}
