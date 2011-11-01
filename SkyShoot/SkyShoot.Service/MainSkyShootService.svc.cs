using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using SkyShoot.Contracts.Service;
using SkyShoot.Service.Client;

namespace SkyShoot.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MainSkyShootService" in code, svc and config file together.
    public class MainSkyShootService : ISkyShootService
    {

        private Account.AccountManager _accountManager = new Account.AccountManager();
        private Session.SessionManager _sessionManager = new Session.SessionManager();

        private List<Client.Client> _clientsList = new List<Client.Client>();

        public bool Register(string username, string password)
        {
            bool result = _accountManager.Register(username, password);

//            bool loginResult = Login(username, password);

            return result;
        }

        public bool Login(string username, string password)
        {
            bool result = _accountManager.Login(username, password);

            if (result)
            {
                Client.Client client = new Client.Client(username);
                _clientsList.Add(client);
            }

            return result;
        }

        public Contracts.Session.GameDescription[] GetGameList()
        {
            return _sessionManager.GetGameList();
        }

        public Contracts.Session.GameDescription CreateGame(Contracts.Session.GameMode mode, int maxPlayers)
        {
            //Позже заменить 4ый параметр на какую-нибудь переменную.
            return _sessionManager.CreateGame(mode, maxPlayers, "user", Contracts.Session.TileSet.Grass); // потом вместо "user" будет имя из Client'а
        }

        public bool JoinGame(Contracts.Session.GameDescription game)
        {
            return _sessionManager.JoinGame(game, "user"); // потом вместо "user" будет имя из Client'а
        }

        public void Move(System.Drawing.PointF direction)
        {
            throw new NotImplementedException();
        }

        public void Shoot(System.Drawing.PointF direction)
        {
            throw new NotImplementedException();
        }

        public void TakeBonus(Contracts.Bonuses.AObtainableDamageModifier bonus)
        {
            throw new NotImplementedException();
        }

        public void TakePerk(Contracts.Perks.Perk perk)
        {
            throw new NotImplementedException();
        }

        public void LeaveGame()
        {
            bool result = _sessionManager.LeaveGame("user"); // потом вместо "user" будет имя из Client'а
            if (!result)
            { /* что-то сделать, например, добавить сообщение в лог */ }
        }
    }
}
