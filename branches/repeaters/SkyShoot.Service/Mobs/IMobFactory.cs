using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyShoot.Contracts.Mobs
{
	public interface IMobFactory
	{
		Mob CreateMob();
	}
}
