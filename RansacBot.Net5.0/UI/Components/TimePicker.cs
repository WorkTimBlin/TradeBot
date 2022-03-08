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
	public partial class TimePicker : UserControl
	{
		public TimeSpan Time => new(Hours, Minutes, 0);
		public int Hours { get => (int)hoursNumericUpDown.Value; set => hoursNumericUpDown.Value = value; }
		public int Minutes { get => (int)minutesNumericUpDown.Value; set => minutesNumericUpDown.Value = value; }

		public TimePicker()
		{
			InitializeComponent();
		}
	}
}
