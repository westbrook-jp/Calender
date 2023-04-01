using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Reflection;

namespace Westbrook.Calender.Dialog
{
	/// <summary>
	/// ApplicationAboutDialog.xaml の相互作用ロジック
	/// </summary>
	public partial class ApplicationAboutDialog : Window
	{
		public ApplicationAboutDialog()
		{
			InitializeComponent();
			linkSoftwarePage.Inlines.Clear();
			linkSoftwarePage.Inlines.Add(Properties.Resources.SoftwarePageUrl);
			Assembly asm = Assembly.GetExecutingAssembly();
			var info = FileVersionInfo.GetVersionInfo(asm.Location);
			// Version ver = asm.GetName().Version;
			StringBuilder versionString = new StringBuilder("Version");
			int versionMinor = info.FileMinorPart;
			string developVersionString = null; ;
			switch (info.FilePrivatePart) {
			case 9999: // Beta
				developVersionString = "Beta";
				break;
			case 9998: // Alpha
				developVersionString = "Alpha";
				break;
			}
			if (!String.IsNullOrEmpty(developVersionString)) {
				++versionMinor;
			}
			versionString.AppendFormat(" {0}.{1}", info.FileMajorPart, versionMinor);
			if (!String.IsNullOrEmpty(developVersionString)) {
				versionString.AppendFormat(" {0}", developVersionString);
			}
			versionString.AppendFormat(" build {0}", info.FileBuildPart);
			labelVersion.Content = versionString;
		}
		private void linkToHomePage_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start(Properties.Resources.HomepageUrl);
		}

		private void linkSoftwarePage_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start(Properties.Resources.SoftwarePageUrl);
		}
	}
}
