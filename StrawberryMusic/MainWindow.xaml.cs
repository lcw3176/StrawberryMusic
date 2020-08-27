using StrawberryMusic.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace StrawberryMusic
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        public System.Windows.Forms.NotifyIcon notify;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
            notify = new System.Windows.Forms.NotifyIcon();
            notify.Icon = new System.Drawing.Icon("./Resource/icon.ico");
            notify.Visible = false;
            notify.ContextMenu = menu;
            notify.Text = "NewBerry Beta";

            notify.DoubleClick += Notify_DoubleClick;

            System.Windows.Forms.MenuItem item1 = new System.Windows.Forms.MenuItem();
            menu.MenuItems.Add(item1);
            item1.Index = 0;
            item1.Text = "프로그램 종료";
            item1.Click += delegate (object click, EventArgs eClick)
            {
                Application.Current.Shutdown();
                notify.Dispose();
            };

        }

        private void Notify_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
            notify.Visible = false;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            notify.Visible = true;
            e.Cancel = true;
            this.Hide();
        }

        private void dragBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
