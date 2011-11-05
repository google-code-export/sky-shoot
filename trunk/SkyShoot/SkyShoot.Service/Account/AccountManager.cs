using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.Services.Client;


namespace SkyShoot.Service.Account
{
    public class AccountManager
    {
        // для теста
        // Account name: devstoreaccount1
        // Account key: Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==
        // прописывается в Web.config

        string storageConnectionString = ConfigurationManager.ConnectionStrings["Storage"].ConnectionString;
        TableHelper TableHelper = new TableHelper(storageConnectionString);

        /**
         в начале ещё нужно будет создать таблицу:
         TableHelper.CreateTable("AccountsTable")

        (!) нужно будет делать регистрацию приводя все знаки username / password к строчным
        ещё проверять нужно будет пароли и имена, чтобы они содержали только a-z,A-Z,1-9 ,-,_ и никаких пробелов
        **/

        public bool Register(string username, string password)
        {
            Account account = null;
            if (TableHelper.GetEntity<Account>("AccountsTable", "account", username, out account))
            {
                if (account != null)
                {
                    return false; // такой аккаунт уже есть > выберете другое имя для аккаунта
                }
                else
                {
                    if (TableHelper.InsertEntity("AccountsTable",
                    new Account("account", username) { Login = username, Password = password, Email = "--", Info = "--" })) // пока Email и Info будут пустовать 
                    {
                        return true; // регистрация прошла успешно!
                    }
                    else
                    {
                        return false; // какие-то ошибки вылезли
                    }

                }
            }
            else
            {
                return false; // какие-то ошибки вылезли
            }
        }

        public bool Login(string username, string password)
        {
            Account account = null;
            if (TableHelper.GetEntity<Account>("AccountsTable", "account", username, out account))
            {
                if (account != null)
                {
                    if (account.Password == password)
                    {
                        return true; // залогинились
                    }
                    else
                    {
                        return false; // неверный пароль от аккаунта
                    }
                }
                else
                {
                    return false; // такого аккаунта вообще нет!
                }
            }
            else
            {
                return false; // какие-то ошибки вылезли
            }
        }

        public bool CreatePassword(string username, string password, string new_password)
            // даже если игрок залогинен, то ему для подтверждения нужно вводить заново свои данные (будем это считать зачатками безопасности)
        {
            if (Login(username, password))
            {
                if (TableHelper.ReplaceUpdateEntity("AccountTable", "account", username,
                    new Account("account", username) { Password = new_password }))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false; // нельзя менять пароль если не знаешь прошлый
            }
        }

        public bool DeleteAccount(string username, string password)
        {
            if (Login(username, password)) // надо убедиться что происходит осмысленное удаление аккаунта
            {
                if (TableHelper.DeleteEntity<Account>("AccountsTable", "account", username))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false; // проверку не прошёл > удалить аккаунт нельзя
            }
        }

    }
}