using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StopwatchOverlay
{
    /// <summary>
    /// Interaction logic for FirstTimeDialogue.xaml
    /// </summary>
    public partial class MessageBox : Window
    {
        public MessageBox()
        {
            InitializeComponent();
        }

        void AddButton(string text, MessageBoxResult result, bool isCancel = false)
        {
            var button = new Button() { Content = text, IsCancel = isCancel };
            button.Click += (o, args) => { Result = result; DialogResult = true; };
            ButtonContainer.Children.Add(button);
        }

        MessageBoxResult Result = MessageBoxResult.None;

        public static MessageBoxResult Show(string caption, string message)
        {
            var dialog = new MessageBox() { Title = caption };
            dialog.TitleContainer.Text = caption.ToUpper();
            dialog.MessageContainer.Text = message;
            dialog.AddButton("Continue", MessageBoxResult.None);
            dialog.ShowDialog();
            return dialog.Result;
        }
    }
}
