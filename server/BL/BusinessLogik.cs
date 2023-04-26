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
        public static string FindUser(string bookingId, DataBase.BD.DataBase dataBase) //возвращает юзера по id
        {
            string user = "0";

            for (int i = 0; i < dataBase.userobject.users.Length; i++)
            {
                if (dataBase.userobject.users[i].id == Convert.ToInt64(bookingId))
                {
                    user = DataBase.BD.DataBase.GetCurrentUserString(dataBase.userobject.users[i]);
                    break;
                }
                else if (dataBase.userobject.users[i].id != Convert.ToInt64(bookingId))
                {
                    user = "ошибка";
                }
            }
            return (user);
        }

        public static string FindRoom(string roomId, DataBase.BD.DataBase dataBase) //возвращает комнату по ее id
        {
            string roomx = "0";

            for (int i = 0; i < dataBase.roomobject.rooms.Length; i++)
            {
                if (dataBase.roomobject.rooms[i].id == Convert.ToInt64(roomId))
                {
                    roomx = DataBase.BD.DataBase.GetCurrentUserString(dataBase.userobject.users[i]);
                    break;
                }
                else if (dataBase.userobject.users[i].id != Convert.ToInt64(roomId))
                {
                    roomx = "ошибка";
                }
            }
            return (roomx);
        
        }
    }
}
