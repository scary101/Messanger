using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using WpfApp7.Model;
using WpfApp7.ViewModel.Helpers;

namespace WpfApp7
{
    internal class VhodViewModel : HelpCommand
    {
        private string servername;
        public string Servername { get { return servername; } set { servername = value; OnPropertyChanged(); } }
        public RelayCommand StartServer {  get; set; }

        public VhodViewModel()
        {
            StartServer = new RelayCommand(_ => OpenServer());
        }
        public void OpenServer()
        {
            if (CheckValidServerName())
            {
                MainWindow window = new MainWindow(Servername);
                Application.Current.MainWindow.Close();
                window.Show();
            }
            else
            {
                MessageBox.Show("Проверьте ввод данных");
            }
        }
        private bool CheckValidServerName()
        {
            Validation validation = new Validation();
            if(Servername != null && validation.ValidateString(Servername))
            {
                return true;
            }
            return false;
        }
    }
}
