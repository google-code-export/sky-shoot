using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SkyShoot.Contracts.Mobs;
using Microsoft.Xna.Framework;

namespace SkyShoot.Contracts.Session
{
    [DataContract]
    public class GameLevel
    {
		public const int MOVEMENTTIME = 100; //in milliseconds
		public float levelHeight = 1000; 
		public float levelWidth = 1000;
		public const float LEVELBORDER = 50;
		
        public GameLevel(TileSet usedTileSet)
        {
            UsedTileSet = usedTileSet;
        }

		public Vector2 ComputeMovement(AMob mob)
		{
			var realHeight=levelHeight-mob.Radius;
			var realWidth=levelWidth-mob.Radius;
			var newCoord = new Vector2(mob.Coordinates.X + mob.Speed * MOVEMENTTIME * mob.RunVector.X, mob.Coordinates.Y + mob.Speed * MOVEMENTTIME * mob.RunVector.Y);
			if(Math.Abs(mob.Coordinates.X) <= realWidth)
				newCoord.X=Math.Min(Math.Abs(newCoord.X), realWidth) * Math.Sign(newCoord.X);
			else
				newCoord.X=Math.Min(Math.Abs(newCoord.X), realWidth+LEVELBORDER) * Math.Sign(newCoord.X);
				
			if (Math.Abs(mob.Coordinates.Y) <= realHeight)
				newCoord.Y = Math.Min(Math.Abs(newCoord.Y), realHeight) * Math.Sign(newCoord.Y);
			else
				newCoord.Y = Math.Min(Math.Abs(newCoord.Y), realHeight + LEVELBORDER) * Math.Sign(newCoord.Y);

			return newCoord;
		}

        [DataMember]
        public TileSet UsedTileSet
        {
            get;
            private set;
        }

    }
}
