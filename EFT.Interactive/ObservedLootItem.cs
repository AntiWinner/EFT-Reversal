using Comfort.Common;
using UnityEngine;

namespace EFT.Interactive;

public sealed class ObservedLootItem : LootItem
{
	public override float PhysicsQuality => 0f;

	protected override _E383 GetVisibilityChecker()
	{
		if (!(_E8A8.Instance.Camera != null) || !_E7A3.IsNetworkGame)
		{
			return null;
		}
		return new _E383(_E8A8.Instance.Camera, base.gameObject, _E2B6.Config.Physics.CullingForLoot);
	}

	protected override bool IsRigidbodyDone()
	{
		if (!_E7A3.IsNetworkGame)
		{
			return base.IsRigidbodyDone();
		}
		if (_rigidBody.collisionDetectionMode == CollisionDetectionMode.Continuous && _rigidBody.velocity.sqrMagnitude <= 0.008f)
		{
			return _currentPhysicsTime >= 15f;
		}
		return _currentPhysicsTime >= 30f;
	}

	public void ApplyNetPacket(_E5C3 packet)
	{
		base.transform.SetPositionAndRotation(packet.Position, packet.Rotation);
		if (_rigidBody != null)
		{
			_rigidBody.velocity = packet.Velocity;
			_rigidBody.angularVelocity = packet.AngularVelocity;
		}
		if (packet.Done)
		{
			if (base.Platform == null)
			{
				Singleton<GameWorld>.Instance.BoardIfOnPlatform(this, base.transform.position);
			}
			PlayDropSound();
			StopPhysics();
		}
	}
}
