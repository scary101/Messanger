using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
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

namespace WpfApp7.View
{
    /// <summary>
    /// Логика взаимодействия для ClientView.xaml
    /// </summary>
    public partial class ClientView : Window
    {
        private Socket socket;
        private ObservableCollection<Chat> chatMessages = new ObservableCollection<Chat>();
        private CancellationTokenSource token;

        public ClientView(string serverIp, string username)
        {
            InitializeComponent();
            try
            {
                token = new CancellationTokenSource();
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(serverIp, 7777);
                SendUser(new User(username, null), token.Token);
                ListenMess(token.Token);
            }
            catch (SocketException ex)
            {

            }
        }

        private async void SendMessage(string message)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(bytes, SocketFlags.None);
            }
            catch (Exception ex)
            {

            }
        }

        private void But_Click(object sender, RoutedEventArgs e)
        {
            SendMessage(Mess_Tbx.Text);
        }

        private async void ListenMess(CancellationToken cancellation)
        {
            try
            {
                while (!cancellation.IsCancellationRequested)
                {
                    byte[] bytes = new byte[1024];
                    int bytesRead = await socket.ReceiveAsync(bytes, SocketFlags.None);
                    if (bytesRead > 0)
                    {
                        string jsonString = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                        Chat chatMessage = JsonConvert.DeserializeObject<Chat>(jsonString);
                        chatMessages.Add(chatMessage);
                        Main_Lbx.ItemsSource = chatMessages;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void SendUser(User user, CancellationToken cancellation)
        {
            if (!cancellation.IsCancellationRequested)
            {
                try
                {
                    string json = JsonConvert.SerializeObject(user);
                    var bytes = Encoding.UTF8.GetBytes(json);
                    await socket.SendAsync(bytes, SocketFlags.None);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            token.Cancel();
            socket.Close();
            VhodView vhod = new VhodView();
            vhod.Show();
            Close();
        }
    }
}
