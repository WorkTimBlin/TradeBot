using RansacBot.Trading.Filters;
using RansacBot.UI.Components;
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
	public partial class CurfewFiltersDialog : Form
	{
		public List<(TimeSpan closingTime, TimeSpan openingTime)> AllFilterTimes =>
			(flowLayoutPanel.Controls as IEnumerable<Control> ?? throw new Exception("Controls not Enumerable")).
			Select(control => control as CurfewControl ?? throw new Exception("Control wasn't CurfewControl")).
			Select(control => (control.ClosingTime, control.OpeningTime)).ToList();
		public CurfewFiltersDialog()
		{
			InitializeComponent();
		}
		private void AddNewFilter()
		{
			flowLayoutPanel.Controls.Add(new Components.CurfewControl());
		}
		private void ClearAllFilters()
		{
			flowLayoutPanel.Controls.Clear();
		}
		private void RemoveLastFilter()
		{
			flowLayoutPanel.Controls.RemoveAt(flowLayoutPanel.Controls.Count);
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			AddNewFilter();
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			RemoveLastFilter();
		}

		private void clearButton_Click(object sender, EventArgs e)
		{
			ClearAllFilters();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}
	}
}
