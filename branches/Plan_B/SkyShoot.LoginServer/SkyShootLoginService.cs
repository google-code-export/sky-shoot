using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using SkyShoot.Contracts;
using SkyShoot.Contracts.GameEvents;
using SkyShoot.Contracts.Mobs;
using SkyShoot.Contracts.Service;
using SkyShoot.Contracts.Session;

namespace SkyShoot.LoginServer
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
			InstanceContextMode = InstanceContextMode.Single)]
	public class SkyShootLoginService : ISkyShootLogin
	{
		public static int GlobalID;
		public int LocalID;
		public string Name;

	    private int _serverIndex = 0;

	    private Dictionary<string, ISkyShootService> _servers = new Dictionary<string, ISkyShootService>();

	    private Dictionary<Guid, ISkyShootService> _gameSessions = new Dictionary<Guid, ISkyShootService>();

	    private AutoResetEvent _disposeEvent = new AutoResetEvent(false);

	    private Thread _askRolesThread;

        private MessageFilterTable<IEnumerable<ServiceEndpoint>> _routingTable;

        public SkyShootLoginService(MessageFilterTable<IEnumerable<ServiceEndpoint>> table)
		{
			LocalID = GlobalID;
			GlobalID++;
            _askRolesThread = new Thread(AskRolesThreadFunc);
            _askRolesThread.Start();
            _routingTable = table;
		}

		//public void Disconnect() { LeaveGame(); }

        private void AskRolesThreadFunc(object state)
        {
            while(!_disposeEvent.WaitOne(30000))
            {
                try
                {
                    var servers = RoleEnvironment.Roles.First(x => x.Value.Name == "SkyShoot.GameServer").Value.Instances;

                    lock (_servers)
                        foreach (var roleInstance in servers)
                        {
                            var endpoint = roleInstance.InstanceEndpoints["ServiceEndpoint"];
                            string address = string.Format("net.tcp://{0}", endpoint.IPEndpoint);
                            if (!_servers.ContainsKey(address))
                            {

                                var proxy =
                                    ChannelFactory<ISkyShootService>.CreateChannel(new NetTcpBinding(SecurityMode.None)
                                                                                       {
                                                                                           CloseTimeout =
                                                                                               new TimeSpan(1, 0, 0, 0),
                                                                                           OpenTimeout =
                                                                                               new TimeSpan(1, 0, 0, 0),
                                                                                           ReceiveTimeout =
                                                                                               new TimeSpan(1, 0, 0, 0),
                                                                                           SendTimeout =
                                                                                               new TimeSpan(1, 0, 0, 0)
                                                                                       },
                                                                                   new EndpointAddress(address));
                                _routingTable.Add(new SkyShootMessageFilter(address, false), new List<ServiceEndpoint>()
                                                                                                 {
                                                                                                     new ServiceEndpoint
                                                                                                         (
                                                                                                         ContractDescription
                                                                                                             .
                                                                                                             GetContract
                                                                                                             (
                                                                                                                 typeof
                                                                                                                     (
                                                                                                                     ISkyShootService
                                                                                                                     )),
                                                                                                         new NetTcpBinding
                                                                                                             (SecurityMode
                                                                                                                  .None)
                                                                                                             {
                                                                                                                 CloseTimeout
                                                                                                                     =
                                                                                                                     new TimeSpan
                                                                                                                     (1,
                                                                                                                      0,
                                                                                                                      0,
                                                                                                                      0),
                                                                                                                 OpenTimeout
                                                                                                                     =
                                                                                                                     new TimeSpan
                                                                                                                     (1,
                                                                                                                      0,
                                                                                                                      0,
                                                                                                                      0),
                                                                                                                 ReceiveTimeout
                                                                                                                     =
                                                                                                                     new TimeSpan
                                                                                                                     (1,
                                                                                                                      0,
                                                                                                                      0,
                                                                                                                      0),
                                                                                                                 SendTimeout
                                                                                                                     =
                                                                                                                     new TimeSpan
                                                                                                                     (1,
                                                                                                                      0,
                                                                                                                      0,
                                                                                                                      0)
                                                                                                             },
                                                                                                         new EndpointAddress
                                                                                                             (address))
                                                                                                 });
                                _servers.Add(address, proxy);
                            }

                        }
                }
                catch (Exception)
                {
                    
                }
                
            }
        }

		public bool Register(string username, string password)
		{
			/*bool result = _accountManager.Register(username, password);
			if(result)
			{
				Trace.WriteLine(username + "has registered");
			}
			else
			{
				Trace.WriteLine(username + "is not registered. The name of the employing or other errors");
			}
			return result; */
			return true;
		}

		public Guid? Login(string username, string password)
		{
			bool result = true;//_accountManager.Login(username, password);

			if (result)
			{
				Name = username;
				//_callback = OperationContext.Current.GetCallbackChannel<ISkyShootCallback>();
			}
			else
			{
				return null;
			}

			return Guid.NewGuid();
		}

		public GameDescription[] GetGameList()
		{
            lock(_servers)
            {
                var games = _servers.Values.SelectMany(x => x.GetGameList()).ToList();
                var ids = games.Select(x => x.GameId);
                _gameSessions = _gameSessions.Where(x => ids.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
                return games.ToArray();
            }
		}

	    public void LeaveGame()
	    {
            try
            {
                lock (_servers)
                {
                    
                    var id = OperationContext.Current.IncomingMessageHeaders.GetHeader<Guid>("ID", "namespace");

                    var filter =
                            _routingTable.FirstOrDefault(x => ((SkyShootMessageFilter)x.Key).Players.ContainsKey(id)).Key as
                            SkyShootMessageFilter;
                    if (filter != null)
                    {
                        var targetProxy = _servers[filter.Address];
                        using (OperationContextScope scope = new OperationContextScope((IContextChannel) targetProxy))
                        {
                            var header = new MessageHeader<Guid>(id);
                            var untyped = header.GetUntypedHeader("ID", "namespace");
                            if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("ID", "namespace") == -1)
                            {
                                OperationContext.Current.OutgoingMessageHeaders.Add(untyped);
                            }
                            targetProxy.LeaveGame();

                            //todo:  add filter here



                            filter.Players.Remove(id);

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.Fail(Name + " has not joined the game. " + e.Message);
            }
	    }

	    public GameDescription CreateGame(GameMode mode, int maxPlayers, TileSet tileSet)
        {
            try
            {
                // zee ingenious algorithm!
                lock (_servers)
                {
                    
                    if (_servers.Count <= _serverIndex) _serverIndex = 0;

                    var target = _servers.Keys.ToList()[_serverIndex++];

                    var targetProxy = _servers[target];

                    var id = OperationContext.Current.IncomingMessageHeaders.GetHeader<Guid>("ID", "namespace");
                    var name = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("Name", "namespace");

                    using (OperationContextScope scope = new OperationContextScope((IContextChannel)targetProxy))
                    {
                        var header = new MessageHeader<Guid>(id);
                        var untyped = header.GetUntypedHeader("ID", "namespace");
                        if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("ID", "namespace") == -1)
                        {
                            OperationContext.Current.OutgoingMessageHeaders.Add(untyped);
                        }
                        var header2 = new MessageHeader<string>(name);
                        var untyped2 = header2.GetUntypedHeader("Name", "namespace");
                        if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("Name", "namespace") == -1)
                        {
                            OperationContext.Current.OutgoingMessageHeaders.Add(untyped2);
                        }
                        var filter =
                            _routingTable.First(x => ((SkyShootMessageFilter) x.Key).Address == target).Key as
                            SkyShootMessageFilter;

                        filter.Players.Add(id, id);

                        var gameDescription = targetProxy.CreateGame(mode, maxPlayers, tileSet);

                        _gameSessions.Add(gameDescription.GameId, targetProxy);

                        return gameDescription;
                    }


                }
            }
            catch (Exception e)
            {
                Trace.Fail(Name + " unable to create game. " + e.Message);
                return null;
            }
        }

	    public bool JoinGame(GameDescription game)
		{
			try
			{
                lock(_servers)
                {
                    var targetProxy = _gameSessions[game.GameId];
                    var id = OperationContext.Current.IncomingMessageHeaders.GetHeader<Guid>("ID", "namespace");
                    var name = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("Name", "namespace");
                    using (OperationContextScope scope = new OperationContextScope((IContextChannel)targetProxy))
                    {
                        var header = new MessageHeader<Guid>(id);
                        var untyped = header.GetUntypedHeader("ID", "namespace");
                        if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("ID", "namespace") == -1)
                        {
                            OperationContext.Current.OutgoingMessageHeaders.Add(untyped);
                        }
                        var header2 = new MessageHeader<string>(name);
                        var untyped2 = header2.GetUntypedHeader("Name", "namespace");
                        if (OperationContext.Current.OutgoingMessageHeaders.FindHeader("Name", "namespace") == -1)
                        {
                            OperationContext.Current.OutgoingMessageHeaders.Add(untyped2);
                        }
                        bool result = targetProxy.JoinGame(game);

                        //todo:  add filter here
                        var address = _servers.First(x => x.Value == _gameSessions[game.GameId]).Key;

                        var filter =
                            _routingTable.First(x => ((SkyShootMessageFilter) x.Key).Address == address).Key as
                            SkyShootMessageFilter;

                        filter.Players.Add(id, id);

                        return result;
                    }
                }
			}
			catch (Exception e)
			{
				Trace.Fail(Name + " has not joined the game. " + e.Message);
				return false;
			}
		}

		public GameLevel GameStart(Guid gameId)
		{
			// Trace.WriteLine("GameStarted");
            lock(_servers)
            {
                return _gameSessions[gameId].GameStart(gameId);
            }
		}

        ~SkyShootLoginService()
        {
            _disposeEvent.Set();
        }
	}
}
