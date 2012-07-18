//------------------------------------------------------------------------------
// <auto-generated>
//	 This code was generated from a template.
//
//	 Changes to this file may cause incorrect behavior and will be lost if
//	 the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// Architectural overview and usage guide: 
// http://blogofrab.blogspot.com/2010/08/maintenance-free-mocking-for-unit.html
//------------------------------------------------------------------------------
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Common;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Services.Data.EntitiesMockObjectSet;

namespace Services.Data
{
    /// <summary>
    /// The concrete mock context object that implements the context's interface.
    /// Provide an instance of this mock context class to client logic when testing, 
    /// instead of providing a functional context object.
    /// </summary>
    [GeneratedCode("Entity","0.9"), DebuggerNonUserCode]
    public partial class EntitiesMock : IEntities
    {
    	public IObjectSet<Administrator> Administrators
    	{
    		get { return _administrators  ?? (_administrators = new MockObjectSet<Administrator>()); }
    	}
    	private IObjectSet<Administrator> _administrators;
    	public IObjectSet<User> Users
    	{
    		get { return _users  ?? (_users = new MockObjectSet<User>()); }
    	}
    	private IObjectSet<User> _users;
    
    	public void Dispose()
    	{
    		
    	}
    
    	public int SaveChanges()
    	{
    		return 0;
    	}
    
    	public void UseTracking(bool enable)
    	{
    		return;
    	}
    }
}