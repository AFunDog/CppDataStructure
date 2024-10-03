using System.Configuration;
using System.Data;
using System.Windows;

namespace WPFExample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            FriendManager.Init();
            DispatcherUnhandledException += (s, e) =>
            {
                FriendManager.End();
            };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            FriendManager.End();
        }
    }
}
