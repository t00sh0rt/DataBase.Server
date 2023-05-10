using System.Net;
using System;
using System.Net.Sockets;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace server
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath("Rooms1.json"))))) + "\\BD\\";// BD init
            DataBase dataBase = DataBase.InitBD(path);// BD init
            int choice;
            const int port = 8080; //Port любой
            var tcpEndPoint = new IPEndPoint(IPAddress.Any, port); // класс конечной точки (точка подключения), принимает Ip and Port
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // сокет объявляем, через него все проходит + прописываем дефолтные характеристики для TCP
            tcpSocket.Bind(tcpEndPoint); // Связываем сокет с конечной точкой (кого нужно слушать)
            tcpSocket.Listen(100); // кол-во челов, которые могут подключиться
            while (true)
            {
                dataBase = DataBase.InitBD(path);//подгружаем данные
                // обработчик на прием сообщения 
                var listener = tcpSocket.Accept(); //новый сокет, который обрабатывает клиента
                var buffer = new byte[256]; // массив байтов, куда будут приниматься сообщения
                var data = new StringBuilder();
                byte[] ChoiceByte = new byte[4];
                if (BusinessLogik.TryReceive(listener, ChoiceByte))//проверяем доступ к клиенту
                {
                    
                    choice = BitConverter.ToInt32(ChoiceByte, 0);
                    do
                    {

                        var size = listener.Receive(buffer); // в size записывается размерность реально полученных байт
                        data.Append(Encoding.UTF8.GetString(buffer, 0, size)); // переводим и записываем текст
                    }
                    while (listener.Available > 0);
                    var message = data.ToString();
                    Console.WriteLine("Принял сообщение:");
                    Console.WriteLine(message);

                    
                    string client = DataBase.GetUserObjectString(dataBase.userobject);
                    if (choice == 1)//отправить информацию про комнату по номеру комнаты
                    {
                        string roomx = DataBase.GetCurrentRoomString(dataBase,message);
                        Console.WriteLine($"Отправил{roomx}");
                        listener.Send(Encoding.UTF8.GetBytes(roomx)); //передаем какое-либо сообщение
                    }
                    if (choice == 2)//отправить информацию про бронь по айди
                    {
                        var res = DataBase.GetCurrentBookingString(dataBase, message);
                        Console.WriteLine($"Отправил{res}");
                        listener.Send(Encoding.UTF8.GetBytes(res));
                    }
                    if (choice == 3)//отправить информацию про пользователя по айди
                    {

                    }
                    if (choice == 4)//создать бронь и отправить её инфу
                    {
                        var result = BusinessLogik.Book(dataBase, message.Split("'")[1], message.Split("'")[2], message.Split("'")[3], message.Split("'")[0]);
                        if (!(result.Contains("wrongCount")||result.Contains("wrongDate")||result==null))
                        {
                            var booking =DataBase.InitBooking(result);
                            DataBase.AddBooking(dataBase, booking);
                            dataBase.SaveBD();
                            dataBase = DataBase.InitBD(path);
                        }
                        Console.WriteLine("Oтправил: "+result);
                        listener.Send(Encoding.UTF8.GetBytes(result));
                        

                    }
                    if (choice == 5)//создать пользователя и отправить код ошибки если возникла ошибка
                    {
                        var user = DataBase.InitUser(message);
                        var check = BusinessLogik.CheckUserCreation(dataBase, user);
                        if (check.Contains('e') || check.Contains('l') || check.Contains('n'))
                        {
                            Console.WriteLine($"Ошибка:\n{check}\ne-аккаунт с данным email уже существует\nl-аккаунт с данным логином уже существует\nn-аккаунт с данным номером уже существует");
                            listener.Send(Encoding.UTF8.GetBytes(check));
                        }
                        else
                        {
                            Console.WriteLine("Создан аккаунт:\n");
                            user.id = BusinessLogik.UserIdCreation(dataBase);
                            Console.WriteLine(DataBase.GetUserString(user));
                            DataBase.AddUser(dataBase, user);
                            dataBase.SaveBD();
                            dataBase = DataBase.InitBD(path);


                        }

                    }
                    if (choice == 6)//вход в аккаунт
                    {
                        var res = BusinessLogik.CheckLogin(dataBase, message.Split("'")[0], message.Split("'")[1]);
                        Console.WriteLine($" отправил {res}");
                        listener.Send(Encoding.UTF8.GetBytes(res));
                    }
                    if (choice == 7)//отправить инфу про бронь по айди пользователя
                    {
                        var res = BusinessLogik.GetBookings(dataBase,message);
                        listener.Send(Encoding.UTF8.GetBytes(res));
                        Console.WriteLine($" отправил {res}");
                    }
                    if (choice == 8)//удалить бронь по её номеру
                    {
                        var res = BusinessLogik.DeleteBooking(dataBase,message);
                        listener.Send(Encoding.UTF8.GetBytes(res));
                        dataBase.SaveBD();
                        dataBase = DataBase.InitBD(path);
                        Console.WriteLine($" отправил {res}");
                        Console.WriteLine($"ok-удалили, wrong-ошибка такой брони нет");
                    }
                    listener.Shutdown(SocketShutdown.Both); // отключаем и у клиента, и у сервера
                    listener.Close(); // закрываем
                }
            }
        }
    }
}
