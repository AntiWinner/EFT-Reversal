using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.Ballistics;
using UnityEngine;

namespace EFT.InventoryLogic;

public class ArmorComponent : _EB19
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ArmorComponent _003C_003E4__this;

		public Item item;

		internal float _E000()
		{
			return _003C_003E4__this.ArmorZone.Length;
		}

		internal string _E001()
		{
			EHeadSegment[] source = item.GetItemComponentsInChildren<CompositeArmorComponent>().SelectMany((CompositeArmorComponent x) => x.HeadSegments).Distinct()
				.ToArray();
			bool flag = source.Any();
			IEnumerable<object> first = source.Cast<object>();
			IEnumerable<EBodyPart> source2;
			if (!flag)
			{
				IEnumerable<EBodyPart> armorZone = _003C_003E4__this.ArmorZone;
				source2 = armorZone;
			}
			else
			{
				source2 = _003C_003E4__this.ArmorZone.Where((EBodyPart x) => x != EBodyPart.Head);
			}
			return first.Concat(source2.Cast<object>()).CastToStringValue(_ED3E._E000(2540));
		}

		internal EItemAttributeDisplayType _E002()
		{
			if (_003C_003E4__this.ArmorZone.Length <= 1 && item.GetItemComponentsInChildren<CompositeArmorComponent>().SelectMany((CompositeArmorComponent x) => x.HeadSegments).Distinct()
				.Count() <= 1)
			{
				return EItemAttributeDisplayType.Compact;
			}
			return EItemAttributeDisplayType.CompactWithTooltip;
		}

		internal string _E003()
		{
			return ((_003C_003E4__this.Template.RicochetVals.y > 0.3f) ? _ED3E._E000(215188) : ((_003C_003E4__this.Template.RicochetVals.y > 0.15f) ? _ED3E._E000(215199) : _ED3E._E000(215138))).Localized();
		}

		internal string _E004()
		{
			CompositeArmorComponent[] array = item.GetItemComponentsInChildren<CompositeArmorComponent>().ToArray();
			if (array.Length > 1)
			{
				return array.Select((CompositeArmorComponent x) => x.Item.ShortName.Localized() + _ED3E._E000(12201) + x.ArmorClass).CastToStringValue(_ED3E._E000(2540));
			}
			return _003C_003E4__this.ArmorClass.ToString();
		}

		internal EItemAttributeDisplayType _E005()
		{
			if (item.GetItemComponentsInChildren<CompositeArmorComponent>().ToArray().Length <= 1)
			{
				return EItemAttributeDisplayType.Compact;
			}
			return EItemAttributeDisplayType.CompactWithTooltip;
		}

		internal string _E006()
		{
			return (_ED3E._E000(215186) + _003C_003E4__this.Template.ArmorMaterial).Localized();
		}

		internal string _E007()
		{
			return _003C_003E4__this.ArmorType.ToString().Localized();
		}

		internal float _E008()
		{
			return _003C_003E4__this.SpeedPenalty;
		}

		internal string _E009()
		{
			return _003C_003E4__this.SpeedPenalty + _ED3E._E000(215182);
		}

		internal float _E00A()
		{
			return _003C_003E4__this.MousePenalty;
		}

		internal string _E00B()
		{
			return _003C_003E4__this.MousePenalty + _ED3E._E000(215182);
		}

		internal float _E00C()
		{
			return _003C_003E4__this.WeaponErgonomicPenalty;
		}

		internal string _E00D()
		{
			return string.Format(_ED3E._E000(215177), _003C_003E4__this.WeaponErgonomicPenalty);
		}
	}

	public readonly _E9D0 Template;

	public readonly RepairableComponent Repairable;

	public readonly BuffComponent Buff;

	public MaterialType Material => Template.Material;

	public EBodyPart[] ArmorZone => Template.ArmorZone;

	public int ArmorClass => Template.ArmorClass;

	public int SpeedPenalty => Template.SpeedPenaltyPercent;

	public int MousePenalty => Template.MousePenalty;

	public int WeaponErgonomicPenalty => Template.WeaponErgonomicPenalty;

	public EDeafStrength Deaf => Template.Deaf;

	public float BluntThroughput => Template.BluntThroughput;

	public EArmorType ArmorType => Template.ArmorType;

	public bool IsDestroyed => Repairable.Durability < 0.01f;

	public ArmorComponent(Item item, _E9D0 template, RepairableComponent repairable, BuffComponent buffComponent)
		: base(item)
	{
		ArmorComponent armorComponent = this;
		Template = template;
		Repairable = repairable;
		Buff = buffComponent;
		item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.ArmorZone)
		{
			Name = EItemAttributeId.ArmorZone.GetName(),
			Base = () => armorComponent.ArmorZone.Length,
			StringValue = delegate
			{
				EHeadSegment[] source = item.GetItemComponentsInChildren<CompositeArmorComponent>().SelectMany((CompositeArmorComponent x) => x.HeadSegments).Distinct()
					.ToArray();
				bool flag = source.Any();
				IEnumerable<object> first = source.Cast<object>();
				IEnumerable<EBodyPart> source2;
				if (!flag)
				{
					IEnumerable<EBodyPart> armorZone = armorComponent.ArmorZone;
					source2 = armorZone;
				}
				else
				{
					source2 = armorComponent.ArmorZone.Where((EBodyPart x) => x != EBodyPart.Head);
				}
				return first.Concat(source2.Cast<object>()).CastToStringValue(_ED3E._E000(2540));
			},
			DisplayType = () => (armorComponent.ArmorZone.Length <= 1 && item.GetItemComponentsInChildren<CompositeArmorComponent>().SelectMany((CompositeArmorComponent x) => x.HeadSegments).Distinct()
				.Count() <= 1) ? EItemAttributeDisplayType.Compact : EItemAttributeDisplayType.CompactWithTooltip
		});
		if (ArmorZone.Contains(EBodyPart.Head))
		{
			item.Attributes.Add(new _EB10(EItemAttributeId.Ricochet)
			{
				Name = EItemAttributeId.Ricochet.GetName(),
				StringValue = () => ((armorComponent.Template.RicochetVals.y > 0.3f) ? _ED3E._E000(215188) : ((armorComponent.Template.RicochetVals.y > 0.15f) ? _ED3E._E000(215199) : _ED3E._E000(215138))).Localized(),
				DisplayType = () => EItemAttributeDisplayType.Compact
			});
		}
		item.Attributes.Add(new _EB10(EItemAttributeId.ArmorClass)
		{
			Name = EItemAttributeId.ArmorClass.GetName(),
			StringValue = delegate
			{
				CompositeArmorComponent[] array = item.GetItemComponentsInChildren<CompositeArmorComponent>().ToArray();
				return (array.Length > 1) ? array.Select((CompositeArmorComponent x) => x.Item.ShortName.Localized() + _ED3E._E000(12201) + x.ArmorClass).CastToStringValue(_ED3E._E000(2540)) : armorComponent.ArmorClass.ToString();
			},
			DisplayType = () => (item.GetItemComponentsInChildren<CompositeArmorComponent>().ToArray().Length <= 1) ? EItemAttributeDisplayType.Compact : EItemAttributeDisplayType.CompactWithTooltip
		});
		item.Attributes.Add(new _EB10(EItemAttributeId.ArmorMaterial)
		{
			Name = EItemAttributeId.ArmorMaterial.GetName(),
			StringValue = () => (_ED3E._E000(215186) + armorComponent.Template.ArmorMaterial).Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		if (ArmorType == EArmorType.Light || ArmorType == EArmorType.Heavy)
		{
			item.Attributes.Add(new _EB10(EItemAttributeId.ArmorType)
			{
				Name = EItemAttributeId.ArmorType.GetName(),
				StringValue = () => armorComponent.ArmorType.ToString().Localized(),
				DisplayType = () => EItemAttributeDisplayType.Compact
			});
		}
		item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.ChangeMovementSpeed)
		{
			Name = EItemAttributeId.ChangeMovementSpeed.GetName(),
			Base = () => armorComponent.SpeedPenalty,
			StringValue = () => armorComponent.SpeedPenalty + _ED3E._E000(215182),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
		item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.ChangeTurningSpeed)
		{
			Name = EItemAttributeId.ChangeTurningSpeed.GetName(),
			Base = () => armorComponent.MousePenalty,
			StringValue = () => armorComponent.MousePenalty + _ED3E._E000(215182),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
		item.SafelyAddAttributeToList(new _EB10(EItemAttributeId.Ergonomics)
		{
			Name = EItemAttributeId.Ergonomics.GetName(),
			Base = () => armorComponent.WeaponErgonomicPenalty,
			StringValue = () => string.Format(_ED3E._E000(215177), armorComponent.WeaponErgonomicPenalty),
			DisplayType = () => EItemAttributeDisplayType.Compact,
			LabelVariations = EItemAttributeLabelVariations.Colored
		});
	}

	public virtual bool ShotMatches(EBodyPart bodyPartType, int pitch = 60, int yaw = 0)
	{
		return ArmorZone.Contains(bodyPartType);
	}

	public bool Deflects(Vector3 shotDirection, Vector3 hitNormal, _EC26 shot)
	{
		Vector3 ricochetVals = Template.RicochetVals;
		if (ricochetVals.x > 0f)
		{
			float num = Vector3.Angle(-shotDirection, hitNormal);
			if (num > ricochetVals.z)
			{
				float t = Mathf.InverseLerp(90f, ricochetVals.z, num);
				float num2 = Mathf.Lerp(ricochetVals.x, ricochetVals.y, t);
				if (shot.Randoms.GetRandomFloat(shot.RandomSeed) < num2)
				{
					shot.DeflectedBy = Item.Id;
					return true;
				}
			}
		}
		return false;
	}

	public void SetPenetrationStatus(_EC26 shot)
	{
		if (!(Repairable.Durability <= 0f))
		{
			float penetrationPower = shot.PenetrationPower;
			float num = Repairable.Durability / (float)Repairable.TemplateDurability * 100f;
			float num2 = Singleton<_E5CB>.Instance.Armor.GetArmorClass(ArmorClass).Resistance;
			float num3 = (121f - 5000f / (45f + num * 2f)) * num2 * 0.01f;
			if (((num3 >= penetrationPower + 15f) ? 0f : ((num3 >= penetrationPower) ? (0.4f * (num3 - penetrationPower - 15f) * (num3 - penetrationPower - 15f)) : (100f + penetrationPower / (0.9f * num3 - penetrationPower)))) - shot.Randoms.GetRandomFloat(shot.RandomSeed) * 100f < 0f)
			{
				shot.BlockedBy = Item.Id;
				Debug.Log(_ED3E._E000(215081));
			}
		}
	}

	public float ApplyDamage(ref _EC23 damageInfo, EBodyPart bodyPartType, bool damageInfoIsLocal, _E74F._E004 lightVestsDamageReduction, _E74F._E004 heavyVestsDamageReduction)
	{
		EDamageType damageType = damageInfo.DamageType;
		float num = 0f;
		if (!damageType.IsWeaponInduced() && damageType != EDamageType.GrenadeFragment)
		{
			return 0f;
		}
		TryShatter(damageInfo.Player, damageInfoIsLocal);
		if (!(Repairable.Durability > 0f))
		{
			return 0f;
		}
		if (damageInfo.DeflectedBy == Item.Id)
		{
			damageInfo.Damage /= 2f;
			damageInfo.ArmorDamage /= 2f;
			damageInfo.PenetrationPower /= 2f;
		}
		float num2 = Template.BluntThroughput;
		bool flag = Template.ArmorZone.Contains(bodyPartType);
		if (flag && Template.ArmorType == EArmorType.Heavy)
		{
			num2 *= 1f - (float)heavyVestsDamageReduction;
		}
		float penetrationPower = damageInfo.PenetrationPower;
		float num3 = Repairable.Durability / (float)Repairable.TemplateDurability * 100f;
		float num4 = Singleton<_E5CB>.Instance.Armor.GetArmorClass(ArmorClass).Resistance;
		float num5 = (121f - 5000f / (45f + num3 * 2f)) * num4 * 0.01f;
		if (damageInfo.BlockedBy == Item.Id || damageInfo.DeflectedBy == Item.Id)
		{
			num = damageInfo.PenetrationPower * damageInfo.ArmorDamage * Mathf.Clamp(penetrationPower / num4, 0.6f, 1.1f) * Singleton<_E5CB>.Instance.ArmorMaterials[Template.ArmorMaterial].Destructibility;
			damageInfo.Damage *= num2 * Mathf.Clamp(1f - 0.03f * (num5 - penetrationPower), 0.2f, 1f);
			damageInfo.StaminaBurnRate *= ((num2 > 0f) ? (3f / Mathf.Sqrt(num2)) : 1f);
		}
		else
		{
			num = damageInfo.PenetrationPower * damageInfo.ArmorDamage * Mathf.Clamp(penetrationPower / num4, 0.5f, 0.9f) * Singleton<_E5CB>.Instance.ArmorMaterials[Template.ArmorMaterial].Destructibility;
			float num6 = Mathf.Clamp(penetrationPower / (num5 + 12f), 0.6f, 1f);
			damageInfo.Damage *= num6;
			if (Template.ArmorType == EArmorType.Light && damageInfo.DamageType == EDamageType.Melee && flag)
			{
				damageInfo.Damage *= 1f - (float)lightVestsDamageReduction;
			}
			damageInfo.PenetrationPower *= num6;
		}
		if (Buff.IsActive && Buff.BuffType == ERepairBuffType.DamageReduction)
		{
			damageInfo.Damage *= (float)Buff.DamageReduction;
		}
		num = Mathf.Max(1f, num);
		ApplyDurabilityDamage(num);
		return num;
	}

	public void TryShatter(Player damageSource, bool damageInfoIsLocal = true)
	{
		if (Material != MaterialType.GlassVisor)
		{
			return;
		}
		FaceShieldComponent itemComponent = Item.GetItemComponent<FaceShieldComponent>();
		if (itemComponent != null)
		{
			itemComponent.StoreValidationTimestamp();
			if (damageSource != null)
			{
				damageSource.FaceshieldMarkOperation(itemComponent, damageInfoIsLocal);
			}
		}
	}

	public void ApplyDurabilityDamage(float armorDamage)
	{
		Repairable.Durability -= armorDamage;
		Buff.TryDisableComponent(Repairable.Durability);
		if (Repairable.Durability < 0f)
		{
			Repairable.Durability = 0f;
		}
		Item.RaiseRefreshEvent(refreshIcon: false, checkMagazine: false);
	}

	public float ApplyExplosionDurabilityDamage(float armorDamage, _EC23 damageInfo)
	{
		float num = armorDamage * Singleton<_E5CB>.Instance.ArmorMaterials[Template.ArmorMaterial].ExplosionDestructibility;
		if (num / (float)Repairable.TemplateDurability > 0.4f)
		{
			TryShatter(damageInfo.Player);
		}
		ApplyDurabilityDamage(num);
		return armorDamage;
	}

	public override string ToString()
	{
		return string.Format(_ED3E._E000(215113), ArmorClass, Repairable.Durability, Repairable.MaxDurability, (ArmorZone.Length != 0) ? string.Join(_ED3E._E000(10270), ArmorZone.Select((EBodyPart x) => x.ToString()).ToArray()) : _ED3E._E000(160088));
	}
}
