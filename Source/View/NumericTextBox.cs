using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace RollingLineSavegameFix.View
{
    public class NumericTextBox : TextBox
    {
        public static readonly DependencyProperty IsEnabledProperty = 
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(NumericTextBox), new PropertyMetadata(PropertyChanged));

        private static void PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            TextBox textBox = (TextBox)dependencyObject;
            textBox.PreviewTextInput += OnPreviewTextInput;
        }

        private static void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (GetIsEnabled((TextBox)sender))
            {
                foreach (char charItem in e.Text)
                {
                    if (!Char.IsNumber(charItem))
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        public static bool GetIsEnabled(TextBox textBox)
        {
            return (bool)textBox.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(TextBox textBox, bool isEnabled)
        {
            textBox.SetValue(IsEnabledProperty, isEnabled);
        }
    }
}
