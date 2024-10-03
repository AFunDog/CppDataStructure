using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using Wpf.Ui.Controls;

namespace WPFExample.Controls
{
    internal sealed class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)(int)value;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        )
        {
            return (int)(double)value;
        }
    }

    internal partial class EditFriendContentModel : ObservableObject
    {
        [ObservableProperty]
        private string _contentTitle = "编辑窗口";

        [ObservableProperty]
        private string _name = "";

        [ObservableProperty]
        private int _birthYear = 0;

        [ObservableProperty]
        private int _birthMonth = 0;

        [ObservableProperty]
        private string _picPath = "Assets/icon.png";

        [ObservableProperty]
        private string _hobby = "无";

        [ObservableProperty]
        private bool _isInfoBarOpen = false;

        [ObservableProperty]
        private string _infoBarMessage = "";

        [ObservableProperty]
        private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Informational;
    }

    public partial class EditFriendContent : UserControl
    {
        internal EditFriendContentModel Model => (EditFriendContentModel)DataContext;

        public EditFriendContent()
        {
            DataContext = new EditFriendContentModel();
            InitializeComponent();
        }

        private void FilePickerButton_OnClicked(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog =
                new() { FileName = "Document", Filter = "图片|*.bmp;*.jpg;*.png;*.jpeg;*.gif" };
            if (dialog.ShowDialog() is true)
            {
                Model.PicPath = new Uri(Path.Combine(Environment.CurrentDirectory, " "))
                    .MakeRelativeUri(new Uri(dialog.FileName))
                    .ToString();
            }
        }
    }
}
