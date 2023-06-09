using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.HealthSystem;
using UnityEngine;

namespace EFT.UI;

public sealed class EffectsPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public SimpleTooltip tooltip;

		internal void _E000(_E98F effect, EffectIcon view)
		{
			view.Show(effect, tooltip, effect.EffectState() == EEffectState.Residued);
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E98F newEffect;

		internal bool _E000(_E98F d)
		{
			return d.Type == newEffect.Type;
		}

		internal bool _E001(_E98F d)
		{
			return d.Type == newEffect.Type;
		}
	}

	[CompilerGenerated]
	private sealed class _E002
	{
		public _E986 activeBuff;

		internal bool _E000(_E98F x)
		{
			return x.Type == activeBuff.BuffType;
		}
	}

	[SerializeField]
	private EffectIcon _effectIconTemplate;

	private EBodyPart _E0AF;

	private _E9C4 _E0AE;

	private _ECEF<_E98F> _E0C6;

	private Dictionary<Type, Sprite> _E0C7;

	private bool _E0B3;

	private bool _E0C8;

	private void Update()
	{
		if (_E0B3)
		{
			_E0B3 = false;
			_E000();
		}
	}

	public void Show(_E9C4 healthController, EBodyPart bodyPart, SimpleTooltip tooltip)
	{
		ShowGameObject();
		_E006();
		_E0AF = bodyPart;
		_E0AE = healthController;
		_E0C6 = new _ECEF<_E98F>();
		_E0C7 = EFTHardSettings.Instance.StaticIcons.EffectIcons.EffectIcons;
		_E0B3 = true;
		_E0AE.EffectStartedEvent += _E001;
		_E0AE.EffectUpdatedEvent += _E002;
		_E0AE.EffectResidualEvent += _E003;
		_E0AE.EffectRemovedEvent += _E004;
		_E0AE.StimulatorBuffEvent += _E005;
		UI.AddDisposable(_E006);
		UI.AddDisposable(new _EC71<_E98F, EffectIcon>(_E0C6, _effectIconTemplate, base.transform, delegate(_E98F effect, EffectIcon view)
		{
			view.Show(effect, tooltip, effect.EffectState() == EEffectState.Residued);
		}));
	}

	private void _E000()
	{
		List<_E98F> list = new List<_E98F>();
		List<_E992> list2 = _E0AE.GetAllActiveEffects(_E0AF).ToList();
		List<_E992> list3 = _E0AE.GetAllResidualEffects(_E0AF).ToList();
		_E992[] array = new _E992[list2.Count + list3.Count];
		list2.CopyTo(array, 0);
		list3.CopyTo(array, list2.Count);
		_E992[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			foreach (_E98F newEffect in array2[i].DisplayableVariations.Where((_E98F variation) => _E0C7.ContainsKey(variation.Type)))
			{
				switch (newEffect.BuffType)
				{
				case _E98F.EBuffType.Stackable:
				{
					_E98F obj = list.FirstOrDefault((_E98F d) => d.Type == newEffect.Type);
					if (obj != null)
					{
						if (obj.TimeLeft > newEffect.TimeLeft)
						{
							continue;
						}
						list.Remove(obj);
					}
					break;
				}
				case _E98F.EBuffType.Stimulant:
					if (list.FirstOrDefault((_E98F d) => d.Type == newEffect.Type) != null)
					{
						continue;
					}
					break;
				}
				list.Add(newEffect);
			}
		}
		foreach (_E9C0 item in _E0AE.FindActiveEffects<_E9C0>())
		{
			_E986[] activeBuffs = item.ActiveBuffs;
			foreach (_E986 activeBuff in activeBuffs)
			{
				_E98E description = new _E98E(activeBuff);
				_E98F obj2 = list.FirstOrDefault((_E98F x) => x.Type == activeBuff.BuffType);
				if (obj2 != null && activeBuff.Active)
				{
					obj2.Add(description);
				}
			}
		}
		_E0C8 = _EC46.DoFilter(list, from effect in _E0AE.GetAllEffects()
			where effect.Active
			select effect);
		_E0C6.Clear();
		_E0C6.AddRange(list);
	}

	private void _E001(_E992 effect)
	{
		if (effect.BodyPart == _E0AF || _E0C8)
		{
			_E0B3 = true;
		}
	}

	private void _E002(_E992 effect)
	{
		if (effect.BodyPart == _E0AF)
		{
			_E0B3 = true;
		}
	}

	private void _E003(_E992 effect)
	{
		if (effect.BodyPart == _E0AF)
		{
			_E0B3 = true;
		}
	}

	private void _E004(_E992 effect)
	{
		if (effect.BodyPart == _E0AF || _E0C8)
		{
			_E0B3 = true;
		}
	}

	private void _E005(_E986 buff)
	{
		if (buff.BodyPart == _E0AF)
		{
			_E0B3 = true;
		}
	}

	private void _E006()
	{
		_E0B3 = false;
		_E0C8 = false;
		if (_E0AE != null)
		{
			_E0AE.EffectStartedEvent -= _E001;
			_E0AE.EffectUpdatedEvent -= _E002;
			_E0AE.EffectResidualEvent -= _E003;
			_E0AE.EffectRemovedEvent -= _E004;
			_E0AE.StimulatorBuffEvent -= _E005;
			_E0AE = null;
			_E0C6.Clear();
			_E0C6 = null;
		}
	}

	[CompilerGenerated]
	private bool _E007(_E98F variation)
	{
		return _E0C7.ContainsKey(variation.Type);
	}
}
