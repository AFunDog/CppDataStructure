using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace WPFExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        internal MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
            expTextBox.Focus();
        }

        private void AppendLeftRoundButton_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('(');

        private void AppendRightRoundButton_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute(')');

        private void DeleteLastButton_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionCommandCommand.Execute(ExpressionCommandType.DeleteLast);

        private void AppendDivButton_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('/');

        private void Append7Button_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('7');

        private void Append8Button_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('8');

        private void Append9Button_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('9');

        private void AppendMulButton_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('×');

        private void Append4Button_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('4');

        private void Append5Button_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('5');

        private void Append6Button_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('6');

        private void AppendSubButton_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('-');

        private void Append1Button_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('1');

        private void Append2Button_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('2');

        private void Append3Button_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('3');

        private void AppendAddButton_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('+');

        private void Append0Button_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('0');

        private void AppendPointButton_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionAppendCommand.Execute('.');

        private void StartCalculateButton_OnClicked(object sender, RoutedEventArgs e) =>
            ViewModel.ExpressionCommandCommand.Execute(ExpressionCommandType.Calculate);

        private void ExpressionTextBox_OnPreviewTextInputed(
            object sender,
            TextCompositionEventArgs e
        )
        {
            if (e.Handled)
                return;
            e.Handled = true;
            foreach (var inputC in e.Text)
            {
                switch (inputC)
                {
                    case >= '0'
                    and <= '9'
                    or '.'
                    or '('
                    or ')'
                    or '+'
                    or '-'
                    or '/':
                        ViewModel.ExpressionAppendCommand.Execute(inputC);
                        break;
                    case '*':
                        ViewModel.ExpressionAppendCommand.Execute('×');
                        break;
                    default:
                        e.Handled = false;
                        break;
                }
            }
        }

        private void ExpressionTextBox_OnPreviewKeyDowned(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;
            e.Handled = true;
            switch (e.Key)
            {
                case Key.Enter:
                    ViewModel.ExpressionCommandCommand.Execute(ExpressionCommandType.Calculate);
                    break;
                case Key.Back:
                    ViewModel.ExpressionCommandCommand.Execute(ExpressionCommandType.DeleteLast);
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }
    }
}
