using System.Net;
using System;
using System.Net.Sockets;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace server
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string gog;
            string message;
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath("Rooms1.json"))))) + "\\BD\\";// BD init
            DataBase dataBase = DataBase.InitBD(path);// BD init
            char[] symbols = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            int choice;
            const string ip = "127.0.0.1"; //Ip локальный  
            const int port = 8080; //Port любой
            var tcpEndPoint = new IPEndPoint(IPAddress.Any, port); // класс конечной точки (точка подключения), принимает Ip and Port
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // сокет объявляем, через него все проходит + прописываем дефолтные характеристики для TCP
            tcpSocket.Bind(tcpEndPoint); // Связываем сокет с конечной точкой (кого нужно слушать)
            tcpSocket.Listen(100); // кол-во челов, которые могут подключиться


            DataBase.SortById(dataBase.roomobject);


            
            while (true)
            {
                // обработчик на прием сообщения 
                var listener = tcpSocket.Accept(); //новый сокет, который обрабатывает клиента
                var buffer = new byte[256]; // массив байтов, куда будут приниматься сообщения
                var size = 0;
                var data = new StringBuilder();
                byte[] ChoiceByte = new byte[4];
                if (BusinessLogik.TryReceive(listener, ChoiceByte))
                {
                    
                    choice = BitConverter.ToInt32(ChoiceByte, 0);
                    Console.WriteLine(choice);
                    do
                    {

                        size = listener.Receive(buffer); // в size записывается размерность реально полученных байт
                        data.Append(Encoding.UTF8.GetString(buffer, 0, size)); // переводим и записываем текст
                    }
                    while (listener.Available > 0);
                    message = data.ToString();
                    Console.WriteLine(message);
                    
                    string client = DataBase.GetUserObjectString(dataBase.userobject);
                    if (choice == 1)
                    {
                        string roomx = DataBase.GetCurrentRoomString(dataBase,message);
                        Console.WriteLine(roomx);
                        listener.Send(Encoding.UTF8.GetBytes(roomx)); //передаем какое-либо сообщение
                    }
                    if (choice == 2)
                    {
                        listener.Send(Encoding.UTF8.GetBytes(client));
                    }
                    if (choice == 3)
                    {
                        Random random = new Random((int)DateTime.Now.Ticks);
                        string BookingId = "";
                        for (int i = 0; i < 4; i++)
                        {
                            BookingId += (symbols[random.Next(symbols.Length)]);
                        }
                        Console.WriteLine(BookingId);
                        var user = DataBase.InitUser(message);
                        user.id = BookingId.ToString();
                        DataBase.AddUser(dataBase, user);

                        listener.Send(Encoding.UTF8.GetBytes(BookingId));
                    }
                    if (choice == 4)
                    {
                        listener.Send(Encoding.UTF8.GetBytes(DataBase.GetCurrentUserString(dataBase,message )));
                    }

                    
                }
                listener.Shutdown(SocketShutdown.Both); // отключаем и у клиента, и у сервера
                listener.Close(); // закрываем
            }
        }
    }
}
