using System;
using System.Linq;
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
            ClearForms();
            InitForms();
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
        private void CbClassCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cbTools.Items.Clear();
                cbAccountID.Items.Clear();
                cbFirmID.Items.Clear();

                cbFirmID.Items.Add(Connector.quik.Class.GetClassInfo(cbClassCode.Text).Result.FirmId);
                cbTools.Items.AddRange(Connector.quik.Class.GetClassSecurities(cbClassCode.Text).Result);
                cbAccountID.Items.AddRange(Connector.quik.Class.GetTradeAccounts().Result.FindAll(x => x.ClassCodes.Contains(cbClassCode.Text)).Select(x => x.TrdaccId).ToArray());

                cbFirmID.SelectedIndex = 0;

                if (cbAccountID.Items.Count > 0)
                    cbAccountID.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LOGGER.Message("CbClassCode_SelectedIndexChanged(): Exception - " + ex.Message);
            }
        }
        private void CbTools_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckTool();
        }




        private void CheckTool()
        {
            try
            {
                if (cbTools.Text != "" && cbClientCode.Text != "" && cbAccountID.Text != "")
                {
                    Tool tool = new(cbTools.Text, cbClientCode.Text, cbAccountID.Text);
                    nmcN.DecimalPlaces = tool.PriceAccuracy;
                    nmcN.Increment = Convert.ToDecimal(tool.Step);
                    nmcN.Minimum = Convert.ToDecimal(tool.Step);
                    nmcN.Maximum = Convert.ToDecimal(tool.Step) * 10000;
                    nmcN.Value = Convert.ToDecimal(tool.Step) * 10;
                    btnStart.Enabled = true;
                    Trader.InitTrader(tool);
                    return;
                }
            }
            catch
            {

            }
            btnStart.Enabled = false;
        }
        private void InitForms()
        {
            try
            {
                cbPort.Items.Add(34132);
                cbClassCode.Items.AddRange(Connector.quik.Class.GetClassesList().Result.ToArray());
                cbClientCode.Items.AddRange(Connector.quik.Class.GetClientCodes().Result.ToArray());

            }
            catch (Exception ex)
            {
                btnStart.Enabled = false;
                LOGGER.Message("FormLogin.InitForms(): Exception - " + ex.Message);
                return;
            }

            cbPort.SelectedIndex = 0;
            cbClientCode.SelectedIndex = 0;
        }
        private void ClearForms()
        {
            cbPort.Items.Clear();
            cbClassCode.Items.Clear();
            cbTools.Items.Clear();
            cbAccountID.Items.Clear();
            cbClientCode.Items.Clear();
            cbFirmID.Items.Clear();
            btnStart.Enabled = false;
        }
    }
}
