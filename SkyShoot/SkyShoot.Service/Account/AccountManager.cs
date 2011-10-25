using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyShoot.Service.Account
{
    public class AccountManager
    {
        private string line;

        private bool LoginBefore(string Login)
        {
            System.IO.StreamReader file =
                new System.IO.StreamReader(@"пока не понтяно"); // будем переделывать

            while ((line = file.ReadLine()) != null)
            {
                if (line.Split(' ')[0] == Login)
                    return true;
                break;
            }

            file.Close();
            return false;

        }

        public bool Register(string username, string password)
        {
            if (LoginBefore(username))
                return false; // такой username уже занят
            else
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"куда-то", true)) // всё равно будем переделывать
                {
                    file.WriteLine(username + " " + password);
                }
            return true; // всё ок

        }

        public bool Login(string username, string password)
        {
            if (LoginBefore(username))
                if (password == line.Split(' ')[1])
                    return true;  // всё ок
                else
                    return false; // пароль неверный
            else
                return false; // такого логина нет

        }
    }
}