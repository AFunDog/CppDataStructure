using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFExample
{
    internal static class FriendManager
    {
        static Process? Process { get; set; }
        static Task? ProcessInTask { get; set; }
        static Task? ProcessOutTask { get; set; }
        static Task? ProcessErrTask { get; set; }

        static ObservableCollection<Friend> Friends { get; } = [];

        private static StreamReader? _output;
        public static StreamReader Output => _output ?? throw new InvalidOperationException("未初始化");
        private static StreamWriter? _input;
        public static StreamWriter Input => _input ?? throw new InvalidOperationException("未初始化");
        private static StreamReader? _error;
        public static StreamReader Error => _error ?? throw new InvalidOperationException("未初始化");
        public static IReadOnlyCollection<Friend> FriendList => Friends;

        public static void Init()
        {
            Process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "FriendManager.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };
            Process.Start();
            _input = Process.StandardInput;
            _output = Process.StandardOutput;
            _error = Process.StandardError;
            ProcessInTask = Task.Run(async () =>
            {
                while (Process.HasExited is false)
                {
                    while (
                        await Output.ReadLineAsync() is string data
                        && int.TryParse(data, out var count)
                    )
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(() =>
                        {
                            Friends.Clear();
                        });
                        try
                        {
                            for (int i = 0; i < count; i++)
                            {
                                var f = new Friend();
                                f.Info = f.Info with { Id = int.Parse(Output.ReadLine()!) };
                                f.Name = Output.ReadLine()!;
                                var ym = Output.ReadLine()!.Split();
                                f.BirthYear = int.Parse(ym[0]);
                                f.BirthMonth = int.Parse(ym[1]);
                                f.PicPath = Output.ReadLine()!;
                                f.Hobby = Output.ReadLine()!;
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    Friends.Add(f);
                                });
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine($"Error {e}");
                        }
                    }
                }
            });
            ProcessOutTask = Task.Run(async () => { });
            ProcessErrTask = Task.Run(async () =>
            {
                while (Process.HasExited is false)
                {
                    while (
                        await Error.ReadLineAsync() is string err
                        && (string.IsNullOrEmpty(err) is false)
                    )
                    {
                        Debug.WriteLine(err);
                    }
                }
            });
            Input.WriteLine("load");
            Input.WriteLine("selectAll");
        }

        public static async void End()
        {
            if (Process is not null)
            {
                Input.WriteLine("exit");
                Process.WaitForExit();

                if (ProcessInTask is not null)
                {
                    await ProcessInTask;
                    ProcessInTask.Dispose();
                    ProcessInTask = null;
                }

                if (ProcessOutTask is not null)
                {
                    await ProcessOutTask;
                    ProcessOutTask.Dispose();
                    ProcessOutTask = null;
                }

                if (ProcessErrTask is not null)
                {
                    await ProcessErrTask;
                    ProcessErrTask.Dispose();
                    ProcessErrTask = null;
                }

                Process.Dispose();
                Process = null;
            }
        }
    }
}
