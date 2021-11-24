using System;
using System.Windows.Forms;

namespace RansacBot.Net5._0
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            cbPort.SelectedIndex = 0;
            CheckSetting();
        }
        private void BtnStart_Click(object sender, EventArgs e)
        {
            Trader.SetN((int)nmcN.Value);
            DialogResult = DialogResult.OK;
            Close();
        }
        private void BtnCansel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private void TbTool_TextChanged(object sender, EventArgs e)
        {
            CheckSetting();
        }
        private void CheckSetting()
        {
            try
            {
                Tool tool = new(tbTool.Text, "SPBFUT");
                nmcN.DecimalPlaces = tool.PriceAccuracy;
                nmcN.Increment = Convert.ToDecimal(tool.Step);
                nmcN.Minimum = Convert.ToDecimal(tool.Step);
                nmcN.Maximum = Convert.ToDecimal(tool.Step) * 10000;
                nmcN.Value = Convert.ToDecimal(tool.Step) * 10;
                btnStart.Enabled = true;
                Trader.InitTrader(tool);
            }
            catch
            {
                btnStart.Enabled = false;
            }
        }
    }
}
