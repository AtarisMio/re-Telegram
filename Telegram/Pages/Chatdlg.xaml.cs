using FirstFloor.ModernUI.Windows.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telegram.Pages.Controls;

namespace Telegram.Pages
{
    /// <summary>
    /// Interaction logic for chatdlg.xaml
    /// </summary>
    public partial class Chatdlg : UserControl
    {
        public Chatdlg()
        {
            InitializeComponent();
            
        }

        private void send_btn_Click(object sender, RoutedEventArgs e)
        {

            ChatList.Children.Add(new PopoChat(send_text.Text));
            send_text.Clear();
        }

        private void send_text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                send_btn_Click(sender,e);
        }
    }
}
