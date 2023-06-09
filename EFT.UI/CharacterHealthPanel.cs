using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using EFT.HealthSystem;
using EFT.UI.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

[RequireComponent(typeof(BattleUIComponentAnimation))]
public sealed class CharacterHealthPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Dictionary<Type, Sprite> icons;

		internal bool _E000(_E98F x)
		{
			return icons.ContainsKey(x.Type);
		}

		internal Sprite _E001(_E98F x)
		{
			return icons[x.Type];
		}
	}

	[CompilerGenerated]
	private sealed class _E001
	{
		public _E98F effectDescription;

		internal bool _E000(_E98F b)
		{
			return b.Type == effectDescription.Type;
		}
	}

	[SerializeField]
	private Image _background;

	[SerializeField]
	private BodyPartImage _headImage;

	[SerializeField]
	private BodyPartImage _chestImage;

	[SerializeField]
	private BodyPartImage _stomachImage;

	[SerializeField]
	private BodyPartImage _leftHandImage;

	[SerializeField]
	private BodyPartImage _rightHandImage;

	[SerializeField]
	private BodyPartImage _leftLegImage;

	[SerializeField]
	private BodyPartImage _rightLegImage;

	[SerializeField]
	private GameObject _effectsPanel;

	[SerializeField]
	private StackedEffectIcon _effectIconTemplate;

	private BattleUIComponentAnimation _E093;

	private _E9C4 _E0AE;

	private _ECEF<_E98F> _E0B2;

	private bool _E0B3;

	[CompilerGenerated]
	private bool _E0B4;

	public bool AutoHide
	{
		[CompilerGenerated]
		get
		{
			return _E0B4;
		}
		[CompilerGenerated]
		private set
		{
			_E0B4 = value;
		}
	}

	public bool AnyCriticalEffects => _E002().Any((_E992 effect) => effect.Critical);

	private void Update()
	{
		if (_E0B3)
		{
			_E0B3 = false;
			_E001();
		}
	}

	public void Show(_E9C4 healthController)
	{
		UI.Dispose();
		ShowGameObject();
		if (_E093 == null)
		{
			_E093 = base.gameObject.GetComponent<BattleUIComponentAnimation>();
		}
		_E0AE = healthController;
		_E0B2 = new _ECEF<_E98F>();
		_E0AE.EffectStartedEvent += _E003;
		_E0AE.EffectUpdatedEvent += _E004;
		_E0AE.EffectResidualEvent += _E005;
		_E0AE.EffectRemovedEvent += _E006;
		_E0AE.StimulatorBuffEvent += _E007;
		_headImage.Show(_E0AE, EBodyPart.Head);
		_chestImage.Show(_E0AE, EBodyPart.Chest);
		_stomachImage.Show(_E0AE, EBodyPart.Stomach);
		_leftHandImage.Show(_E0AE, EBodyPart.LeftArm);
		_rightHandImage.Show(_E0AE, EBodyPart.RightArm);
		_leftLegImage.Show(_E0AE, EBodyPart.LeftLeg);
		_rightLegImage.Show(_E0AE, EBodyPart.RightLeg);
		Color white = Color.white;
		if (Singleton<_E7DE>.Instance.Game.Settings.HealthColor.Value == EHealthColorScheme.GreenToRed)
		{
			white.a = 0.2f;
		}
		_background.color = white;
		UI.AddDisposable(_E000);
		Dictionary<Type, Sprite> icons = EFTHardSettings.Instance.StaticIcons.EffectIcons.EffectIcons;
		UI.AddDisposable(new _EC71<_ED02<_E98F>, StackedEffectIcon>(_E0B2.BindWhere((_E98F x) => icons.ContainsKey(x.Type)).Counted((_E98F x) => icons[x.Type]), _effectIconTemplate, _effectsPanel.transform, delegate(_ED02<_E98F> effect, StackedEffectIcon icon)
		{
			icon.Show(effect, (_E98F e) => e.BuffType != _E98F.EBuffType.Stackable && e.BuffType != _E98F.EBuffType.Stimulant, effect.Count.Value == 1 && effect.Value.EffectState() == EEffectState.Residued);
		}));
		_E0B3 = true;
		_E093.Close();
	}

	public void AnimatedShow(bool autohide)
	{
		AutoHide = autohide;
		if (_E093 != null)
		{
			_E093.Show(autohide).HandleExceptions();
		}
	}

	public void AnimatedHide(float delaySeconds = 0f)
	{
		if (_E093 != null)
		{
			_E093.Hide(delaySeconds).HandleExceptions();
		}
	}

	private void _E000()
	{
		if (_E093 != null)
		{
			_E093.StopAnimation();
		}
		_E0B3 = false;
		if (_E0AE != null)
		{
			_E0AE.EffectStartedEvent -= _E003;
			_E0AE.EffectUpdatedEvent -= _E004;
			_E0AE.EffectResidualEvent -= _E005;
			_E0AE.EffectRemovedEvent -= _E006;
			_E0AE.StimulatorBuffEvent -= _E007;
			_E0AE = null;
		}
	}

	private void _E001()
	{
		List<_E98F> list = new List<_E98F>();
		foreach (_E98F effectDescription in _E002().SelectMany((_E992 x) => x.DisplayableVariations).ToList())
		{
			_E98F obj = list.FirstOrDefault((_E98F b) => b.Type == effectDescription.Type);
			if (obj != null && obj.BuffType == _E98F.EBuffType.Stimulant)
			{
				obj.Buffs.AddRange(effectDescription.Buffs);
			}
			else
			{
				list.Add(effectDescription);
			}
		}
		_EC46.DoFilter(list);
		_E0B2.Clear();
		_E0B2.AddRange(list);
	}

	private IEnumerable<_E992> _E002()
	{
		return from effect in _E0AE.GetAllEffects()
			where effect.Active || (effect.Critical && effect.Residual)
			select effect;
	}

	private void _E003(_E992 effect)
	{
		_E0B3 = true;
	}

	private void _E004(_E992 effect)
	{
		_E0B3 = true;
	}

	private void _E005(_E992 effect)
	{
		_E0B3 = true;
	}

	private void _E006(_E992 effect)
	{
		_E0B3 = true;
	}

	private void _E007(_E986 buff)
	{
		_E0B3 = true;
	}
}
