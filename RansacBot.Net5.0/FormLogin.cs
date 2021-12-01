using System;
using System.Linq;
using System.Windows.Forms;
using RansacRealTime;

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
            if (!CheckTool(out Instrument tool))
                return;

            MonkeyNFilter monkeyNFilter = new((int)nmcN.Value);
            Vertexes vertexes = new();

            RansacHystory ransacHystory1 = new(vertexes, ToTypeSigma(cbFilterMMR.Text), (int)nmcLevelMMR.Value - 1);
            RansacHystory ransacHystory2 = new(vertexes, ToTypeSigma(cbFilterOR.Text), (int)nmcLevelOR.Value - 1);
            RansacHystory ransacHystory3 = new(vertexes, ToTypeSigma(cbCloseN1.Text), (int)nmcLevelCloseN1.Value - 1);
            RansacHystory ransacHystory4 = new(vertexes, ToTypeSigma(cbCloseN2.Text), (int)nmcLevelCloseN2.Value - 1);
  
            RansacObserver ransacObserver = new(vertexes, monkeyNFilter);
            //ToolObserver.Initialization(ransacObserver, tool, (int)nmcN.Value, (double)nmcPercentCloseN1.Value);
            InstrumentObserver.Initialization((int)nmcN.Value, (double)nmcPercentCloseN1.Value);

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
            catch
            {
                lblError.Text = "Нарушение связи с Quik. Попробуйте войти снова.";
                gbQuikSettings.Enabled = false;
                gbStrategySettings.Enabled = false;
                btnStart.Enabled = false;
            }
        }
        private void CbTools_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CheckTool(out Instrument tool))
                btnStart.Enabled = true;
            else btnStart.Enabled = false;
        }

        private bool CheckTool(out Instrument tool)
        {
            tool = null;

            try
            {
                if (cbTools.Text != "" && cbClientCode.Text != "" && cbAccountID.Text != "")
                {
                    tool = new(cbTools.Text, cbClientCode.Text, cbAccountID.Text, cbFirmID.Text);
                    nmcN.DecimalPlaces = tool.priceAccuracy;
                    nmcN.Increment = Convert.ToDecimal(tool.step);
                    nmcN.Minimum = Convert.ToDecimal(tool.step);
                    nmcN.Maximum = Convert.ToDecimal(tool.step) * 10000;
                    nmcN.Value = Convert.ToDecimal(tool.step) * 10;
                    nmcN.Enabled = true;
                    return true;
                }
                lblError.Text = "Некоторые поля не заполнены или заполнены некорректно!";
            }
            catch
            {
                lblError.Text = "Поймано исключение во время загрузки информации об инструменте.";
            }
            return false;
        }
        private void InitForms()
        {
            var clietnCodes = Connector.quik.Class.GetClientCodes().Result.ToArray();
            var classes = Connector.quik.Class.GetClassesList().Result.ToArray();

            if (clietnCodes != null && classes != null)
            {
                cbClientCode.Items.AddRange(clietnCodes);
                cbClassCode.Items.AddRange(classes);
                cbPort.Items.Add(34132);
                cbClassCode.SelectedItem = "SPBFUT";
                cbTools.SelectedItem = "RiZ1";

                cbPort.SelectedIndex = 0;
                cbClientCode.SelectedIndex = 0;
                cbFilterMMR.SelectedIndex = 0;
                cbFilterOR.SelectedIndex = 1;
                cbCloseN1.SelectedIndex = 2;
                cbCloseN2.SelectedIndex = 3;
            }
            else
            {
                lblError.Text = "Нарушение связи с Quik. Попробуйте войти снова.";
                gbQuikSettings.Enabled = false;
                gbStrategySettings.Enabled = false;
                btnStart.Enabled = false;
            }
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
        private static TypeSigma ToTypeSigma(string text)
        {
            return text switch
            {
                "ConfidenceInterval-90" => TypeSigma.СonfidenceInterval,
                "ErrorThreshold" => TypeSigma.ErrorThreshold,
                "SigmaInliers" => TypeSigma.SigmaInliers,
                _ => TypeSigma.Sigma,
            };
        }
    }
}
