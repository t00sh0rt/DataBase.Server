using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace server
{
    internal class BusinessLogik
    {
        public static bool TryReceive(Socket listener, byte[] urByte)
        {
            try
            {
                listener.Receive(urByte);
                //Console.WriteLine("Connected!");
            }
            catch (SocketException e)
            {   //комменты для того чтобы узнать ошибку... алгоритм взял с сайта майкрософака...
                //Console.WriteLine("Source : " + e.Source);
                //Console.WriteLine("Message : " + e.Message); 
                return false;
            }
            catch (Exception e)
            {
                //Console.WriteLine("Source : " + e.Source);
                //Console.WriteLine("Message : " + e.Message);
                return false;
            }
            return true;
        }
        public static string FindUserToSend(string bookingId, DataBase dataBase) //возвращает юзера по id
        {
            for (int i = 0; i < dataBase.userobject.users.Length; i++)
            {
                if (dataBase.userobject.users[i].id == bookingId)
                {
                    string user = DataBase.GetCurrentUserString(dataBase,bookingId);
                    return (user);
                }
            }
            return null;
        }

        public static string GetRoom(string roomId, DataBase dataBase) //возвращает комнату по ее id
        {
            for (int i = 0; i < dataBase.roomobject.rooms.Length; i++)
            {
                if (dataBase.roomobject.rooms[i].id == roomId)
                {
                    string roomx = DataBase.GetCurrentRoomString(dataBase, roomId);
                    return (roomx);
                }
            }
            return null;
        }
    }
}
