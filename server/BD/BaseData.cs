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
        public int roomPrice { get; set; }
        public int room_id { get; set; }
    }


    public class Userobject
    {
        public User[] users { get; set; }
    }

    public class User
    {
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
        
        public static void initBD(DataBase dataBase)
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath("Rooms1.json"))))) + "\\BD\\";
            if (File.Exists(path + "Rooms.json") && File.Exists(path + "Users.json"))
            {
                string roomData = File.ReadAllText(path + "Rooms.json");
                string userData = File.ReadAllText(path + "Users.json");
                dataBase.userobject = JsonSerializer.Deserialize<Userobject>(userData);
                dataBase.roomobject = JsonSerializer.Deserialize<Roomobject>(roomData);
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
            string userData = JsonSerializer.Serialize<Userobject>(_userobject,options);
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
            string roomData = JsonSerializer.Serialize<Roomobject>(_roomobject,options);
            return roomData;
        }
    }
}
