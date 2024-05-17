using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using WpfApp7.Model;
using WpfApp7.View;
using WpfApp7.ViewModel;
using WpfApp7.ViewModel.Helpers;

namespace WpfApp7
{
    public partial class VhodView : Window
    {
        public VhodView()
        {
            InitializeComponent();
            DataContext = new VhodViewModel();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string ipAddress = IpAddressTextBox.Text.Trim();
            string username = UsernameTextBox.Text.Trim();

            if (IsValidIp(ipAddress) && !string.IsNullOrEmpty(username))
            {
                ClientView client = new ClientView(ipAddress, username);
                client.Show();
                Close();
            }
            else
            {
            }
        }

        private bool IsValidIp(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out _);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Media media = new Media();
            media.Play();
        }
    }
}
