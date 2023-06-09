using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.InventoryLogic;
using EFT.UI;
using UnityEngine;

namespace EFT.ProfileEditor.UI;

public sealed class EquipmentItemPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public Action<Item> onItemSelected;

		public Item[] items;

		internal void _E000(int selectedIndex)
		{
			onItemSelected(items[selectedIndex]);
		}
	}

	[SerializeField]
	private DropDownBox _dropDown;

	[SerializeField]
	private CustomTextMeshProUGUI _caption;

	public void Show(string profileName, Item[] items, Action<Item> onItemSelected)
	{
		ShowGameObject();
		_caption.text = profileName + _ED3E._E000(30697);
		string[] values = items.Select((Item x) => (x != null) ? x.ShortName.Localized() : _ED3E._E000(175289)).ToArray();
		_dropDown.Show(values);
		_dropDown.Bind(delegate(int selectedIndex)
		{
			onItemSelected(items[selectedIndex]);
		});
	}
}
