using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace server
{
    internal class BusinessLogik
    {
        public static bool TryReceive(Socket listener, byte[] urByte)//функция проеряет есть ли доступ к удаленному сокету
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
        public static string GetUser(string bookingId, DataBase dataBase) //возвращает юзера по id
        {
            var result = DataBase.GetCurrentUserString(dataBase, bookingId);
            if (result == null)
            {
                //warring
            }
            return result;
        }

        public static string GetRoom(string roomNumber, DataBase dataBase) //возвращает инфу про этот тип комнат по номеру комнаты
        {
            var result = DataBase.GetCurrentRoomString(dataBase, roomNumber);
            if (result == null)
            {
                //warring
            }
            return result;
        }
        public static string GetBooking(string bookingId, DataBase dataBase) //возвращает бронь по ее id
        {
            var result = DataBase.GetCurrentBookingString(dataBase, bookingId);
            if (result == null)
            {
                //warring
            }
            return result;
        }
        public static string BookingNumberRandom(DataBase dataBase)
        {
            string symbols = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random((int)DateTime.Now.Ticks);
            bool ok = true;
            while (true)
            {
                random = new Random((int)DateTime.Now.Ticks);
                string bookingId = "";
                for (int i = 0; i < 4; i++)
                {
                    bookingId += (symbols[random.Next(symbols.Length)]);
                }
                for (int i = 0; i < dataBase.bookingobject.bookings.Length; i++)
                {
                    if (dataBase.bookingobject.bookings[i].id == bookingId)
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    return bookingId;
                }
            }


        }
        public static string UserIdCreation(DataBase dataBase)
        {
            var maxId = 0;
            for (int i = 0; i < dataBase.userobject.users.Length; i++)
            {
                if (Convert.ToInt32(dataBase.userobject.users[i].id) >= maxId) { maxId = Convert.ToInt32(dataBase.userobject.users[i].id); }
            }
            maxId += 1;
            return maxId.ToString();
        }
        public static string CheckUserCreation(DataBase dataBase, User user)
        {
            var result = "";
            var e = false;
            var l = false;
            var n = false;
            for (int i = 0; i < dataBase.userobject.users.Length; i++)
            {
                if (user.email == dataBase.userobject.users[i].email) { e = true; }
                if (user.login == dataBase.userobject.users[i].login) { l = true; }
                if (user.number == dataBase.userobject.users[i].number) { n = true; }
            }
            if (e) { result += "e"; }
            if (l) { result += "l"; }
            if (n) { result += "n"; }
            return result;
        }

        public static string CheckLogin(DataBase dataBase, string nameLoginNumber, string password)
        {
            for (int i = 0; i < dataBase.userobject.users.Length; i++)
            {
                var user = dataBase.userobject.users[i];
                if (user.login == nameLoginNumber || user.email == nameLoginNumber || user.number == nameLoginNumber)
                {
                    if (user.password == password) return user.id;
                }
            }
            return "wrong";
        }
    }
}
