using System.Net;
using System;
using System.Net.Sockets;
using System.Text;
using DataBase.BD;

namespace server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string message;
           // BD init
                string path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath("Rooms1.json"))))) + "\\BD\\";
            DataBase.BD.DataBase dataBase = DataBase.BD.DataBase.InitBD(path);

            const string ip = "127.0.0.1"; //Ip локальный
            const int port = 8080; //Port любой

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // класс конечной точки (точка подключения), принимает Ip and Port

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // сокет объявляем, через него все проходит + прописываем дефолтные характеристики для TCP
            tcpSocket.Bind(tcpEndPoint); // Связываем сокет с конечной точкой (кого нужно слушать)
            tcpSocket.Listen(100); // кол-во челов, которые могут подключиться

            while (true)
            {
                // обработчик на прием сообщения 
                var listener = tcpSocket.Accept(); //новый сокет, который обрабатывает клиента
                var buffer = new byte[256]; // массив байтов, куда будут приниматься сообщения
                var size = 0;
                var data = new StringBuilder();

                do
                {
                    size = listener.Receive(buffer); // в size записывается размерность реально полученных байт
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size)); // переводим и записываем текст
                }
                while (listener.Available > 0);

                message = data.ToString();
                Console.WriteLine(data.ToString());
                string roomx = DataBase.BD.DataBase.GetRoomObjectString(dataBase.roomobject);
                string client = DataBase.BD.DataBase.GetUserObjectString(dataBase.userobject);
                if (message == "1")
                {
                    listener.Send(Encoding.UTF8.GetBytes(roomx)); //передаем какое-либо сообщение
                }
                if (message == "2")
                {
                    listener.Send(Encoding.UTF8.GetBytes(client));
                }
                

               
                
                listener.Shutdown(SocketShutdown.Both); // отключаем и у клиента, и у сервера
                listener.Close(); // закрываем

                
            }
        }
    }
}