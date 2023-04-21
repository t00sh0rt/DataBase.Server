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
        public int villagerId { get; set; }
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
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public int admin { get; set; }
        public string comeDate { get; set; }
        public string outDate { get; set; }
    }


    public class DataBase
    {
        public Userobject userobject;
        public Roomobject roomobject;

        public static DataBase InitBD(string path)//указываем путь к папке
        {
            DataBase dataBase = new DataBase();//создаём класс для бдшки
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

        public static void AddUser(DataBase dataBase, User user)
        {
            List<User> users = dataBase.userobject.users.ToList();//делаем из массива список
            users.Add(user);// добавляем в список пользователя
            dataBase.userobject.users = users.ToArray();//делаем из списка новый массив и присваиваем его к массиву который в классе бд
        }

        public static void RemoveUser(DataBase dataBase, int id)// функция обратная добавлению, удаляет конкретного пользователя по айди (реализация похожая на добавление)
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

        public static string GetUserObjectString(Userobject _userobject)
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
        public static string GetRoomObjectString(Roomobject _roomobject)
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
    }
}
