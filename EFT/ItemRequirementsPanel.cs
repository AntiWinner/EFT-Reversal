using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace EFT;

public class ItemRequirementsPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemRequirementsPanel _003C_003E4__this;

		public Dictionary<ECurrencyType, int> money;

		public Action<bool> onAvailableChanged;

		internal void _E000(_EBE5 item, RequiredItem view)
		{
			if (_EA10.TryGetCurrencyType(item._tpl, out var type))
			{
				Sprite sprite = type switch
				{
					ECurrencyType.EUR => _003C_003E4__this._euroSprite, 
					ECurrencyType.USD => _003C_003E4__this._dollarSprite, 
					_ => _003C_003E4__this._rubSprite, 
				};
				int num = money[type];
				bool flag = item.count <= (float)num;
				view.Show(item, flag, sprite);
				if (!flag && _003C_003E4__this._E00B)
				{
					_003C_003E4__this._E00B = false;
					onAvailableChanged?.Invoke(obj: false);
				}
			}
			else
			{
				view.HideGameObject();
			}
		}
	}

	[SerializeField]
	private RequiredItem _template;

	[SerializeField]
	private Sprite _rubSprite;

	[SerializeField]
	private Sprite _euroSprite;

	[SerializeField]
	private Sprite _dollarSprite;

	private bool _E00B;

	public void Show(_E9EF stashGrid, _EBE5[] requirements, Action<bool> onAvailableChanged)
	{
		ShowGameObject();
		_E00B = true;
		CreateItemsViews(stashGrid, requirements, onAvailableChanged);
	}

	public void CreateItemsViews(_E9EF stashGrid, [CanBeNull] _EBE5[] requirements, [CanBeNull] Action<bool> onAvailableChanged)
	{
		if (requirements == null || requirements.Length == 0)
		{
			return;
		}
		Dictionary<ECurrencyType, int> money = _EB0E.GetMoneySums(stashGrid.ContainedItems.Keys);
		UI.AddViewList(requirements, _template, base.transform, delegate(_EBE5 item, RequiredItem view)
		{
			if (_EA10.TryGetCurrencyType(item._tpl, out var type))
			{
				Sprite sprite = type switch
				{
					ECurrencyType.EUR => _euroSprite, 
					ECurrencyType.USD => _dollarSprite, 
					_ => _rubSprite, 
				};
				int num = money[type];
				bool flag = item.count <= (float)num;
				view.Show(item, flag, sprite);
				if (!flag && _E00B)
				{
					_E00B = false;
					onAvailableChanged?.Invoke(obj: false);
				}
			}
			else
			{
				view.HideGameObject();
			}
		});
	}
}
