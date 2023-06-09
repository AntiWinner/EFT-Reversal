using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class FilterButton : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Action<FilterButton> onClick;

		public FilterButton _003C_003E4__this;

		internal void _E000()
		{
			onClick(_003C_003E4__this);
		}
	}

	[SerializeField]
	protected Button _button;

	[SerializeField]
	protected Image _icon;

	[SerializeField]
	protected Sprite _on;

	[SerializeField]
	protected Sprite _off;

	public void Show(Action<FilterButton> onClick)
	{
		ShowGameObject();
		UI.SubscribeEvent(_button.onClick, delegate
		{
			onClick(this);
		});
	}

	public void ApplyAscend()
	{
		_icon.sprite = _on;
	}

	public void ApplyDescend()
	{
		_icon.sprite = _off;
	}

	public void ShowIcon()
	{
		_icon.gameObject.SetActive(value: true);
	}

	public void HideIcon()
	{
		_icon.gameObject.SetActive(value: false);
	}
}
