namespace server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //BD init
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetFullPath("Rooms1.json"))))) + "\\BD\\";
            DataBase.BD.DataBase dataBase = DataBase.BD.DataBase.initBD(path);
            
        }
    }
}