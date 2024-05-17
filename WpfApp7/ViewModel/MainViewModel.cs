using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Serialization;
using WpfApp7.Model;
using WpfApp7.ViewModel.Helpers;

namespace WpfApp7.ViewModel
{
    public class MainViewModel : HelpCommand
    {
        private TestTcp server;
        private ObservableCollection<User> _users;
        private CancellationTokenSource cancellationTokenSource;
        public ObservableCollection<User> users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(users));
            }
        }

        private ObservableCollection<Chat> _messages = new ObservableCollection<Chat>();

        public ObservableCollection<Chat> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }

        private string _serverMes;
        public string Servermes
        {
            get { return _serverMes; }
            set
            {
                _serverMes = value;
                OnPropertyChanged(nameof(Servermes));
            }
        }

        public RelayCommand Send { get; set; }
        public RelayCommand LogOpen { get; set; }
        public RelayCommand CloseServer { get; set; }

        public MainViewModel(string servername)
        {
            server = new TestTcp();
            server.servername = servername;
            users = server.users;
            Messages = server.chat;
            server.StartServer();
            Send = new RelayCommand(_ => SendServerMessage());
            LogOpen = new RelayCommand(_ => OpenLogo());
            CloseServer = new RelayCommand(_ => ServerClose());
            User serv = new User(servername, IPAddress.Any.ToString());
            server.users.Add(serv);
        }
        private void SendServerMessage()
        {

            if (Servermes == "/disconect")
            {
                server.DropServer();
            }
            else
            {
                server.SendMessageServer(Servermes);
            }
        }
        private void OpenLogo()
        {
            LogoView logoView = new LogoView(server.logo);
            logoView.Show();
        }
        public void WindowClosing(object sender, CancelEventArgs e)
        {
            VhodView vhod = new VhodView();
            vhod.Show();
            server.DropServer();
        }
        public void ServerClose()
        {
            MessageBox.Show("Кнопка ушла в отпуск, закрывать через крестик!!!");
        }
    }
}
