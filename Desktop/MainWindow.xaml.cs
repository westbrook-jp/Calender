using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Westbrook.Calender
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		const int DefaultPosition = 1000000000;
		// values
		DispatcherTimer timer_;
		TimeSpan refleshTimeSpan_;
		DateTime currentDate_ = DateTime.MinValue;
		bool showOtherDateFlag_ = false;

		public MainWindow()
		{
			// initialize values
			timer_ = new DispatcherTimer(DispatcherPriority.Normal);
			refleshTimeSpan_ = new TimeSpan(0, 0, Properties.Settings.Default.Calender_RefleshSpan);

			// initialize command binding objects
			InitializeCommandBindingObjects();
			// initialize UI component
			InitializeComponent();
			// initialize command binding
			InitializeCommandBinding(this);
			//
			// additional UI initalize
			if (Properties.Settings.Default.MainWindow_Left != DefaultPosition) {
				this.Left = Properties.Settings.Default.MainWindow_Left;
				this.Top = Properties.Settings.Default.MainWindow_Top;
			}
			this.Calender.FirstDayOfWeek = Properties.Settings.Default.Calender_FirstDayOfWeek;

			// event
			this.Loaded += Window_Loaded;
			this.Closing += Window_Closing;
			this.Closed += Window_Closed;
			this.MouseLeftButtonDown += Window_MouseLeftButtonDown;

			this.Calender.DisplayDateChanged += Calender_DisplayDateChanged;

			// timer
			timer_.Interval = new TimeSpan(0, 0, 1);
			timer_.Tick += new EventHandler(Timer_Tick);
			timer_.Start();

			// Set the logical focus to the window
			Focus();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
#if false
			if (Properties.Settings.Default.MainWindow_Left == DefaultPosition) {
				Properties.Settings.Default.MainWindow_Left = (int)this.Left;
				Properties.Settings.Default.MainWindow_Top = (int)this.Top;
			}
#endif
		}
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// save window position
			if (this.WindowState == System.Windows.WindowState.Normal) {
				Properties.Settings.Default.MainWindow_Left = (int)this.Left;
				Properties.Settings.Default.MainWindow_Top = (int)this.Top;
			}
		}
		private void Window_Closed(object sender, EventArgs e)
		{
			Properties.Settings.Default.Save();
		}
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
			// save window position
			if (this.WindowState == System.Windows.WindowState.Normal) {
				Properties.Settings.Default.MainWindow_Left = (int)this.Left;
				Properties.Settings.Default.MainWindow_Top = (int)this.Top;
			}
		}
		void Timer_Tick(object sender, EventArgs e)
		{
			timer_.Stop();
			if (!showOtherDateFlag_) {
				DateTime today = DateTime.Today;
#if false  // for test
				today = new DateTime(today.Year, (int)((double)DateTime.Now.Minute / 60 * 12 + 1), (int)((double)DateTime.Now.Second / 60 * 30 + 1));
#endif
				if (currentDate_ != today) {
					currentDate_ = today;
					this.Calender.DisplayDate = today;
					Debug.WriteLine("DateChange:{0}", this.Calender.DisplayDate);
				}
			}
			timer_.Interval = refleshTimeSpan_;
			timer_.Start();
		}

		void Calender_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
		{
			if (this.Calender.DisplayDate == currentDate_) {
				showOtherDateFlag_ = false;
			}
			else {
				showOtherDateFlag_ = true;
			}
			Debug.WriteLine("DisplayDateChanged:{0}:showOther:{1}", this.Calender.DisplayDate, showOtherDateFlag_);
		}

		//
		// Command & Command Bindings
		//
		// base commands
		public static readonly ICommand Command_Shutdown = new RoutedCommand("Command_Shutdown", typeof(MainWindow));
		CommandBinding CommandBinding_Shutdown;
		public static readonly ICommand Command_About = new RoutedCommand("Command_About", typeof(MainWindow));
		CommandBinding CommandBinding_About;
		public static readonly ICommand Command_Setting = new RoutedCommand("Command_Setting", typeof(MainWindow));
		CommandBinding CommandBinding_Setting;
		public static readonly ICommand Command_Today = new RoutedCommand("Command_Today", typeof(MainWindow));
		CommandBinding CommandBinding_Today;

		// initalize
		void InitializeCommandBindingObjects()
		{
			// base commands
			CommandBinding_Shutdown = new CommandBinding(Command_Shutdown, Command_Shutdown_Executed, Command_Shutdown_CanExecute);
			CommandBinding_About = new CommandBinding(Command_About, Command_About_Executed, Command_About_CanExecute);
			CommandBinding_Setting = new CommandBinding(Command_Setting, Command_Setting_Executed, Command_Setting_CanExecute);
			CommandBinding_Today = new CommandBinding(Command_Today, Command_Today_Executed, Command_Today_CanExecute);
		}
		void InitializeCommandBinding(UIElement element)
		{
			element.CommandBindings.Add(CommandBinding_Shutdown);
			element.CommandBindings.Add(CommandBinding_About);
			element.CommandBindings.Add(CommandBinding_Setting);
			element.CommandBindings.Add(CommandBinding_Today);
		}

		// Commands
		void Command_Shutdown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
		void Command_Shutdown_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}

		void Command_About_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
		void Command_About_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var dlg = new Dialog.ApplicationAboutDialog();
			dlg.Owner = this;
			dlg.ShowDialog();
		}

		void Command_Setting_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
		void Command_Setting_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var dlg = new Dialog.ApplicationSettingDialog();
			dlg.Owner = this;

			dlg.MainWindowLeft = Properties.Settings.Default.MainWindow_Left;
			dlg.MainWindowTop = Properties.Settings.Default.MainWindow_Top;
			dlg.FirstDayOfWeek = Properties.Settings.Default.Calender_FirstDayOfWeek;

			// show
			var dlgResult = dlg.ShowDialog();
			if (!dlgResult.HasValue || !dlgResult.Value) { return; }

			// change setting
			bool reloadFlag = false;
			if (dlg.MainWindowLeft != Properties.Settings.Default.MainWindow_Left ||
				dlg.MainWindowTop != Properties.Settings.Default.MainWindow_Top) {
				Properties.Settings.Default.MainWindow_Left = dlg.MainWindowLeft;
				Properties.Settings.Default.MainWindow_Top = dlg.MainWindowTop;
				this.Left = Properties.Settings.Default.MainWindow_Left;
				this.Top = Properties.Settings.Default.MainWindow_Top;
			}
			if (dlg.FirstDayOfWeek != Properties.Settings.Default.Calender_FirstDayOfWeek) {
				Properties.Settings.Default.Calender_FirstDayOfWeek = dlg.FirstDayOfWeek;
				this.Calender.FirstDayOfWeek = Properties.Settings.Default.Calender_FirstDayOfWeek;
			}
			if (reloadFlag) {
				// UpdateEventList();
			}
		}

		void Command_Today_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
		void Command_Today_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.Calender.DisplayMode = CalendarMode.Month;
			this.Calender.DisplayDate = currentDate_;
		}
	}
}
