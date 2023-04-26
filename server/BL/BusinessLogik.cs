using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.BD;

namespace server.BL
{
    internal class BusinessLogik
    {
        public static string FindUser(string Login, string Password, DataBase.BD.DataBase dataBase)
        {
            string user="0";

            for (int i = 0; i < dataBase.userobject.users.Length; i++)
            {
                if ((dataBase.userobject.users[i].password == Password) && (dataBase.userobject.users[i].login == Login)){
                    user = DataBase.BD.DataBase.GetCurrentUserString(dataBase.userobject.users[i]);
                    break;
                }
                else if ((dataBase.userobject.users[i].password != Password) || (dataBase.userobject.users[i].login != Login))
                {
                    user = "ошибка";
                }
            }
            return (user);
        }
    }
}
