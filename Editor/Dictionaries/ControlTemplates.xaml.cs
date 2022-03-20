using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Editor.Dictionaries
{
    public partial class ControlTemplates : ResourceDictionary
    {
        //Method initialised on pressing key for TextBoxes
        private void OnTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //Get the TextBox and it's text field binding expression
            var textBox = sender as TextBox;
            var bindingExp = textBox?.GetBindingExpression(TextBox.TextProperty);

            //If there is no binding expression, nothing to be done
            if (bindingExp == null) return;

            //Else, if the character pressed is enter
            if (e.Key == Key.Enter)
            {
                //Clear the focus of the keyboard and handle the event
                Keyboard.ClearFocus();
                e.Handled = true;
                //Give focus to the parent of the textbox, if there is one
                (textBox?.Parent as Control)?.Focus();

                //If there is a command that can be executed, execute it
                if (textBox!.Tag is ICommand command && command.CanExecute(textBox.Text))
                {
                    command.Execute(textBox.Text);
                }
                else
                {
                    //Else Update the sourse
                    bindingExp.UpdateSource();
                }
            }
            //If the key pressed is escape
            else if (e.Key == Key.Escape)
            {
                //Update the binding expression
                bindingExp.UpdateSource();
                //Lose keyboard focus and set it to the parent of the textbox uf there is one
                Keyboard.ClearFocus();
                (textBox?.Parent as Control)?.Focus();
            }
        }
    }
}
