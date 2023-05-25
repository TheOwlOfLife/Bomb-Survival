﻿global using Sandbox;
global using Sandbox.Sdf;
global using Sandbox.UI.Construct;
global using System;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
using System.Numerics;

namespace BombSurvival;

public partial class BombSurvival : GameManager
{
	public static BombSurvival Instance { get; private set; }

	public BombSurvival()
	{
		Instance = this;
	}

	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		var pawn = new Player();
		client.Pawn = pawn;
		pawn.Position = PointToTop( Vector3.Zero );

	}

	[Event("TerrainLoaded")]
	public static void PlacePlayers()
	{
		foreach( var player in Entity.All.OfType<Player>() )
		{
			player.Position = PointToTop( Vector3.Zero );
		}
	}

	[GameEvent.Physics.PreStep]
	public static void PreStep() // Lock the Y axis
	{
		if ( Game.IsClient ) return;

		foreach( var entity in Entity.All.OfType<ModelEntity>() )
		{
			if ( entity.PhysicsEnabled )
			{
				entity.Velocity = entity.Velocity.WithY( 0 );
				entity.Position = entity.Position.WithY( 0 );
			}
		}
	}
}
