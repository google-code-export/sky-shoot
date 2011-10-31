using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using SkyShoot.Contracts.Service;

namespace SkyShoot.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MainSkyShootService" in code, svc and config file together.
    public class MainSkyShootService : ISkyShootService
    {

        public bool Register(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Contracts.Session.GameDescription[] GetGameList()
        {
            throw new NotImplementedException();
        }

        public Contracts.Session.GameDescription CreateGame(Contracts.Session.GameMode mode, int maxPlayers)
        {
            throw new NotImplementedException();
        }

        public bool JoinGame(Contracts.Session.GameDescription game)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
