using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.ObjectModel;
using System.Windows;
using Newtonsoft.Json;

namespace WpfApp7.Model
{
    internal class TestTcp
    {
        private Socket socket;
        public List<Socket> clients = new List<Socket>();
        public ObservableCollection<User> users = new ObservableCollection<User>();
        public ObservableCollection<Chat> chat = new ObservableCollection<Chat>();
        public ObservableCollection<string> logo = new ObservableCollection<string>();
        private LogoMes logoMes = new LogoMes();
        private CancellationTokenSource token = new CancellationTokenSource();
        public string servername;

        public void DropServer()
        {
            token.Cancel();
            socket.Close();
        }

        public void StartServer()
        {
            IPEndPoint point = new IPEndPoint(IPAddress.Any, 7777);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            logo.Add(logoMes.ServerCreate());
            socket.Bind(point);
            socket.Listen(10);
            ListenClients();
        }

        private async Task ListenClients()
        {
            while (!token.IsCancellationRequested)
            {
                var client = await socket.AcceptAsync();
                clients.Add(client);
                AddUser(client);
                ReceiveMessage(client);
                string remoteIpAddress = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
                var user = users.FirstOrDefault(u => u.IpAdd == remoteIpAddress);
                SendUser(user);
                logo.Add(logoMes.ClientConnect(user));
            }
        }

        private async Task ReceiveMessage(Socket client)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    byte[] bytes = new byte[1024];
                    int bytesRead = await client.ReceiveAsync(bytes, SocketFlags.None);
                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                        string timeStamp = DateTime.Now.ToString("HH:mm:ss");
                        string remoteIpAddress = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
                        var user = users.FirstOrDefault(u => u.IpAdd == remoteIpAddress);
                        string userName = user != null ? user.username : "Unknown User";
                        var chatMessage = new Chat(userName, timeStamp, message);
                        chat.Add(chatMessage);
                        foreach (var i in clients)
                        {
                            await SendMessage(i, chatMessage);
                        }
                    }
                }
            }
            catch (SocketException)
            {
                clients.Remove(client);
                RemoveUser(client);
            }
        }

        private async Task SendMessage(Socket client, Chat chatMessage)
        {
            if (!token.IsCancellationRequested)
            {
                string jsonMessage = JsonConvert.SerializeObject(chatMessage);
                byte[] bytes = Encoding.UTF8.GetBytes(jsonMessage);
                await client.SendAsync(bytes, SocketFlags.None);
                string ip = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
                var user = users.FirstOrDefault(u => u.IpAdd == ip);
                logo.Add(logoMes.ClientSendMessage(user));
            }

        }

        public async Task SendMessageServer(string message)
        {
            if (!token.IsCancellationRequested)
            {
                User ser = new User("Server",IPAddress.Any.ToString());
                string timeStamp = DateTime.Now.ToString("HH:mm:ss");
                Chat messerv = new Chat(servername, timeStamp, message);
                chat.Add(messerv);
                logo.Add(logoMes.ClientSendMessage(ser));
                foreach (var client in clients)
                {
                    await SendMessage(client, messerv);
                }
            }


        }
        private async Task SendUser(User user)
        {
            try
            {
                string json = JsonConvert.SerializeObject(user);
                var bytes = Encoding.UTF8.GetBytes(json);

                foreach (var client in clients)
                {
                    await client.SendAsync(bytes, SocketFlags.None);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void AddUser(Socket client)
        {
            string remoteIpAddress = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
            NetworkStream stream = new NetworkStream(client);
            byte[] data = new byte[1024];
            StringBuilder builder = new StringBuilder();
            int bytesRead = 0;
            do
            {
                bytesRead = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.UTF8.GetString(data, 0, bytesRead));
            } while (stream.DataAvailable);

            string receivedData = builder.ToString();
            User user = JsonConvert.DeserializeObject<User>(receivedData);
            user.IpAdd = remoteIpAddress;

            users.Add(user);
        }

        private void RemoveUser(Socket client)
        {
            string remoteIpAddress = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
            var userToRemove = users.FirstOrDefault(u => u.IpAdd == remoteIpAddress);
            if (userToRemove != null)
            {
                users.Remove(userToRemove);
                logo.Add(logoMes.ClientDisconnect(userToRemove));
            }
        }
    }


}
