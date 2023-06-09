using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Comfort.Common;
using EFT.UI;
using UnityEngine;

namespace EFT.Hideout;

public sealed class ChangeLightPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ELightingLevel level;

		public ComplementaryButton button;

		public ChangeLightPanel _003C_003E4__this;

		internal void _E000(bool arg)
		{
			_003C_003E4__this._E001(level);
			button.CloseTooltip();
		}

		internal string _E001()
		{
			return _003C_003E4__this._E000(level, button);
		}
	}

	private const string _E04C = "hideout/Disable all light";

	private const string _E04D = "hideout/Enable candles";

	private const string _E04E = "hideout/Enable light bulbs";

	private const string _E04F = "hideout/Enable gas lamps";

	private const string _E050 = "hideout/Enable christmas lights";

	private const string _E051 = "hideout/Enable halloween lights";

	private const string _E052 = "This light level has not been unlocked yet";

	[SerializeField]
	private Dictionary<ELightingLevel, ComplementaryButton> _lightButtons;

	[SerializeField]
	private Dictionary<ELightingLevel, AudioClip> _sounds;

	[CompilerGenerated]
	private Action<Sprite> _E053;

	[CompilerGenerated]
	private Action<ELightingLevel> _E054;

	private HashSet<ELightingLevel> _E055 = new HashSet<ELightingLevel>();

	private ELightingLevel _E056;

	public event Action<Sprite> OnButtonSelected
	{
		[CompilerGenerated]
		add
		{
			Action<Sprite> action = _E053;
			Action<Sprite> action2;
			do
			{
				action2 = action;
				Action<Sprite> value2 = (Action<Sprite>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E053, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<Sprite> action = _E053;
			Action<Sprite> action2;
			do
			{
				action2 = action;
				Action<Sprite> value2 = (Action<Sprite>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E053, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<ELightingLevel> OnLightningSelected
	{
		[CompilerGenerated]
		add
		{
			Action<ELightingLevel> action = _E054;
			Action<ELightingLevel> action2;
			do
			{
				action2 = action;
				Action<ELightingLevel> value2 = (Action<ELightingLevel>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E054, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ELightingLevel> action = _E054;
			Action<ELightingLevel> action2;
			do
			{
				action2 = action;
				Action<ELightingLevel> value2 = (Action<ELightingLevel>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E054, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Show()
	{
		foreach (KeyValuePair<ELightingLevel, ComplementaryButton> lightButton in _lightButtons)
		{
			var (level, button) = lightButton;
			UI.AddDisposable(button);
			button.Show(delegate
			{
				_E001(level);
				button.CloseTooltip();
			}, null, null, unlocked: false);
			button.SetUnlockStatus(_E055.Contains(level));
			button.SetTooltipMessages(() => _E000(level, button), null);
			if (level == _E056)
			{
				_E053?.Invoke(button.Icon);
			}
		}
	}

	public void Hide()
	{
		HideGameObject();
	}

	private string _E000(ELightingLevel level, ComplementaryButton button)
	{
		if (!button.IsUnlocked)
		{
			return _ED3E._E000(164060);
		}
		return level switch
		{
			ELightingLevel.Off => _ED3E._E000(164087), 
			ELightingLevel.Candles => _ED3E._E000(164121), 
			ELightingLevel.LightBulbs => _ED3E._E000(164096), 
			ELightingLevel.FluorescentLamps => _ED3E._E000(164139), 
			ELightingLevel.ChristmasLights => _ED3E._E000(164172), 
			ELightingLevel.HalloweenLights => _ED3E._E000(164204), 
			_ => string.Empty, 
		};
	}

	private void _E001(ELightingLevel level)
	{
		Hide();
		SetCurrentLightingLevel(level);
		_E054?.Invoke(level);
		if (_sounds.TryGetValue(level, out var value))
		{
			Singleton<GUISounds>.Instance.PlaySound(value, single: true);
		}
	}

	public void SetCurrentLightingLevel(ELightingLevel level)
	{
		_E056 = level;
		foreach (KeyValuePair<ELightingLevel, ComplementaryButton> lightButton in _lightButtons)
		{
			_E39D.Deconstruct(lightButton, out var key, out var value);
			ELightingLevel num = key;
			ComplementaryButton complementaryButton = value;
			bool flag = num == _E056;
			complementaryButton.SetSelectedStatus(flag);
			if (flag)
			{
				_E053?.Invoke(complementaryButton.Icon);
			}
		}
	}

	public void AddLightingLevel(ELightingLevel level, bool switchToNewLevel)
	{
		if (_lightButtons.TryGetValue(level, out var value))
		{
			_E055.Add(level);
			value.SetUnlockStatus(isUnlocked: true);
			if (switchToNewLevel)
			{
				_E001(level);
			}
		}
	}

	public void ClearLightLevels()
	{
		_E055.Clear();
		foreach (KeyValuePair<ELightingLevel, ComplementaryButton> lightButton in _lightButtons)
		{
			_E39D.Deconstruct(lightButton, out var _, out var value);
			value.SetUnlockStatus(isUnlocked: false);
		}
		SetCurrentLightingLevel(ELightingLevel.Off);
	}
}
