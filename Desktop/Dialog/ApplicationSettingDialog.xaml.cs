using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Westbrook.Calender.Dialog
{
	/// <summary>
	/// ApplicationSettingDialog.xaml の相互作用ロジック
	/// </summary>
	public partial class ApplicationSettingDialog : Window
	{
		public ApplicationSettingDialog()
		{
			InitializeComponent();
			this.MainWindowLeft = 0;
			this.MainWindowTop = 0;
			this.FirstDayOfWeek = DayOfWeek.Sunday;
		}
		private void buttonOk_Click(object sender, RoutedEventArgs e)
		{
			mainWindowLeft_ = ParseDecimal(this.textLeft.Text, mainWindowLeft_);
			mainWindowTop_ = ParseDecimal(this.textTop.Text, mainWindowTop_);
			firstDayOfWeek_ = (DayOfWeek)this.comboFirstDayOfWeek.SelectedValue;
			this.DialogResult = true;
		}
		// positive decimal
		static int ParsePositiveDecimal(string str, int defalut)
		{
			int result = defalut;
			int value;
			if (Int32.TryParse(str, out value)) {
				if (value > 0) {
					result = value;
				}
			}
			return result;
		}
		// 
		static int ParseDecimal(string str, int defalut)
		{
			int result = defalut;
			int value;
			if (Int32.TryParse(str, out value)) {
				result = value;
			}
			return result;
		}
		// hex 
		static int ParseHex(string str, int defalut)
		{
			int result = defalut;
			int value;
			if (Int32.TryParse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value)) {
				result = value;
			}
			return result;
		}
		// MainWindow Location
		int mainWindowLeft_;
		int mainWindowTop_;

		public int MainWindowLeft {
			set {
				mainWindowLeft_ = value;
				this.textLeft.Text = mainWindowLeft_.ToString();
			}
			get {
				return mainWindowLeft_;
			}
		}
		public int MainWindowTop {
			set {
				mainWindowTop_ = value;
				this.textTop.Text = mainWindowTop_.ToString();
			}
			get {
				return mainWindowTop_;
			}
		}

		// 
		DayOfWeek firstDayOfWeek_;

		public DayOfWeek FirstDayOfWeek
		{
			set {
				firstDayOfWeek_ = value;
				this.comboFirstDayOfWeek.SelectedValue = value;
			}
			get {
				return firstDayOfWeek_;
			}
		}
	}
}
