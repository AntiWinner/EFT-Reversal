using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI.Ragfair;

public sealed class RagfairFilterButton : FilterButton
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Action<RagfairFilterButton> onClick;

		public RagfairFilterButton _003C_003E4__this;

		internal void _E000()
		{
			onClick(_003C_003E4__this);
		}
	}

	[SerializeField]
	private GameObject _loader;

	public void Show(Action<RagfairFilterButton> onClick)
	{
		ShowGameObject();
		UI.SubscribeEvent(_button.onClick, delegate
		{
			onClick(this);
		});
	}

	public void ShowLoader(bool finished)
	{
		_loader.gameObject.SetActive(!finished);
		_icon.gameObject.SetActive(finished);
	}
}
