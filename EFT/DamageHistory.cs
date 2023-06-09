using System;
using System.Collections.Generic;

namespace EFT;

[Serializable]
public sealed class DamageHistory
{
	public EBodyPart LethalDamagePart;

	public DamageStats LethalDamage;

	public Dictionary<EBodyPart, List<DamageStats>> BodyParts = new Dictionary<EBodyPart, List<DamageStats>>();

	public void Clear()
	{
		BodyParts.Clear();
		LethalDamage = null;
		LethalDamagePart = EBodyPart.Common;
	}
}
