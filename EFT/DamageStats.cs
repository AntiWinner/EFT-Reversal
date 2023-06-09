using System;
using System.Text;

namespace EFT;

[Serializable]
public sealed class DamageStats
{
	public enum EDamageResult
	{
		[_E618("None")]
		None,
		[_E618("Regular")]
		Regular,
		[_E618("Lethal")]
		Lethal
	}

	public float Amount;

	public EDamageType Type { get; private set; }

	public string SourceId { get; private set; }

	public EBodyPart? OverDamageFrom { get; private set; }

	public bool Blunt { get; private set; }

	public float ImpactsCount { get; private set; }

	public DamageStats(EDamageType type, float amount, EBodyPart? overDamageFrom, bool blunt = false, string sourceId = null)
	{
		Type = type;
		Amount = amount;
		OverDamageFrom = overDamageFrom;
		Blunt = blunt;
		SourceId = sourceId;
		ImpactsCount = 1f;
	}

	public bool IsEqual(DamageStats damageStats, bool compareShots = false)
	{
		if (!compareShots && !string.IsNullOrEmpty(SourceId))
		{
			return false;
		}
		if (SourceId == damageStats.SourceId && Type == damageStats.Type)
		{
			return OverDamageFrom == damageStats.OverDamageFrom;
		}
		return false;
	}

	public void Add(DamageStats damage)
	{
		Amount += damage.Amount;
		ImpactsCount += damage.ImpactsCount;
	}

	public DamageStats Clone()
	{
		return new DamageStats(Type, Amount, OverDamageFrom, Blunt, SourceId);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder((string.IsNullOrEmpty(SourceId) ? (_ED3E._E000(138108) + Type.ToStringNoBox()) : (SourceId + _ED3E._E000(70087))).Localized());
		if (!string.IsNullOrEmpty(SourceId) && ImpactsCount > 1f)
		{
			stringBuilder.AppendFormat(_ED3E._E000(47265), ImpactsCount);
		}
		if (Blunt)
		{
			stringBuilder.AppendFormat(_ED3E._E000(138096), _ED3E._E000(138086).Localized());
		}
		if (OverDamageFrom.HasValue)
		{
			stringBuilder.AppendFormat(_ED3E._E000(138126), _ED3E._E000(138168).Localized(), OverDamageFrom.ToString().Localized());
		}
		return stringBuilder.ToString();
	}
}
