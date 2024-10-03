using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Wpf.Ui.Controls;
using WPFExample.Controls;

namespace WPFExample
{
    internal sealed class ImagePathToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && Path.Exists(path))
            {
                return Path.IsPathRooted(path)
                    ? path
                    : Path.Combine(@"pack://SiteOfOrigin:,,,/", path);
            }
            return "Assets/icon.png";
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotImplementedException();
        }
    }

    internal sealed class DivConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / double.Parse((string)parameter);
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            throw new NotImplementedException();
        }
    }

    public partial class MainWindow : FluentWindow
    {
        MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }

        private void FriendInfo_OnMouseRightButtonDowned(object sender, MouseButtonEventArgs e)
        {
            var source = (sender as FrameworkElement);
            if (source is not null)
            {
                var info = (Friend.BasicInfo)source.Tag;
                ViewModel.SelectedFriendId = info.Id;
                ViewModel.SelectedFriendIndex = info.Index;
            }
        }

        private void RemoveFriendMenuItem_OnClicked(object sender, RoutedEventArgs e)
        {
            FriendManager.Input.WriteLine($"remove {ViewModel.SelectedFriendId}");
            FriendManager.Input.WriteLine("selectAll");
        }

        private void EditFriendMenuItem_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.StartEditCommand.Execute(null);

        private void InsertFriendAheadMenuItem_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.StartAddCommand.Execute(ViewModel.SelectedFriendIndex);

        private void InsertFriendBackMenuItem_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.StartAddCommand.Execute(ViewModel.SelectedFriendIndex + 1);

        private void AddFriendMenuItem_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.StartAddCommand.Execute(0);

        private void ReadFriendDataMenuItem_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.LoadLocalDataCommand.Execute(null);

        private void SaveFriendDataMenuItem_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.SaveLocalDataCommand.Execute(null);

        private void ClearFriendMenuItem_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.StartClearFriendCommand.Execute(null);

        private void SearchFriendMenuItem_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.StartSearchFriendCommand.Execute(null);

        private void CancelSearchMenuItem_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.CancelSearchModeCommand.Execute(null);
    }
}
