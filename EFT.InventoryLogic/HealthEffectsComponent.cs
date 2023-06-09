using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.HealthSystem;

namespace EFT.InventoryLogic;

public sealed class HealthEffectsComponent : StimulatorBuffsComponent
{
	private readonly _E9DD m__E000;

	public float UseTime => this.m__E000.UseTime;

	public KeyValuePair<EBodyPart, float>[] BodyPartTimeMults => this.m__E000.BodyPartTimeMults;

	public Dictionary<EHealthFactorType, _E561> HealthEffects => this.m__E000.HealthEffects;

	public Dictionary<EDamageEffectType, _E560> DamageEffects => this.m__E000.DamageEffects;

	private string _E001 => UseTime.ToString(_ED3E._E000(215469));

	public HealthEffectsComponent(Item item, _E9DD template)
		: base(item, template)
	{
		this.m__E000 = template;
		Item.Attributes.Add(new _EB11(EItemAttributeId.UseTime)
		{
			Name = EItemAttributeId.UseTime.GetName(),
			StringValue = () => this._E001 + _ED3E._E000(124724).Localized(),
			FullStringValue = () => _ED3E._E000(215465).Localized() + _ED3E._E000(18502) + this._E001 + _ED3E._E000(124724).Localized(),
			DisplayType = () => EItemAttributeDisplayType.Compact
		});
		Item.CreateAttributesFromDictionary(HealthEffects, EItemAttributeDisplayType.Compact, EItemAttributeLabelVariations.Colored);
		Item.CreateAttributesFromDictionary(DamageEffects, EItemAttributeDisplayType.Compact);
		SetupStimulatorBuffsAttributes(item);
	}

	public bool AffectsAny(params EDamageEffectType[] effectTypes)
	{
		return effectTypes.Any((EDamageEffectType type) => DamageEffects.ContainsKey(type));
	}

	public float UseTimeFor(EBodyPart bodyPart)
	{
		if (BodyPartTimeMults == null)
		{
			return UseTime;
		}
		KeyValuePair<EBodyPart, float>[] bodyPartTimeMults = BodyPartTimeMults;
		for (int i = 0; i < bodyPartTimeMults.Length; i++)
		{
			var (eBodyPart2, num2) = bodyPartTimeMults[i];
			if (eBodyPart2 == bodyPart)
			{
				return UseTime * num2;
			}
		}
		return UseTime;
	}

	[CompilerGenerated]
	private string _E000()
	{
		return this._E001 + _ED3E._E000(124724).Localized();
	}

	[CompilerGenerated]
	private string _E001()
	{
		return _ED3E._E000(215465).Localized() + _ED3E._E000(18502) + this._E001 + _ED3E._E000(124724).Localized();
	}

	[CompilerGenerated]
	private bool _E002(EDamageEffectType type)
	{
		return DamageEffects.ContainsKey(type);
	}
}
