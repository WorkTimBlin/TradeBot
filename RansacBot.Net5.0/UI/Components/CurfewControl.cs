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
	public partial class CurfewControl : UserControl
	{
		public TimeSpan ClosingTime => closingTimePicker.Time;
		public TimeSpan OpeningTime => openingTimePicker.Time;
		public CurfewControl()
		{
			InitializeComponent();
		}
	}
}
