using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RansacBot.UI
{
	public partial class FormBotOrHystoryTestChoice : Form
	{
		public Button buttonBot;
		public Button buttonTest;

		public FormBotOrHystoryTestChoice()
		{
			InitializeComponent();
			buttonBot = buttonBotDemo;
			buttonTest = buttonHystoryTest;
		}

		private void buttonHystoryTest_Click(object sender, EventArgs e)
		{
			SwitchToForm(new HystoryTest.FlexibleHystoryRunForm());
		}

		private void buttonBotDemo_Click(object sender, EventArgs e)
		{
			SwitchToForm(new FormRansacsWithTradesBuildingPreview());
		}

		void SwitchToForm(Form form)
		{
			form.FormClosed += (object? sender, FormClosedEventArgs args) => { this.Close(); };
			form.Show();
			this.Hide();
		}
	}
}
