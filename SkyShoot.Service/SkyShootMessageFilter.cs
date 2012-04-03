using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Diagnostics;

namespace SkyShoot.ServProgram
{
	class SkyShootMessageFilter:MessageFilter
  {
	  public static Dictionary<Guid,Guid> table= new Dictionary<Guid,Guid>();
	  private Guid guid;
	  public  bool IsLogin;
    public SkyShootMessageFilter(Guid serviceId,bool isLogin)
    {
		IsLogin = isLogin;
		guid = serviceId;
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
			return IsLogin;
		}
		Guid serviceGuid;
		bool isInTable = table.TryGetValue(header, out serviceGuid);
		//if (!isInTable && IsLogin) return true;
		//if (!isInTable && !IsLogin) { Trace.WriteLine("MessageFilter table mismatch"); }
		if (!isInTable)
		{
			return IsLogin;
		}
		return serviceGuid == guid;
    }

	public static void DeleteFromTable(Guid client){
		table.Remove(client);
	}
  }
}
