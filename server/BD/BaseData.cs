using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace DataBase.BD
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
        public int status { get; set; }
        public int seats { get; set; }
        public int price { get; set; }
        public int id { get; set; }
    }







    public class Userobject
    {
        public User[] users { get; set; }
    }

    public class User
    {
        public string id { get; set; }//номер брони
        public int room_id { get; set; }//номер комнаты
        public string name { get; set; }
        public string email { get; set; }
        public string number { get; set; }
        public string comeDate { get; set; }
        public string outDate { get; set; }
    }




    public class DataBase
    {
        public Userobject userobject;
        public Roomobject roomobject;
        public string path;

        //функция сортировки
        public static void SortById(DataBase dataBase)
        {
            List<Room> roomlist = dataBase.roomobject.rooms.ToList();//делаем из массива список и сортируем id комнаты по возрастания
            var sortedlist = from p in roomlist                                     // var локальная переменная
                             orderby p.id  
                             select p;
            dataBase.roomobject.rooms = sortedlist.ToArray();//делаем из списка новый массив   
        }

        public static void SortByIdDescending(DataBase dataBase)//сортировка id(номер комнаты) по убыванию ;)
        {
            List<Room> roomlist = dataBase.roomobject.rooms.ToList();
            var sortedlist = from p in roomlist
                             orderby p.id descending
                             select p;
            dataBase.roomobject.rooms = sortedlist.ToArray();
        }

        public static void SortBysSeats(DataBase dataBase)//сортировка мест в комнате по возрастанию
        {
            List<Room> roomlist = dataBase.roomobject.rooms.ToList();
            var sortedlist = from p in roomlist
                             orderby p.seats
                             select p;
            dataBase.roomobject.rooms = sortedlist.ToArray();
        }

        public static void SortBysSeatsDescending(DataBase dataBase)//сортировка мест в комнате по убыванию
        {
            List<Room> roomlist = dataBase.roomobject.rooms.ToList();
            var sortedlist = from p in roomlist
                             orderby p.seats descending
                             select p;
            dataBase.roomobject.rooms = sortedlist.ToArray();
        }

        public static void SortByPrice(DataBase dataBase)//сортировка цены комнаты по возрастанию
        {
            List<Room> roomlist = dataBase.roomobject.rooms.ToList();
            var sortedlist = from p in roomlist
                             orderby p.price
                             select p;
            dataBase.roomobject.rooms = sortedlist.ToArray();
        }
         public static void SortByPriceDescending(DataBase dataBase)//сортировка цены комнаты по убыванию
        {
            List<Room> roomlist = dataBase.roomobject.rooms.ToList();
            var sortedlist = from p in roomlist
                             orderby p.price descending
                             select p;
            dataBase.roomobject.rooms = sortedlist.ToArray();
        }


        public static DataBase InitBD(string path)//указываем путь к папке   
        {
            DataBase dataBase = new DataBase();//создаём класс для бдшки
            dataBase.path = path;//сохраняем путь к папке
            //далее десериализуем комнаты и пользователей и присваиваем данные к существующим в классе
            if (File.Exists(path + "Rooms.json") && File.Exists(path + "Users.json"))
            {
                string roomData = File.ReadAllText(path + "Rooms.json");
                string userData = File.ReadAllText(path + "Users.json");
                dataBase.userobject = JsonSerializer.Deserialize<Userobject>(userData);
                dataBase.roomobject = JsonSerializer.Deserialize<Roomobject>(roomData);
                return dataBase;
            }
            return null;
        }

        public static DataBase InitBDClient(string roomobject, string userobject)//(не определились в использовании)    принимает аргументы данных о комнатах и пользователях в формате json
        {
            DataBase dataBase = new DataBase();//создаём класс для бдшки
            dataBase.roomobject = JsonSerializer.Deserialize<Roomobject>(roomobject);//десериализуем
            dataBase.userobject = JsonSerializer.Deserialize<Userobject>(userobject);
            return dataBase;//возвращаем
        }

        public static void AddUser(DataBase dataBase, User user)//функция добавления пользователя в базу данных. принимает на вход бд и пользователя
        {
            List<User> users = dataBase.userobject.users.ToList();//делаем из массива список
            users.Add(user);// добавляем в список пользователя
            dataBase.userobject.users = users.ToArray();//делаем из списка новый массив и присваиваем его к массиву который в классе бд
        }
        public static void RemoveUser(DataBase dataBase, string id)// функция обратная добавлению, удаляет конкретного пользователя по номеру его брони (реализация похожая на добавление)
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
        public static void AddRoomobject(DataBase dataBase, Room room)//принцип такой же как у добавления пользователя 
        {
            List<Room> rooms = dataBase.roomobject.rooms.ToList();
            rooms.Add(room);
            dataBase.roomobject.rooms = rooms.ToArray();
        }
        public static void RemoveRoomObject(DataBase dataBase, int id)//принцип такой же как удаление пользователя по айди
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
        public static string GetRoomObjectString(Roomobject _roomobject)//функция сериализует все комнаты, результат функции является тем же что хранится в файле Rooms.json
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
        public static string GetCurrentUserString(User _user)//функция сериализует конкретного пользователя, десериализовав которого, можно создать класс User в клиенте или сервере
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
        public static string GetCurrentRoomString(Room _room)//функция сериализует конкретного комнату, десериализовав которую, можно создать класс Room в клиенте или сервере
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
        public static void SaveBD(DataBase dataBase)//функция сохраняет базу данных - записывает в файл
        {
            var roomsPath = dataBase.path + "Rooms.json";//путь к файлу
            var usersPath = dataBase.path + "Users.json";
            File.WriteAllText(roomsPath, GetRoomObjectString(dataBase.roomobject));//записывает в файл результат функции (смотри комментарий функции)
            File.WriteAllText(usersPath, GetUserObjectString(dataBase.userobject));
        }
        public static User InitUser(string userData)//функция возвращает класс пользователя. на вход принимает результат функции GetCurrentUserString
        {
            return JsonSerializer.Deserialize<User>(userData);
        }
        public static Room InitRoom(string roomData)//функция возвращает класс комнаты. на вход принимает результат функции GetCurrentRoomString
        {
            return JsonSerializer.Deserialize<Room>(roomData);
        }
    }
}
