﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Editor.Dictionaries
{
    public partial class ControlTemplates :ResourceDictionary
    {
        private void OnTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            var bindingExp = textBox?.GetBindingExpression(TextBox.TextProperty);
            
            if (bindingExp == null) return;
            
            if(e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                e.Handled = true;

                if (textBox!.Tag is ICommand command && command.CanExecute(textBox.Text))
                {
                    command.Execute(textBox.Text);
                }
                else
                {
                    bindingExp?.UpdateSource();
                }
            }
            else if (e.Key == Key.Escape)
            {
                bindingExp?.UpdateSource();
                Keyboard.ClearFocus();
                (textBox?.Parent as Control)?.Focus();
            }
        }
    }
}
