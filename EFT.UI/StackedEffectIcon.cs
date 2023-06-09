using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace EFT.UI;

public sealed class StackedEffectIcon : EffectIcon
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public StackedEffectIcon _003C_003E4__this;

		public Func<_E98F, bool> counterVisibility;

		public _ED02<_E98F> variation;

		internal void _E000(int count)
		{
			_003C_003E4__this._stackCount.gameObject.SetActive(count > 1 && counterVisibility(variation.Value));
			if (count > 1)
			{
				_003C_003E4__this._stackCount.text = count.ToString();
			}
		}
	}

	[SerializeField]
	private TextMeshProUGUI _stackCount;

	public void Show(_ED02<_E98F> variation, Func<_E98F, bool> counterVisibility, bool blinking = false)
	{
		Show(variation.Value, null, blinking);
		UI.AddDisposable(variation.Count.Bind(delegate(int count)
		{
			_stackCount.gameObject.SetActive(count > 1 && counterVisibility(variation.Value));
			if (count > 1)
			{
				_stackCount.text = count.ToString();
			}
		}));
	}

	public override void Close()
	{
		base.Close();
		UI.Dispose();
		HideGameObject();
	}
}
