using System.Net;
using System;
using System.Net.Sockets;
using System.Text;
using DataBase.BD;
using server.BL;

namespace server
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string gog;



            string message;
            // BD init
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath("Rooms1.json"))))) + "\\BD\\";
            DataBase.BD.DataBase dataBase = DataBase.BD.DataBase.InitBD(path);

            //gog = server.BL.BusinessLogik.FindUser("petya1234", "petya1234", dataBase);

            //Console.WriteLine(gog);
            int choice;
            int BookingId = 2;

            const string ip = "127.0.0.1"; //Ip ���������  
            const int port = 8080; //Port �����

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // ����� �������� ����� (����� �����������), ��������� Ip and Port

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // ����� ���������, ����� ���� ��� �������� + ����������� ��������� �������������� ��� TCP


            tcpSocket.Bind(tcpEndPoint); // ��������� ����� � �������� ������ (���� ����� �������)
            tcpSocket.Listen(100); // ���-�� �����, ������� ����� ������������
            DataBase.BD.User user = new User();

            while (true)
            {

                // ���������� �� ����� ��������� 
                var listener = tcpSocket.Accept(); //����� �����, ������� ������������ �������
                var buffer = new byte[256]; // ������ ������, ���� ����� ����������� ���������
                var size = 0;
                var data = new StringBuilder();

                byte[] ChoiceByte = new byte[4];
                listener.Receive(ChoiceByte);

                choice = BitConverter.ToInt32(ChoiceByte, 0);

                Console.WriteLine(choice);

                do
                {
                    size = listener.Receive(buffer); // � size ������������ ����������� ������� ���������� ����
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size)); // ��������� � ���������� �����
                }
                while (listener.Available > 0);


                message = data.ToString();
                Console.WriteLine(message);


                string roomx = DataBase.BD.DataBase.GetRoomObjectString(dataBase.roomobject);
                string client = DataBase.BD.DataBase.GetUserObjectString(dataBase.userobject);
                if (choice == 1)
                {
                    listener.Send(Encoding.UTF8.GetBytes(roomx)); //�������� �����-���� ���������
                }
                if (choice == 2)
                {
                    listener.Send(Encoding.UTF8.GetBytes(client));
                }
                if (choice == 3)
                {
                    if (BookingId > 1000)
                    {
                        BookingId = 0;
                    }
                    BookingId++;
                    user = DataBase.BD.DataBase.InitUser(message);
                    user.id = BookingId;
                    DataBase.BD.DataBase.AddUser(dataBase, user);

                    listener.Send(Encoding.UTF8.GetBytes(BookingId.ToString()));
                }
                if (choice == 4)
                {
                    listener.Send(Encoding.UTF8.GetBytes(server.BL.BusinessLogik.FindUser(message, dataBase)));
                }

                listener.Shutdown(SocketShutdown.Both); // ��������� � � �������, � � �������
                listener.Close(); // ���������
            }
        }
    }
}
