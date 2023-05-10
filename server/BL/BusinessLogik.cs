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
using System.Runtime.Intrinsics.Arm;

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
        public static string GetBookings(DataBase dataBase,string user_id)
        {
            var result = "wrong";
            List<Booking> bookingsobjectList = new List<Booking>();
            for (int i = 0;i<dataBase.bookingobject.bookings.Length;i++)
            {
                if (dataBase.bookingobject.bookings[i].user_id == user_id)
                {
                    bookingsobjectList.Add(dataBase.bookingobject.bookings[i]);
                }
            }
            if (bookingsobjectList!=null)
            {
                var obj = bookingsobjectList.ToArray();
                var objRes = new Bookingobject();
                objRes.bookings = obj;
                result =DataBase.GetBookingObjectString(objRes);
                return result;
            }
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
        public static bool CheckDate(DataBase dataBase, string roomNumber, string comeDate, string outDate)
        {
            var ok=true;
            var cDcheck = DateTime.Parse(comeDate);
            var oDcheck = DateTime.Parse(outDate);
            for (int i = 0; i < dataBase.bookingobject.bookings.Length; i++)
            {   
                if (dataBase.bookingobject.bookings[i].room_number == roomNumber)
                {
                    var comeDateVar = DateTime.Parse(dataBase.bookingobject.bookings[i].comeDate);
                    var outDateVar = DateTime.Parse(dataBase.bookingobject.bookings[i].outDate);
                    if (cDcheck >= comeDateVar && cDcheck < outDateVar)
                    {
                        ok = false;
                        break;
                    }
                    if (cDcheck < comeDateVar && oDcheck > comeDateVar)
                    {
                        ok = false;
                        break;
                    }
                }
                
            }
            return ok;
        }
        public static string Book(DataBase dataBase, string roomTypeNumber, string comeDate, string outDate,string user_id)
        {   
            var room=DataBase.InitRoom(DataBase.GetRoomType(roomTypeNumber,dataBase));
            for (int i=0; i<dataBase.userobject.users.Length; i++)
            {
                if (dataBase.userobject.users[i].id== user_id)
                {
                    if (dataBase.userobject.users[i].bookingCount >= 5)
                    {
                        return "wrongCount";
                    }
                    for (int j = 0; j < room.numbers.Length; j++)
                    {
                        if (CheckDate(dataBase, room.numbers[j], comeDate, outDate))
                        {
                            var book = new Booking();
                            book.room_number = room.numbers[j];
                            book.comeDate = comeDate;
                            book.outDate = outDate;
                            book.user_id = user_id;
                            book.id = BookingNumberRandom(dataBase);
                            dataBase.userobject.users[i].bookingCount++;
                            return DataBase.GetBookingString(book);
                        }
                    }
                }
            }
            return "wrongDate";
        }
        public static string DeleteBooking(DataBase dataBase,string booking_id)
        {   for (int i = 0; i < dataBase.bookingobject.bookings.Length; i++)
            {
                if (dataBase.bookingobject.bookings[i].id == booking_id)
                {
                    for (int j = 0;j < dataBase.userobject.users.Length; j++)
                    {
                        if (dataBase.userobject.users[j].id== dataBase.bookingobject.bookings[i].user_id)
                        {
                            dataBase.userobject.users[j].bookingCount--;
                            break;
                        }
                    }
                    DataBase.RemoveBooking(dataBase, booking_id);
                    return "ok";
                }
            }
            return "wrong";
            
        }
    }
}
