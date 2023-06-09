using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using UnityEngine;

namespace EFT.UI.WeaponModding;

public sealed class DropDownMenu : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemView itemView;

		internal void _E000()
		{
			itemView.Kill();
		}
	}

	[SerializeField]
	private RectTransform _noneItemContainer;

	[SerializeField]
	private EmptyItemView _emptyItemViewTemplate;

	[SerializeField]
	private RectTransform _itemsContainer;

	[CompilerGenerated]
	private Action<ModdingScreenSlotView> _E23D;

	[CompilerGenerated]
	private Action<ModdingScreenSlotView> _E23E;

	private ModdingScreenSlotView _E23F;

	private ItemUiContext _E089;

	private readonly List<Item> _E19E = new List<Item>();

	[CompilerGenerated]
	private bool _E240;

	[CompilerGenerated]
	private _EB65 _E241;

	public bool Open
	{
		[CompilerGenerated]
		get
		{
			return _E240;
		}
		[CompilerGenerated]
		private set
		{
			_E240 = value;
		}
	}

	public _EB65 ItemContext
	{
		[CompilerGenerated]
		get
		{
			return _E241;
		}
		[CompilerGenerated]
		private set
		{
			_E241 = value;
		}
	}

	public event Action<ModdingScreenSlotView> OnMenuOpen
	{
		[CompilerGenerated]
		add
		{
			Action<ModdingScreenSlotView> action = _E23D;
			Action<ModdingScreenSlotView> action2;
			do
			{
				action2 = action;
				Action<ModdingScreenSlotView> value2 = (Action<ModdingScreenSlotView>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E23D, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ModdingScreenSlotView> action = _E23D;
			Action<ModdingScreenSlotView> action2;
			do
			{
				action2 = action;
				Action<ModdingScreenSlotView> value2 = (Action<ModdingScreenSlotView>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E23D, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action<ModdingScreenSlotView> OnMenuClosed
	{
		[CompilerGenerated]
		add
		{
			Action<ModdingScreenSlotView> action = _E23E;
			Action<ModdingScreenSlotView> action2;
			do
			{
				action2 = action;
				Action<ModdingScreenSlotView> value2 = (Action<ModdingScreenSlotView>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E23E, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<ModdingScreenSlotView> action = _E23E;
			Action<ModdingScreenSlotView> action2;
			do
			{
				action2 = action;
				Action<ModdingScreenSlotView> value2 = (Action<ModdingScreenSlotView>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E23E, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public void Show(_EB65 itemContext, ModdingScreenSlotView slotView, _EB1E itemController)
	{
		_E23F = slotView;
		ItemContext = itemContext;
		_E089 = ItemUiContext.Instance;
		IEnumerable<Item> allItems = itemContext.ModdingSelectionContext.GetAllItems(itemContext.TargetAddress);
		_E19E.Clear();
		foreach (Item item in allItems)
		{
			_E19E.Add(item);
		}
		EmptyItemView emptyItemView = UnityEngine.Object.Instantiate(_emptyItemViewTemplate, _noneItemContainer);
		emptyItemView.Show(itemContext);
		UI.AddDisposable(emptyItemView);
		foreach (Item item2 in _E19E)
		{
			_E000(itemContext, item2, itemController, _itemsContainer);
		}
		_E23D?.Invoke(slotView);
		_E001();
		base.gameObject.SetActive(value: true);
		Open = true;
	}

	private void _E000(_EB66 sourceContext, Item item, _EB1E itemController, RectTransform container)
	{
		ItemView itemView = _E089.CreateItemView(item, sourceContext, ItemRotation.Horizontal, itemController, null, null, null, slotView: false, canSelect: true, searched: true);
		Transform obj = itemView.transform;
		obj.localPosition = Vector3.zero;
		obj.rotation = Quaternion.identity;
		obj.localScale = Vector3.one;
		obj.SetParent(container, worldPositionStays: false);
		UI.AddDisposable(delegate
		{
			itemView.Kill();
		});
	}

	private void _E001()
	{
		base.transform.position = _E23F.MenuAnchor.position;
		CorrectPosition(new _E3F3
		{
			Bottom = 30f
		});
	}

	private void Update()
	{
		_E001();
	}

	public override void Close()
	{
		if (Open)
		{
			Open = false;
			_E23E?.Invoke(_E23F);
			_E23F = null;
			base.Close();
		}
	}
}
