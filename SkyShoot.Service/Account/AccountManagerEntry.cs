using System;
using Microsoft.WindowsAzure.StorageClient;

namespace SkyShoot.Service.Account
{
    public class AccountManagerEntry
        : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {
        public AccountManagerEntry()
        {
            PartitionKey = DateTime.UtcNow.ToString("MMddyyyy");
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
        }

        public string Account { get; set; }        // имя аккаунта
        public string HashPassword { get; set; }   // в таблице хранится хеш пароля с солью
        public string Salt { get; set; }           // строка случайных данных для шифрования паролей
        public string Email { get; set; }          // у приличных людей регистрация через эл.почту :)
        public string Info { get; set; }           // здесь в дальнейшем будет храниться доп.информация о аккаунте
                                                   // (очки бонусов, номер кредитки :) и т.д)
    }
}
