using System.Configuration;

namespace SkyShoot.Service.Account
{
    public class AccountManager
    {
        // для теста
        // Account name: devstoreaccount1
        // Account key: Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==
        // прописывается в Web.config

        static string storageConnectionString = ConfigurationManager.ConnectionStrings["Storage"].ConnectionString;
        TableHelper TableHelper = new TableHelper(storageConnectionString);

        /**
         в начале ещё нужно будет создать таблицу:
         TableHelper.CreateTable("AccountsTable")

        (!) нужно будет делать регистрацию приводя все знаки username / password к строчным
        ещё проверять нужно будет пароли и имена, чтобы они содержали только a-z,A-Z,1-9 ,-,_ и никаких пробелов
          
        Пароль в эти функции уже должен приходить как md5 хеш, это делается при вызове функций на клиенте...только где это писать
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
                    string salt = TableHelper.GetRandomString();
                    string password_hash = HashHelper.GetMd5Hash(salt + password + salt);

                    if (TableHelper.InsertEntity("AccountsTable",
                    new Account("account", username) { Login = username, HashPassword = password_hash, Salt = salt,
                                                       Email = "--", Info = "--" })) // пока Email и Info будут пустовать 
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
                    if (HashHelper.verifyMd5Hash( account.Salt + password + account.Salt, account.HashPassword))
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
                string salt = TableHelper.GetRandomString();
                string password_hash = HashHelper.GetMd5Hash(salt + new_password + salt);

                if (TableHelper.ReplaceUpdateEntity("AccountTable", "account", username,
                    new Account("account", username) { HashPassword = password_hash, Salt = salt }))
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