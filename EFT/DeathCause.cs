using System;

namespace EFT;

[Serializable]
public sealed class DeathCause
{
	public EDamageType DamageType;

	public EPlayerSide Side;

	public WildSpawnType Role;

	public string WeaponId;
}
