using System;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.HealthSystem;
using UnityEngine;

namespace EFT.InventoryLogic;

public class StimulatorBuffsComponent : _EB19
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public _E989._E016._E000 buff;

		internal string _E000()
		{
			return _ED3E._E000(215476).Localized() + _ED3E._E000(215474) + buff.SkillName.Localized() + _ED3E._E000(147706);
		}

		internal string _E001()
		{
			return buff.GetStringValue();
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public string name;

		public _E000 CS_0024_003C_003E8__locals1;

		internal string _E000()
		{
			return CS_0024_003C_003E8__locals1.buff.GetFullStringValue(name);
		}
	}

	private readonly _E9DC m__E000;

	public string StimulatorBuffs => this.m__E000.StimulatorBuffs;

	protected _E989._E016._E000[] BuffSettings
	{
		get
		{
			if (!Singleton<_E5CB>.Instantiated)
			{
				return new _E989._E016._E000[0];
			}
			_E989._E016 stimulator = Singleton<_E5CB>.Instance.Health.Effects.Stimulator;
			if (string.IsNullOrEmpty(StimulatorBuffs) || stimulator == null)
			{
				return new _E989._E016._E000[0];
			}
			if (stimulator.Buffs.ContainsKey(StimulatorBuffs))
			{
				return stimulator.Buffs[StimulatorBuffs];
			}
			Debug.LogError(_ED3E._E000(215398) + StimulatorBuffs + _ED3E._E000(215442));
			return new _E989._E016._E000[0];
		}
	}

	protected StimulatorBuffsComponent(Item item, _E9DC template)
		: base(item)
	{
		this.m__E000 = template;
	}

	protected void SetupStimulatorBuffsAttributes(Item item)
	{
		if (!Singleton<_E5CB>.Instantiated)
		{
			return;
		}
		_E989._E016 stimulator = Singleton<_E5CB>.Instance.Health.Effects.Stimulator;
		if (string.IsNullOrEmpty(StimulatorBuffs) || stimulator == null)
		{
			return;
		}
		if (!stimulator.Buffs.TryGetValue(StimulatorBuffs, out var value))
		{
			Debug.LogError(_ED3E._E000(215398) + StimulatorBuffs + _ED3E._E000(215442));
			return;
		}
		_E989._E016._E000[] array = value;
		foreach (_E989._E016._E000 buff in array)
		{
			string name = ((buff.BuffType == EStimulatorBuffType.SkillRate) ? null : buff.BuffType.ToString());
			Func<string> displayNameFunc = ((buff.BuffType == EStimulatorBuffType.SkillRate) ? ((Func<string>)(() => _ED3E._E000(215476).Localized() + _ED3E._E000(215474) + buff.SkillName.Localized() + _ED3E._E000(147706))) : null);
			item.Attributes.Add(new _E9DE(buff.BuffType)
			{
				Name = name,
				DisplayNameFunc = displayNameFunc,
				StringValue = () => buff.GetStringValue(),
				DisplayType = () => EItemAttributeDisplayType.Compact,
				FullStringValue = () => buff.GetFullStringValue(name),
				LabelVariations = EItemAttributeLabelVariations.Colored
			});
		}
	}
}
