using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Controls;
using WPFExample.Controls;

namespace WPFExample
{
    internal partial class MainViewModel : ObservableObject
    {
        public IReadOnlyCollection<Friend> Friends { get; set; } = FriendManager.FriendList;

        static ContentPresenter DialogPresenter =>
            ((MainWindow)Application.Current.MainWindow).rootContentPresenter;

        [ObservableProperty]
        private int _selectedFriendId = -1;

        [ObservableProperty]
        private int _selectedFriendIndex = -1;

        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(SearchMenuItemVisibility),
            nameof(CancelSearchMenuItemVisibility)
        )]
        private bool _isSearchMode = false;

        [ObservableProperty]
        private ObservableCollection<string> _searchs = [];

        public Visibility SearchMenuItemVisibility =>
            IsSearchMode ? Visibility.Collapsed : Visibility.Visible;
        public Visibility CancelSearchMenuItemVisibility =>
            IsSearchMode ? Visibility.Visible : Visibility.Collapsed;

        //public void NotifyFriendsChanged()
        //{
        //    OnPropertyChanged(nameof(Friends));
        //}


        public MainViewModel() { }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Function();

        [RelayCommand]
        private async Task StartAddAsync(int index)
        {
            await OpenInfoPanelAsync(
                "添加好友",
                model =>
                {
                    FriendManager.Input.WriteLine(
                        $"insert {index}\n{model.Name}\n{model.BirthYear} {model.BirthMonth}\n{model.PicPath}\n{model.Hobby}"
                    );
                    FriendManager.Input.WriteLine("selectAll");
                }
            );
        }

        [RelayCommand]
        private async Task StartEditAsync()
        {
            await OpenInfoPanelAsync(
                "编辑好友",
                model =>
                {
                    FriendManager.Input.WriteLine(
                        $"update {SelectedFriendId}\n{model.Name}\n{model.BirthYear} {model.BirthMonth}\n{model.PicPath}\n{model.Hobby}"
                    );
                    FriendManager.Input.WriteLine("selectAll");
                },
                Friends.FirstOrDefault(x => x.Info.Id == SelectedFriendId)
            );
        }

        [RelayCommand]
        private void LoadLocalData()
        {
            FriendManager.Input.WriteLine("load");
            FriendManager.Input.WriteLine("selectAll");
        }

        [RelayCommand]
        private void SaveLocalData()
        {
            FriendManager.Input.WriteLine("save");
        }

        [RelayCommand]
        private void RemoveFriend()
        {
            FriendManager.Input.WriteLine($"remove {SelectedFriendId}");
            FriendManager.Input.WriteLine("selectAll");
        }

        [RelayCommand]
        private async Task StartClearFriendAsync()
        {
            var content = new System.Windows.Controls.TextBlock() { Text = "确认清空所有数据？" };
            var dialog = new ContentDialog
            {
                IsPrimaryButtonEnabled = true,
                PrimaryButtonAppearance = ControlAppearance.Primary,
                PrimaryButtonText = "确认",
                CloseButtonAppearance = ControlAppearance.Secondary,
                CloseButtonText = "取消",
                DialogHost = DialogPresenter,
                Content = content,
            };
            if (await dialog.ShowAsync() is ContentDialogResult.Primary)
            {
                FriendManager.Input.WriteLine("clear");
                FriendManager.Input.WriteLine("selectAll");
            }
        }

        [RelayCommand]
        private async Task StartSearchFriendAsync()
        {
            await OpenInfoPanelAsync(
                "搜索",
                model =>
                {
                    List<string> searchs = [];
                    if (string.IsNullOrEmpty(model.Name) is false)
                    {
                        searchs.Add($"name {model.Name}");
                        Searchs.Add($"姓名 为 {model.Name}");
                    }
                    if (model.BirthYear != 0)
                    {
                        searchs.Add($"birthYear {model.BirthYear}");
                        Searchs.Add($"出生年份 为 {model.BirthYear}");
                    }
                    if (model.BirthMonth != 0)
                    {
                        searchs.Add($"birthMonth {model.BirthMonth}");
                        Searchs.Add($"出生月份 为 {model.BirthMonth}");
                    }
                    if (string.IsNullOrEmpty(model.PicPath) is false)
                    {
                        searchs.Add($"picPath {model.PicPath}");
                        Searchs.Add($"照片路径 为 {model.PicPath}");
                    }
                    if (string.IsNullOrEmpty(model.Hobby) is false)
                    {
                        searchs.Add($"hobby {model.Hobby}");
                        Searchs.Add($"爱好 为 {model.Hobby}");
                    }
                    FriendManager.Input.WriteLine($"select {searchs.Count}");
                    foreach (string s in searchs)
                    {
                        FriendManager.Input.WriteLine(s);
                    }
                    //FriendManager.Input.WriteLine("selectAll");
                    IsSearchMode = true;
                },
                new Friend(),
                model =>
                {
                    return true;
                }
            );
        }

        [RelayCommand]
        private void CancelSearchMode()
        {
            IsSearchMode = false;
            FriendManager.Input.WriteLine("selectAll");
            Searchs.Clear();
        }

        private async Task OpenInfoPanelAsync(
            string title,
            Action<EditFriendContentModel> resultAction,
            Friend? initValue = null,
            Func<EditFriendContentModel, bool>? checkFunc = null
        )
        {
            var content = new EditFriendContent();
            var dialog = new ContentDialog
            {
                IsPrimaryButtonEnabled = true,
                PrimaryButtonAppearance = ControlAppearance.Primary,
                PrimaryButtonText = "确认",
                CloseButtonAppearance = ControlAppearance.Secondary,
                CloseButtonText = "取消",
                DialogHost = DialogPresenter,
                Content = content,
            };
            bool accept = false;
            checkFunc ??= DefCheckFunc;
            content.Model.ContentTitle = title;
            if (initValue is not null)
            {
                content.Model.Name = initValue.Name;
                content.Model.BirthYear = initValue.BirthYear;
                content.Model.BirthMonth = initValue.BirthMonth;
                content.Model.PicPath = initValue.PicPath;
                content.Model.Hobby = initValue.Hobby;
            }
            do
            {
                var res = await dialog.ShowAsync();
                if (res is ContentDialogResult.Primary)
                {
                    if (checkFunc(content.Model) is false)
                    {
                        continue;
                    }
                    accept = true;
                }
                else
                {
                    return;
                }
            } while (accept is false);
            resultAction(content.Model);

            static bool DefCheckFunc(EditFriendContentModel Model)
            {
                if (string.IsNullOrEmpty(Model.Name))
                {
                    Model.InfoBarMessage = "名称不能为空";
                    Model.IsInfoBarOpen = true;
                    Model.InfoBarSeverity = InfoBarSeverity.Error;
                    return false;
                }
                if (string.IsNullOrEmpty(Model.PicPath))
                {
                    Model.InfoBarMessage = "头像路径不能为空";
                    Model.IsInfoBarOpen = true;
                    Model.InfoBarSeverity = InfoBarSeverity.Error;
                    return false;
                }
                if (
                    Path.Exists(Model.PicPath) is false
                    && Path.Exists(Path.Combine(Directory.GetCurrentDirectory(), Model.PicPath))
                        is false
                )
                {
                    Model.InfoBarMessage = "头像路径无效";
                    Model.IsInfoBarOpen = true;
                    Model.InfoBarSeverity = InfoBarSeverity.Error;
                    return false;
                }
                if (string.IsNullOrEmpty(Model.Hobby))
                {
                    Model.InfoBarMessage = "爱好不能为空";
                    Model.IsInfoBarOpen = true;
                    Model.InfoBarSeverity = InfoBarSeverity.Error;
                    return false;
                }
                return true;
            }
        }
    }
}
