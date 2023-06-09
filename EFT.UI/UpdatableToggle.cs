using System;
using System.Runtime.CompilerServices;
using Comfort.Common;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class UpdatableToggle : Toggle, _EC72<bool>
{
	private Action<bool> m__E000;

	protected override void Awake()
	{
		base.Awake();
		onValueChanged.AddListener(delegate(bool arg)
		{
			UpdateValue(arg);
		});
	}

	public void UpdateValue(bool value, bool sendCallback = true, bool? min = null, bool? max = null)
	{
		this.Set(value);
		if (this.m__E000 != null && sendCallback)
		{
			Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuCheckBox);
			this.m__E000(value);
		}
	}

	public void Bind(Action<bool> valueChanged)
	{
		this.m__E000 = valueChanged;
	}

	public bool CurrentValue()
	{
		return base.isOn;
	}

	[CompilerGenerated]
	private void _E000(bool arg)
	{
		UpdateValue(arg);
	}
}
