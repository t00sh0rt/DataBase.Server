using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace server
{






    public class Roomobject
    {
        public Room[] rooms { get; set; }
    }

    public class Room
    {
        public string name { get; set; }
        public string quality { get; set; }
        public string description { get; set; }
        public string seats { get; set; }
        public string price { get; set; }
        public string[] numbers { get; set; }
        public string id { get; set; }
    }


    public class Userobject
    {
        public User[] users { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string number { get; set; }
        public string email { get; set; }
        public int bookingCount { get; set; }
    }


    public class Bookingobject
    {
        public Booking[] bookings { get; set; }
    }

    public class Booking
    {
        public string id { get; set; }
        public string room_number { get; set; }
        public string user_id { get; set; }
        public string comeDate { get; set; }
        public string outDate { get; set; }
    }





    public class DataBase
    {
        public Bookingobject bookingobject;
        public Userobject userobject;
        public Roomobject roomobject;
        public string path;

        //функция сортировки
        public static void RoomSortById(Roomobject roomobject)
        {
            List<Room> roomlist = roomobject.rooms.ToList();//делаем из массива список и сортируем id комнаты по возрастания
            var sortedlist = from p in roomlist                                     // var локальная переменная
                             orderby p.id
                             select p;
            roomobject.rooms = sortedlist.ToArray();//делаем из списка новый массив   
        }

        public static void RoomSortByIdDescending(Roomobject roomobject)//сортировка id(номер комнаты) по убыванию ;)
        {
            List<Room> roomlist = roomobject.rooms.ToList();
            var sortedlist = from p in roomlist
                             orderby p.id descending
                             select p;
            roomobject.rooms = sortedlist.ToArray();
        }

        public static void RoomSortBysSeats(Roomobject roomobject)//сортировка мест в комнате по возрастанию
        {
            List<Room> roomlist = roomobject.rooms.ToList();
            var sortedlist = from p in roomlist
                             orderby p.seats
                             select p;
            roomobject.rooms = sortedlist.ToArray();
        }

        public static void RoomSortBysSeatsDescending(Roomobject roomobject)//сортировка мест в комнате по убыванию
        {
            List<Room> roomlist = roomobject.rooms.ToList();
            var sortedlist = from p in roomlist
                             orderby p.seats descending
                             select p;
            roomobject.rooms = sortedlist.ToArray();
        }

        public static void RoomSortByPrice(Roomobject roomobject)//сортировка цены комнаты по возрастанию
        {
            List<Room> roomlist = roomobject.rooms.ToList();
            var sortedlist = from p in roomlist
                             orderby p.price
                             select p;
            roomobject.rooms = sortedlist.ToArray();
        }
        public static void RoomSortByPriceDescending(Roomobject roomobject)//сортировка цены комнаты по убыванию
        {
            List<Room> roomlist = roomobject.rooms.ToList();
            var sortedlist = from p in roomlist
                             orderby p.price descending
                             select p;
            roomobject.rooms = sortedlist.ToArray();
        }



        public static DataBase InitBD(string path)//указываем путь к папке   
        {
            DataBase dataBase = new DataBase();//создаём класс для бдшки
            dataBase.path = path;//сохраняем путь к папке
            //далее десериализуем комнаты и пользователей и присваиваем данные к существующим в классе
            if (File.Exists(path + "Rooms.json") && File.Exists(path + "Users.json") && File.Exists(path + "Bookings.json"))
            {
                string roomData = File.ReadAllText(path + "Rooms.json");
                string userData = File.ReadAllText(path + "Users.json");
                string bookingData = File.ReadAllText(path + "Bookings.json");
                dataBase.bookingobject = JsonSerializer.Deserialize<Bookingobject>(bookingData);
                dataBase.userobject = JsonSerializer.Deserialize<Userobject>(userData);
                dataBase.roomobject = JsonSerializer.Deserialize<Roomobject>(roomData);
                return dataBase;
            }
            //warring
            return null;
        }
        public static void AddUser(DataBase dataBase, User user)//функция добавления пользователя в базу данных. принимает на вход бд и пользователя
        {
            List<User> users = dataBase.userobject.users.ToList();//делаем из массива список
            users.Add(user);// добавляем в список пользователя
            dataBase.userobject.users = users.ToArray();//делаем из списка новый массив и присваиваем его к массиву который в классе бд
        }
        public static void AddBooking(DataBase dataBase, Booking booking)//функция добавления брони в базу данных. принимает на вход бд и бронь
        {
            List<Booking> bookings = dataBase.bookingobject.bookings.ToList();//делаем из массива список
            bookings.Add(booking);// добавляем в список бронь
            dataBase.bookingobject.bookings = bookings.ToArray();//делаем из списка новый массив и присваиваем его к массиву который в классе бд
        }
        public static void RemoveUser(DataBase dataBase, string id)// функция обратная добавлению, удаляет конкретного пользователя по айди (реализация похожая на добавление)
        {
            for (int i = 0; i < dataBase.userobject.users.Length; i++)
            {
                if (dataBase.userobject.users[i].id == id)
                {
                    List<User> users = dataBase.userobject.users.ToList();
                    users.Remove(users[i]);
                    dataBase.userobject.users = users.ToArray();
                }
            }
        }
        public static void RemoveBooking(DataBase dataBase, string id)//удаляет конкретную бронь по айди (реализация похожая на добавление)
        {
            for (int i = 0; i < dataBase.bookingobject.bookings.Length; i++)
            {
                if (dataBase.bookingobject.bookings[i].id == id)
                {
                    List<Booking> bookings = dataBase.bookingobject.bookings.ToList();
                    bookings.Remove(bookings[i]);
                    dataBase.bookingobject.bookings = bookings.ToArray();
                }
            }
        }
        public static void AddRoom(DataBase dataBase, Room room)//принцип такой же как у добавления пользователя 
        {
            List<Room> rooms = dataBase.roomobject.rooms.ToList();
            rooms.Add(room);
            dataBase.roomobject.rooms = rooms.ToArray();
        }
        public static void RemoveRoom(DataBase dataBase, string id)//принцип такой же как удаление пользователя по айди
        {
            for (int i = 0; i < dataBase.roomobject.rooms.Length; i++)
            {
                if (dataBase.roomobject.rooms[i].id == id)
                {
                    List<Room> rooms = dataBase.roomobject.rooms.ToList();
                    rooms.Remove(rooms[i]);
                    dataBase.roomobject.rooms = rooms.ToArray();
                }
            }
        }
        public static string GetUserObjectString(Userobject _userobject)//функция сериализует всех пользователей и по сути сам результат функции является тем же что хранится в файле Users.json
        {
            if (_userobject == null)
            {

            }
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string userData = JsonSerializer.Serialize<Userobject>(_userobject, options);
            return userData;
        }
        public static string GetRoomObjectString(Roomobject _roomobject)//функция сериализует все типы комнат, результат функции является тем же что хранится в файле Rooms.json
        {
            if (_roomobject == null)
            {

            }
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string roomData = JsonSerializer.Serialize<Roomobject>(_roomobject, options);
            return roomData;
        }
        public static string GetBookingObjectString(Bookingobject _bookingobject)//функция сериализует все брони, результат функции является тем же что хранится в файле Bookings.json
        {
            if (_bookingobject == null)
            {

            }
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string roomData = JsonSerializer.Serialize<Bookingobject>(_bookingobject, options);
            return roomData;
        }
        public static string GetCurrentUserString(DataBase dataBase, string id)//функция сериализует конкретного пользователя, десериализовав которого, можно создать класс User в клиенте или сервере
        {
            if (dataBase == null)
            {

            }
            JsonSerializerOptions options = new JsonSerializerOptions//настройки для того чтобы русский текст не превратился в юникод по типу /u4040/
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string userData = JsonSerializer.Serialize<User>(FindUser(dataBase, id), options);
            return userData;
        }
        public static string GetCurrentRoomString(DataBase dataBase, string roomNumber)//функция сериализует инфу про конкретный тип комнат, десериализовав которую, можно создать класс Room в клиенте или сервере
        {
            if (dataBase == null)
            {

            }
            JsonSerializerOptions options = new JsonSerializerOptions//настройки для того чтобы русский текст не превратился в юникод по типу /u4040/
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            string roomData = JsonSerializer.Serialize<Room>(FindRoom(dataBase, roomNumber), options);
            return roomData;
        }
        public static string GetCurrentBookingString(DataBase dataBase, string bookingid)//функция сериализует конкретную бронь, десериализовав которую, можно создать класс Room в клиенте или сервере
        {
            if (dataBase == null)
            {

            }
            JsonSerializerOptions options = new JsonSerializerOptions//настройки для того чтобы русский текст не превратился в юникод по типу /u4040/
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            string bookingData = JsonSerializer.Serialize<Booking>(FindBooking(dataBase, bookingid), options);
            return bookingData;
        }
        public void SaveBD()//функция сохраняет базу данных - записывает в файл
        {
            var roomsPath = path + "Rooms.json";//путь к файлу
            var usersPath = path + "Users.json";
            var bookingsPath = path + "Bookings.json";
            File.WriteAllText(roomsPath, GetRoomObjectString(roomobject));//записывает в файл результат функции (смотри комментарий функции)
            File.WriteAllText(usersPath, GetUserObjectString(userobject));
            File.WriteAllText(bookingsPath, GetBookingObjectString(bookingobject));
        }
        public static User InitUser(string userData)//функция возвращает класс пользователя. на вход принимает результат функции GetCurrentUserString
        {
            return JsonSerializer.Deserialize<User>(userData);
        }
        public static Room InitRoom(string roomData)//функция возвращает класс комнаты. на вход принимает результат функции GetCurrentRoomString
        {
            return JsonSerializer.Deserialize<Room>(roomData);
        }
        public static Booking InitBooking(string bookingData)//функция возвращает класс брони. на вход принимает результат функции GetCurrentBookingString
        {
            return JsonSerializer.Deserialize<Booking>(bookingData);
        }
        public static Room FindRoom(DataBase dataBase, string roomNumber) //возвращает инфу про тип комнат по номеру комнаты
        {
            var ch = Convert.ToInt32(roomNumber);
            var del = dataBase.roomobject.rooms.Length;
            if (dataBase.roomobject.rooms.Length >= (ch % del))
            {
                for (int i = 0; i < dataBase.roomobject.rooms[ch % del].numbers.Length; i++)
                {
                    if (dataBase.roomobject.rooms[ch % del].numbers[i] == roomNumber)
                    {
                        return dataBase.roomobject.rooms[ch % del];
                    }
                }
            }
            for (int i = 0; i < dataBase.roomobject.rooms.Length; i++)
            {
                for (int j = 0; j < dataBase.roomobject.rooms[i].numbers.Length; j++)
                {
                    if (dataBase.roomobject.rooms[i].numbers[j] == roomNumber)
                    {
                        return dataBase.roomobject.rooms[i];
                    }
                }
            }
            return null;
        }
        public static User FindUser(DataBase dataBase, string userId) //возвращает юзера по id
        {
            for (int i = 0; i < dataBase.userobject.users.Length; i++)
            {
                if (dataBase.userobject.users[i].id == userId)
                {
                    return dataBase.userobject.users[i];
                }
            }
            return null;
        }
        public static Booking FindBooking(DataBase dataBase, string bookingId) //возвращает бронь по id
        {
            for (int i = 0; i < dataBase.bookingobject.bookings.Length; i++)
            {
                if (dataBase.bookingobject.bookings[i].id == bookingId)
                {
                    return dataBase.bookingobject.bookings[i];
                }
            }
            return null;
        }
        public static string GetUserString(User _user)//функция сериализует пользователя, десериализовав которого, можно создать класс User в клиенте или сервере
        {
            if (_user == null)
            {

            }
            JsonSerializerOptions options = new JsonSerializerOptions//настройки для того чтобы русский текст не превратился в юникод по типу /u4040/
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string userData = JsonSerializer.Serialize<User>(_user, options);
            return userData;
        }
        public static string GetRoomString(Room _room)//функция сериализует комнату, десериализовав которую, можно создать класс Room в клиенте или сервере
        {
            if (_room == null)
            {

            }
            JsonSerializerOptions options = new JsonSerializerOptions//настройки для того чтобы русский текст не превратился в юникод по типу /u4040/
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string roomData = JsonSerializer.Serialize<Room>(_room, options);
            return roomData;
        }
        public static string GetBookingString(Booking _booking)//функция сериализует бронь, десериализовав которую, можно создать класс Booking в клиенте или сервере
        {
            if (_booking == null)
            {

            }
            JsonSerializerOptions options = new JsonSerializerOptions//настройки для того чтобы русский текст не превратился в юникод по типу /u4040/
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string bookingData = JsonSerializer.Serialize<Booking>(_booking, options);
            return bookingData;
        }
    }
}
