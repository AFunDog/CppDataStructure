using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFExample
{
    internal partial class Friend : ObservableObject
    {
        internal struct BasicInfo
        {
            public int Id { get; set; }
            public int Index { get; set; }
        }

        public BasicInfo Info { get; set; }

        [ObservableProperty]
        private string _name = "";

        [ObservableProperty]
        private int _birthYear;

        [ObservableProperty]
        private int _birthMonth;

        [ObservableProperty]
        private string _picPath = "";

        [ObservableProperty]
        private string _hobby = "";
    }
}
