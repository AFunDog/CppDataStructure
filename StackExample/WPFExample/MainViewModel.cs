using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WPFExample
{
    internal enum ExpressionCommandType
    {
        DeleteLast,
        Clear,
        Calculate
    }

    internal sealed partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _errorMessage = "";

        [ObservableProperty]
        private bool _isErrorInfoBarOpen = false;

        private StringBuilder _expressionBuilder = new();

        public string ShowExpression => _expressionBuilder.ToString();

        private string CalculateExpression => _expressionBuilder.Replace('×', '*').ToString();

        [RelayCommand]
        private void ExpressionAppend(char c)
        {
            _expressionBuilder.Append(c);
            OnPropertyChanged(nameof(ShowExpression));
        }

        [RelayCommand]
        private void ExpressionCommand(ExpressionCommandType type)
        {
            switch (type)
            {
                case ExpressionCommandType.DeleteLast:
                    if (_expressionBuilder.Length != 0)
                    {
                        _expressionBuilder.Remove(_expressionBuilder.Length - 1, 1);
                    }
                    break;
                case ExpressionCommandType.Clear:
                    _expressionBuilder.Clear();
                    break;
                case ExpressionCommandType.Calculate:
                    var res = Calculator.Calculate(CalculateExpression);
                    if (res.IsSuccess is false)
                    {
                        ErrorMessage = res.ErrorMessage;
                        IsErrorInfoBarOpen = true;
                    }
                    else
                    {
                        ErrorMessage = "";
                        IsErrorInfoBarOpen = false;
                    }
                    _expressionBuilder.Clear();
                    _expressionBuilder.Append(res.Result);
                    break;
            }
            OnPropertyChanged(nameof(ShowExpression));
        }
    }
}
