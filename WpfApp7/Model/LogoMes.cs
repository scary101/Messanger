using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp7.Model
{
    internal class LogoMes
    {
        public string ServerCreate()
        {
            DateTime time = DateTime.Now;

            string mes = $"({time}) Сервер успешно создан!";
            return mes;
        }
        public string ClientConnect(User user)
        {
            DateTime time = DateTime.Now;

            string mes = $"[{user.IpAdd}] ({time}) {user.username} подключился к серверу!";
            return mes;
        }
        public string ClientDisconnect(User user)
        {
            DateTime time = DateTime.Now;

            string mes = $"[{user.IpAdd}] ({time}) {user.username} отключился от сервера!";
            return mes;
        }
        public string ClientSendMessage(User user)
        {
            DateTime time = DateTime.Now;

            string mes = $"[{user.IpAdd}] ({time}) {user.username} отправил сообщение!";
            return mes;
        }

        public string ServerSendMessage()
        {
            DateTime time = DateTime.Now;

            string mes = $"({time}) Сервер отправил сообщение!";
            return mes;
        }

    }
}
