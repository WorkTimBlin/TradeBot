using RansacsRealTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RansacBot.UI.Components
{
	public partial class RansacLevelUsageControl : UserControl
	{
		public string LabelText
		{
			get => nameLabel.Text;
			set => nameLabel.Text = value;
		}
		public SigmaType SigmaType 
		{ 
			get => Enum.Parse<SigmaType>(sigmaTypeComboBox.SelectedItem.ToString() ?? throw new Exception()); 
			set { sigmaTypeComboBox.SelectedItem = value.ToString(); }
		}
		public int Level 
		{ 
			get => (int)levelNumericUpDown.Value;
			set { levelNumericUpDown.Value = value; }
		}
		public RansacLevelUsageControl()
		{
			InitializeComponent();
			sigmaTypeComboBox.Items.AddRange(Enum.GetNames<SigmaType>());
			sigmaTypeComboBox.SelectedIndex = 0;
		}
	}
}
