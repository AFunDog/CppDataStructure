using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFExample
{
    internal sealed record CalculateResult(bool IsSuccess, double Result, string ErrorMessage);

    internal static class Calculator
    {
        public static CalculateResult Calculate(string expression)
        {
            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "Calculator.exe",
                    Arguments = expression,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                }
            };
            process.Start();
            if (process.StandardOutput.ReadLine() is string res)
            {
                if (double.TryParse(res, out var calRes))
                {
                    return new(true, calRes, "");
                }
                else
                {
                    return new(false, 0, $"无效结果 {res}");
                }
            }
            else
            {
                return new(false, 0, process.StandardError.ReadLine()!.Replace("Error", "").Trim());
            }
        }
    }
}
