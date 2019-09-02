/************************************
 * Author Name : Muhammad Hari      *
 * Author URI  : www.icodewizard.com  *
 ************************************/


using Niagapos.Core;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Niagapos
{
    /// <summary>
    /// Focuses (keyboard focus) this element on load
    /// </summary>
    public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Control control))
                return;

            control.Loaded += (dd, ee) => control.Focus();

        }
    }

    /// <summary>
    /// Focuses (keyboard focus) this element if true
    /// </summary>
    public class FocusProperty : BaseAttachedProperty<FocusProperty, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Control control))
                return;

            if ((bool)e.NewValue)
                control.Focus();
        }
    }

    /// <summary>
    /// Focuses (keyboard focus) and select all text in this element if true 
    /// </summary>
    public class FocusAndSelectProperty : BaseAttachedProperty<FocusAndSelectProperty, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBoxBase control)
            {
                if ((bool)e.NewValue)
                {
                    control.Focus();

                    control.SelectAll();
                }
            }

            if (d is PasswordBox password)
            {
                if ((bool)e.NewValue)
                {
                    password.Focus();

                    // Select all text
                    password.SelectAll();
                }
            }


        }
    }




    /// <summary>
    /// Only numeric or numbers can allowed on textbox if true
    /// </summary>
    public class IsNotAllowedCharacters : BaseAttachedProperty<IsNotAllowedCharacters, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (!(d is TextBox textBox))
                return;

            textBox.PreviewTextInput -= TextBox_PreviewTextInput;

            if ((bool)e.NewValue)
            {
                textBox.PreviewTextInput += TextBox_PreviewTextInput;
                textBox.TextChanged += (ss, ee) =>
                {
                    if (textBox.Text.Length > 20)
                    {
                        DI.UI.ShowMessage(new MessageBoxDialogViewModel
                        {
                            Title = "Error",
                            Message = "Maximum numbers of digit is too long!"

                        });

                     
                    }

                };
            }

        }

        public void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            
        }

    }

    public class IsTextLengthHasChanged : BaseAttachedProperty<IsTextLengthHasChanged, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBox textBox))
                return;

            textBox.TextChanged -= TextBox_HasChanged;

            if ((bool)e.NewValue)
            {
                textBox.TextChanged += TextBox_HasChanged;
                
            }
        }

        public void TextBox_HasChanged(object sender, TextChangedEventArgs e)
        {
            if((sender is TextBox textBox))
            {
                if (textBox.Text.Length >= 1)
                {
                   // DI.Sales.CharacterCounterNotes = Convert.ToInt32(textBox.Text.Length);
                }

                if (textBox.Text.Length >= 250)
                {
                    DI.UI.ShowMessage(new MessageBoxDialogViewModel
                    {
                        Title = "Message",
                        Message = "The maximum number of characters isn't more than 250 that can be allowed.",
                        DialogsYesVisible = false
                    });

                    textBox.Text = string.Empty;
                   // DI.Sales.CharacterCounterNotes = 0;
                }
            }

           
        }
    }

    public class IsCapslockOn : BaseAttachedProperty<IsCapslockOn, bool>
    {
        public override void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PasswordBox passwordBox))
                return;

            passwordBox.PreviewKeyDown -= TextBox_PreviewKeyDown;

            if ((bool)e.NewValue)
            {
                passwordBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            }
        }

        public void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(sender is PasswordBox passwordBox)
            {
                if ((e.KeyboardDevice.GetKeyStates(Key.CapsLock) & KeyStates.Toggled) == KeyStates.Toggled)
                {
                    //DI.Login.CapsLockOnNotification = "Caps lock is ON.";
                }

                else
                {
                   // DI.Login.CapsLockOnNotification = "";
                }
               

            }
        }
    }

}

