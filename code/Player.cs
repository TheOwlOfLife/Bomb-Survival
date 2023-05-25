﻿using Sandbox.Internal;

namespace BombSurvival;

public partial class Player : AnimatedEntity
{
	public float CollisionHeight => 72f;
	public float CollisionWidth => 24f;
	public BBox CollisionBox => new BBox( new Vector3( -CollisionWidth / 2f, -CollisionWidth / 2f, 0f ) * Scale, new Vector3( CollisionWidth / 2f, CollisionWidth / 2f, CollisionHeight ) * Scale );
	public Capsule CollisionCapsule => new Capsule( Vector3.Zero.WithZ( CollisionWidth / 2f ) * Scale, Vector3.Zero.WithZ( CollisionHeight - CollisionWidth / 2f ) * Scale, CollisionWidth / 2f * Scale );

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/citizen/citizen.vmdl" );
		SetupPhysicsFromCapsule( PhysicsMotionType.Keyframed, CollisionCapsule );
	}

	[ClientInput] public Vector3 InputDirection { get; protected set; }
	[ClientInput] public Rotation InputRotation { get; set; }
	TimeSince lastRotation = 0f;

	public override void BuildInput()
	{
		InputDirection = Input.AnalogMove;

		var lookInput = Input.AnalogLook;
		var direction = new Vector3( -lookInput.yaw, 0f, -lookInput.pitch ).Normal;

		if ( lookInput != Angles.Zero )
		{
			InputRotation = Rotation.LookAt( direction, Vector3.Left );
			lastRotation = 0f;
		}
		else
			if ( lastRotation >= 3f )
				InputRotation = Rotation.LookAt( Vector3.Right, Vector3.Up );
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		ComputeAnimations();
		ComputeMotion();
	}

	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );

		Camera.Position = Position + Vector3.Right * 200f + Vector3.Up * 64f;
		Camera.Rotation = Rotation.FromYaw( 90f );

		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );

		ComputeAnimations();
	}
}
