﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Editor
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnProprietyChanged(string proprietyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(proprietyName));
        }
    }
}