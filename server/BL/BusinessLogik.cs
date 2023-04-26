using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataBase.BD;

namespace server.BL
{
    internal class BusinessLogik
    {
        public static string FindUser(string Login, string Password, DataBase.BD.DataBase dataBase)
        {
            string user="0";

            for (int i = 0; i < dataBase.userobject.users.Length; i++)
            {
                if ((dataBase.userobject.users[i].password == Password) && (dataBase.userobject.users[i].login == Login)){
                    user = DataBase.BD.DataBase.GetCurrentUserString(dataBase.userobject.users[i]);
                    break;
                }
                else if ((dataBase.userobject.users[i].password != Password) || (dataBase.userobject.users[i].login != Login))
                {
                    user = "ошибка";
                }
            }
            return (user);
        }

        public static string ChooseFunction(string ip, int port, IPEndPoint tcpEndPoint, Socket tcpSocket)
        {

            
            while (true)
            {

                // класс конечной точки (точка подключения), принимает Ip and Port

                 // сокет объявляем, через него все проходит + прописываем дефолтные характеристики для TCP
                tcpSocket.Bind(tcpEndPoint); // Связываем сокет с конечной точкой (кого нужно слушать)
                tcpSocket.Listen(100); // кол-во челов, которые могут подключиться



                // обработчик на прием сообщения 
                var listener = tcpSocket.Accept(); //новый сокет, который обрабатывает клиента
                var buffer = new byte[256]; // массив байтов, куда будут приниматься сообщения
                var size = 0;
                var choice = new StringBuilder();

                do
                {
                    size = listener.Receive(buffer); // в size записывается размерность реально полученных байт
                    choice.Append(Encoding.UTF8.GetString(buffer, 0, size)); // переводим и записываем текст
                }
                while (listener.Available > 0);






                listener.Shutdown(SocketShutdown.Both); // отключаем и у клиента, и у сервера
                listener.Close(); // закрываем

                return (choice.ToString());
                break;
            }
        }
    }
}
