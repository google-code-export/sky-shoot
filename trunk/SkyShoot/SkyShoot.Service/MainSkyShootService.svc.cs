using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using SkyShoot.Contracts.Service;
using SkyShoot.Service.Client;
using Microsoft.Xna.Framework;
using SkyShoot.Service.Session;


namespace SkyShoot.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.PerSession)]
    public class MainSkyShootService : ISkyShootService
    {
        private Account.AccountManager _accountManager = new Account.AccountManager();
        private Session.SessionManager _sessionManager = new Session.SessionManager();

        private Client.Client _client;

        private GameSession _session;

        private static List<Client.Client> _clientsList = new List<Client.Client>();

        public bool Register(string username, string password)
        {
            bool result = _accountManager.Register(username, password);
            return result;
        }

        public bool Login(string username, string password)
        {
            bool result = _accountManager.Login(username, password);

            if (result)
            {
                _client = new Client.Client(username, 
                    OperationContext.Current.GetCallbackChannel<ISkyShootCallback>(), true);
                _clientsList.Add(_client);
            }

            return result;
        }

        public Contracts.Session.GameDescription[] GetGameList()
        {
            return _sessionManager.GetGameList();
        }

        public bool CreateGame(Contracts.Session.GameMode mode, int maxPlayers)
        {
            
            return ( _sessionManager.CreateGame(mode, maxPlayers, _client, Contracts.Session.TileSet.Grass) != null );
        }

        public bool JoinGame(Contracts.Session.GameDescription game)
        {
            _session = _sessionManager.JoinGame(game, _client.Name);
            if (_session == null)
                return false;
            return true;
        }

        public void Move(Vector2 direction)
        {
            _session.Move(_client, direction);
        }

        public void Shoot(Vector2 direction)
        {
            throw new NotImplementedException();
        }

        public void TakeBonus(Contracts.Bonuses.AObtainableDamageModifier bonus)
        {
            bonus.Owner.State |= bonus.Type;
        }

        public void TakePerk(Contracts.Perks.Perk perk)
        {
            throw new NotImplementedException();
        }

        public void LeaveGame()
        {
            bool result = _sessionManager.LeaveGame(_client.Name);
            if (!result)
            { /* что-то сделать, например, добавить сообщение в лог */ }
        }
    }
}
