using System;
using System.Collections.Generic;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;

namespace SkyShoot.LoginServer
{
    internal class SkyShootMessageFilter : MessageFilter
    {
        public Dictionary<Guid, Guid> Players { get; private set; }
        public string Address { get; private set; }
        public bool IsLogin { get; private set; }

        public SkyShootMessageFilter(string address, bool isLogin)
        {
            IsLogin = isLogin;
            Address = address;
            Players = new Dictionary<Guid, Guid>();
        }

        public override bool Match(MessageBuffer buffer)
        {
            return Match(buffer.CreateMessage());
        }

        public override bool Match(Message message)
        {
            Guid header;
            try
            {
                header = message.Headers.GetHeader<Guid>("ID", "namespace");
            }
            catch (Exception)
            {
                // login stuff
                return IsLogin;
            }
            Guid serviceGuid;
            //if (!isInTable && IsLogin) return true;
            //if (!isInTable && !IsLogin) { Trace.WriteLine("MessageFilter table mismatch"); }
            bool isInTable = Players.ContainsKey(header);
            var action = message.Headers.Action;
            if (action.EndsWith("CreateGame") || action.EndsWith("LeaveGame") || action.EndsWith("JoinGame")
                || action.EndsWith("GameStart")) return IsLogin; // they are login stuff, but with session information
            return isInTable;
        }
    }
}
