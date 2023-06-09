using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Comfort.Common;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public class SlotView : MonoBehaviour, _EC9E, _E640, _E63F, _E641, _E64C, _E64D
{
	[CompilerGenerated]
	private sealed class _E001
	{
		public _EB47 _003CsplitResult_003E5__2;

		public _EB49 _003CtransferResult_003E5__3;

		public SlotView _003C_003E4__this;

		internal void _E000(int count)
		{
			_003C_003E4__this.ItemUiContext.SplitDialog.Hide();
			_003CsplitResult_003E5__2.ExecuteWithNewCount(_003C_003E4__this.InventoryController, count);
		}

		internal void _E001(int count)
		{
			_003C_003E4__this.ItemUiContext.SplitDialog.Hide();
			_003C_003E4__this.InventoryController.TryRunNetworkTransaction(_003CtransferResult_003E5__3.ExecuteWithNewCount(count, simulate: true));
		}
	}

	private static readonly WaitForEndOfFrame m__E006 = new WaitForEndOfFrame();

	[SerializeField]
	protected Image FullBorder;

	[SerializeField]
	protected GameObject SearchIcon;

	[SerializeField]
	protected Image SlotBackImage;

	[SerializeField]
	protected Image ActiveCamoraImage;

	[SerializeField]
	private RectTransform _slotPlace;

	[SerializeField]
	private Image _slotBackground;

	[SerializeField]
	private Image _emptyBorder;

	[SerializeField]
	private Image _selectedBorder;

	[SerializeField]
	private Image _selectedCorner;

	[SerializeField]
	private CustomTextMeshProUGUI _headerText;

	[CompilerGenerated]
	private Action<_EB69> m__E007;

	[CompilerGenerated]
	private Action m__E008;

	[CompilerGenerated]
	private Action m__E009;

	private static readonly Color m__E00A = Color.red;

	private static readonly Color m__E00B = Color.green;

	private static readonly Color _E00C = Color.blue;

	private static readonly Color _E00D = Color.yellow;

	internal ItemView _E00E;

	[CompilerGenerated]
	private _EB68 _E00F;

	protected IItemOwner ItemOwner;

	protected _EAED InventoryController;

	protected _E74F Skills;

	protected _ECB1 InsuranceCompany;

	protected Sprite CachedSprite;

	protected ItemUiContext ItemUiContext;

	protected bool HighlightedGlobally;

	private Color _E010;

	private Color _E011;

	private Color _E012;

	private bool _E013;

	private Action _E014;

	[CompilerGenerated]
	private Slot _E015;

	protected _EB68 ParentItemContext
	{
		[CompilerGenerated]
		get
		{
			return _E00F;
		}
		[CompilerGenerated]
		private set
		{
			_E00F = value;
		}
	}

	public ItemView ContainedItemView => _E00E;

	public Slot Slot
	{
		[CompilerGenerated]
		get
		{
			return _E015;
		}
		[CompilerGenerated]
		private set
		{
			_E015 = value;
		}
	}

	public event Action<_EB69> OnItemHover
	{
		[CompilerGenerated]
		add
		{
			Action<_EB69> action = this.m__E007;
			Action<_EB69> action2;
			do
			{
				action2 = action;
				Action<_EB69> value2 = (Action<_EB69>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E007, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<_EB69> action = this.m__E007;
			Action<_EB69> action2;
			do
			{
				action2 = action;
				Action<_EB69> value2 = (Action<_EB69>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E007, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnDragStarted
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E008;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E008, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E008;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E008, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	public event Action OnDragCancelled
	{
		[CompilerGenerated]
		add
		{
			Action action = this.m__E009;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E009, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action action = this.m__E009;
			Action action2;
			do
			{
				action2 = action;
				Action value2 = (Action)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref this.m__E009, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	protected void Start()
	{
		if (_emptyBorder != null)
		{
			_E010 = _emptyBorder.color;
		}
		if (FullBorder != null)
		{
			_E011 = FullBorder.color;
		}
		if (_selectedBorder != null)
		{
			_E012 = _selectedBorder.color;
		}
	}

	public virtual void Show(Slot slot, _EB68 parentItemContext, _EAED inventoryController, ItemUiContext itemUiContext, _E74F skills, _ECB1 insurance)
	{
		_E013 = true;
		base.gameObject.SetActive(value: true);
		Slot = slot;
		ParentItemContext = parentItemContext;
		Skills = skills;
		InsuranceCompany = insurance;
		ItemOwner = slot.ParentItem.Owner;
		InventoryController = inventoryController;
		ItemUiContext = itemUiContext;
		HighlightedGlobally = false;
		if (_headerText != null)
		{
			_headerText.text = slot.ID.Localized().ToUpper();
		}
		base.name = slot.ID + _ED3E._E000(237015);
		ParentItemContext.OnInventoryError += _E002;
		ParentItemContext.OnCheckAccept += _E001;
		_E014 = delegate
		{
			ParentItemContext.OnCheckAccept -= _E001;
			ParentItemContext.OnInventoryError -= _E002;
		};
		if (Slot.ContainedItem != null)
		{
			_slotPlace.gameObject.SetActive(!Slot.ContainedItem.NotShownInSlot);
			if (!Slot.ContainedItem.NotShownInSlot)
			{
				_E004(Slot.ContainedItem);
			}
		}
		else
		{
			SetSlotGraphics(fullSlot: false, selected: false);
		}
		if (ItemOwner == null)
		{
			return;
		}
		ItemOwner.RegisterView(this);
		if (InventoryController != ItemOwner)
		{
			InventoryController?.RegisterView(this);
		}
		foreach (_EAF1 item in ItemOwner.SelectEvents(null))
		{
			if (item == null)
			{
				continue;
			}
			if (!(item is _EAF2 obj))
			{
				if (!(item is _EAF3 obj2))
				{
					if (!(item is _EB01 obj3))
					{
						if (!(item is _EAF6 obj4))
						{
							if (!(item is _EAF7 obj5))
							{
								if (!(item is _EAF8 obj6))
								{
									if (item is _EB09 obj7 && !(_E00E == null))
									{
										_EB09 obj8 = obj7;
										if (obj8.Item == _E00E.Item)
										{
											_E00E.SetLoadAmmoStatus(obj8);
										}
									}
								}
								else
								{
									_EAF8 obj9 = obj6;
									if (_E00E != null && obj9.FromItem == _E00E.Item)
									{
										_E00E.SetUnloadMagazineStatus(obj9);
									}
								}
							}
							else
							{
								if (_E00E == null)
								{
									continue;
								}
								_EAF7 obj10 = obj5;
								if (obj10.TargetItem == _E00E.Item)
								{
									_E00E.SetLoadMagazineStatus(obj10);
									continue;
								}
								_EAF7 obj11 = obj5;
								if (obj11.Item == _E00E.Item)
								{
									_E00E.SetLoadAmmoStatus(obj11);
								}
							}
						}
						else
						{
							_EAF6 obj12 = obj4;
							if (_E00E != null && obj12.Item == _E00E.Item)
							{
								_E00E.SetBeingExaminedState(obj12);
							}
						}
					}
					else
					{
						_EB01 obj13 = obj3;
						if (_E00E != null && obj13.Item == _E00E.Item)
						{
							_E00E.IsBeingDrained.Value = true;
						}
					}
				}
				else if (obj2.From.Container == Slot)
				{
					_EAF3 obj14 = obj2;
					if (_E00E == null)
					{
						_E004(obj14.Item);
					}
					_E00E.IsBeingRemoved.Value = true;
				}
			}
			else if (obj.To.Container == Slot)
			{
				_EAF2 obj15 = obj;
				if (_E00E == null)
				{
					_E004(obj15.Item, obj15.To.GetOwnerOrNull());
				}
				_E00E.IsBeingAdded.Value = true;
			}
		}
	}

	protected virtual ItemView CreateItemViewKernel(Item item, IItemOwner itemOwner)
	{
		return ItemUiContext.CreateSlotItemView(item, ParentItemContext, InventoryController, itemOwner, Skills);
	}

	protected void SetSlotBackImage([CanBeNull] Sprite sprite)
	{
		CachedSprite = sprite;
		SlotBackImage.sprite = sprite;
		SlotBackImage.SetNativeSize();
	}

	public void OnItemAdded(_EAF2 eventArgs)
	{
		if (eventArgs.To is _EB20 obj && obj.Slot == Slot)
		{
			OnAddToSlot(eventArgs.Item, eventArgs);
		}
	}

	public void OnItemRemoved(_EAF3 eventArgs)
	{
		if (eventArgs.From is _EB20 obj && obj.Slot == Slot)
		{
			OnRemoveFromSlot(eventArgs.Item, eventArgs);
		}
	}

	protected virtual void SetupItemView(Item item)
	{
		StartCoroutine(_E003());
		ScaleItem(_slotPlace.rect.size);
		SetSlotGraphics(fullSlot: true, InventoryController != null && InventoryController.ItemInHands == item);
	}

	private void _E000()
	{
		if (_E00E is SlotItemView slotItemView)
		{
			slotItemView.SetupAddress();
		}
	}

	private void _E001(_EB69 dragItemContext)
	{
		if (Slot == null)
		{
			return;
		}
		if (dragItemContext == null)
		{
			if (HighlightedGlobally)
			{
				HighlightedGlobally = false;
				_E007();
			}
		}
		else if (Slot.ContainedItem == null)
		{
			HighlightItemViewPosition(dragItemContext, null, preview: true);
		}
	}

	private void _E002(InventoryError error)
	{
		List<Slot> list = ((error is Slot._E002 obj) ? new List<Slot> { obj.ConflictingSlot } : (error as Slot._E001)?.Slot.BlockerSlots);
		if (!(_E00E == null))
		{
			bool flag = list?.Contains(Slot) ?? false;
			if (_E00E.IsConflicting != flag)
			{
				_E00E.IsConflicting = flag;
				_E00E.UpdateInfo();
			}
		}
	}

	private IEnumerator _E003()
	{
		yield return SlotView.m__E006;
		if (!(this == null))
		{
			ScaleItem(_slotPlace.rect.size);
		}
	}

	private void _E004(Item item, IItemOwner itemOwner = null)
	{
		if (_E00E != null)
		{
			Debug.LogWarning(_ED3E._E000(237005));
			_E00E.Kill();
		}
		_E00E = CreateItemViewKernel(item, itemOwner);
		_E00E.Container = this;
		_E00E.transform.SetParent(_slotPlace);
		if (SearchIcon != null)
		{
			SearchIcon.transform.SetAsLastSibling();
		}
		_E00E.UpdateRemoveError();
		RectTransform obj = (RectTransform)_E00E.transform;
		obj.localPosition = Vector3.zero;
		obj.localScale = Vector3.one;
		obj.anchorMin = new Vector2(0f, 0f);
		obj.anchorMax = new Vector2(1f, 1f);
		obj.anchoredPosition = Vector2.zero;
		obj.pivot = Vector2.zero;
		obj.anchoredPosition = Vector2.zero;
		obj.sizeDelta = Vector2.zero;
		SetupItemView(item);
	}

	protected virtual void OnAddToSlot(Item item, _EAF2 args)
	{
		switch (args.Status)
		{
		case CommandStatus.Begin:
			_E004(item, args.To.GetOwnerOrNull());
			_E00E.IsBeingAdded.Value = true;
			break;
		case CommandStatus.Succeed:
			_E00E.IsBeingAdded.Value = false;
			_E000();
			break;
		case CommandStatus.Failed:
			_E005();
			break;
		}
	}

	private void _E005()
	{
		if (_E00E != null)
		{
			_E00E.Kill();
			_E00E = null;
		}
		SetSlotGraphics(fullSlot: false, selected: false);
	}

	protected virtual void OnRemoveFromSlot(Item item, _EAF3 args)
	{
		switch (args.Status)
		{
		case CommandStatus.Begin:
			if (_E00E != null)
			{
				_E00E.IsBeingRemoved.Value = true;
			}
			break;
		case CommandStatus.Succeed:
			_E005();
			break;
		case CommandStatus.Failed:
			_E00E.IsBeingRemoved.Value = false;
			DragCancelled();
			break;
		}
	}

	public void OnSetInHands(_EAFA eventArgs)
	{
		if (eventArgs.Item == Slot.ContainedItem && eventArgs.Status == CommandStatus.Succeed)
		{
			SetSlotGraphics(fullSlot: true, selected: true);
		}
	}

	public void OnRemoveFromHands(_EAFB eventArgs)
	{
		if (eventArgs.Item == Slot.ContainedItem && eventArgs.Status == CommandStatus.Succeed)
		{
			SetSlotGraphics(fullSlot: true, selected: false);
		}
	}

	protected void ScaleItem(Vector2 size)
	{
		if (_E00E != null)
		{
			_E00E.IconScale = size;
		}
	}

	protected void SetSlotGraphics(bool fullSlot, bool selected)
	{
		if (_emptyBorder != null && FullBorder != null)
		{
			_emptyBorder.gameObject.SetActive(!fullSlot);
			FullBorder.gameObject.SetActive(fullSlot && !selected);
			if (_selectedBorder != null)
			{
				_selectedBorder.gameObject.SetActive(fullSlot && selected);
			}
			if (_selectedCorner != null)
			{
				_selectedCorner.gameObject.SetActive(fullSlot && selected);
			}
		}
		if (_slotBackground != null)
		{
			_slotBackground.gameObject.SetActive(!fullSlot);
		}
		if (SlotBackImage != null)
		{
			SlotBackImage.gameObject.SetActive(!fullSlot);
		}
	}

	public bool CanAccept(_EB69 itemContext, _EB68 targetItemContext, out _ECD7 operation)
	{
		return itemContext.CanAccept(Slot, InventoryController, out operation);
	}

	public void HighlightItemViewPosition(_EB69 itemContext, _EB68 targetItemContext, bool preview)
	{
		if (Slot == null || (preview && _E00E != null && !_E00E.ItemContext.IsPreviewHighlightAvailable))
		{
			return;
		}
		_ECD7 operation;
		bool flag = itemContext.CanAccept(Slot, InventoryController, out operation) || (preview && operation.Error is _EA05);
		if (preview && !flag)
		{
			return;
		}
		InventoryError inventoryError = operation.Error as InventoryError;
		Color color = (flag ? SlotView.m__E00B : _E006(operation));
		if (!preview)
		{
			this.m__E007?.Invoke(itemContext);
			if (inventoryError != null)
			{
				ParentItemContext.InventoryError = inventoryError;
			}
			InventoryError inventoryError2 = inventoryError;
			if (inventoryError2 != null)
			{
				if (!(inventoryError2 is _E9FE) && !(inventoryError2 is _EA02) && !(inventoryError2 is _EB29._E001) && !(inventoryError2 is _E9FB))
				{
					if (!(inventoryError2 is _EB29._E000 obj))
					{
						inventoryError2 = inventoryError2;
						InventoryError error = inventoryError2;
						ItemUiContext.Tooltip.ShowInventoryError(error);
					}
					else
					{
						_EB29._E000 obj2 = obj;
						if (!InventoryController.HasKnownMalfunction(obj2.Weapon))
						{
							InventoryController.ExamineMalfunction(obj2.Weapon);
						}
						ItemUiContext.Tooltip.ShowInventoryError(obj2);
					}
				}
				else if (ItemUiContext.Tooltip.isActiveAndEnabled)
				{
					ItemUiContext.Tooltip.Close();
				}
			}
		}
		if (operation.Succeeded && operation.Value is _EB30 obj3 && obj3.ItemsDestroyRequired)
		{
			ItemUiContext.Tooltip.ShowWarning(new _EA0B(itemContext.Item, obj3.ItemsToDestroy));
		}
		if (_emptyBorder != null)
		{
			_emptyBorder.color = color;
		}
		if (FullBorder != null)
		{
			FullBorder.color = color;
		}
		if (_selectedBorder != null)
		{
			_selectedBorder.color = color;
		}
		if (preview)
		{
			HighlightedGlobally = true;
		}
		else
		{
			if (!(itemContext.Item is _EA12 item))
			{
				return;
			}
			if (Slot.CanAccept(item))
			{
				if (Slot.ID.StartsWith(_ED3E._E000(155651)) && Slot.ParentItem is Weapon weapon && !InventoryController.CheckedChamber(weapon))
				{
					InventoryController.CheckChamber(weapon, status: true);
				}
			}
			else if (Slot.ID.Contains(_ED3E._E000(237100)) && Slot.ContainedItem is Weapon weapon2 && weapon2.HasChambers && weapon2.Chambers[0].CanAccept(item) && !InventoryController.CheckedChamber(weapon2))
			{
				InventoryController.CheckChamber(weapon2, status: true);
			}
		}
	}

	private static Color _E006(_ECD7 possibleAction)
	{
		if (possibleAction.Failed)
		{
			return SlotView.m__E00A;
		}
		if (possibleAction.Value is _EB3B || possibleAction.Value is _EB4B)
		{
			return SlotView.m__E00B;
		}
		if (possibleAction.Value is _EB3D || possibleAction.Value is _EB47 || possibleAction.Value is _EB49)
		{
			return _E00D;
		}
		if (possibleAction.Value is _EB4A)
		{
			return _E00C;
		}
		return SlotView.m__E00A;
	}

	private void _E007()
	{
		if (ItemUiContext.Tooltip.isActiveAndEnabled)
		{
			ItemUiContext.Tooltip.Close();
		}
		if (!HighlightedGlobally)
		{
			ParentItemContext.InventoryError = null;
			if (_emptyBorder != null)
			{
				_emptyBorder.color = _E010;
			}
			if (FullBorder != null)
			{
				FullBorder.color = _E011;
			}
			if (_selectedBorder != null)
			{
				_selectedBorder.color = _E012;
			}
		}
	}

	public void DisableHighlight()
	{
		this.m__E007?.Invoke(null);
		_E007();
	}

	public virtual void DragStarted()
	{
		this.m__E008?.Invoke();
	}

	public virtual void DragCancelled()
	{
		this.m__E009?.Invoke();
	}

	public virtual async Task AcceptItem(_EB69 itemContext, _EB68 targetItemContext)
	{
		_ECD7 operation;
		bool num = itemContext.CanAccept(Slot, InventoryController, out operation);
		itemContext.DragCancelled();
		if (!num || !InventoryController.CanExecute(operation.Value) || !(await operation.Value.TryShowDestroyItemsDialog()))
		{
			return;
		}
		Item item = itemContext.Item;
		bool flag = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		if (item is _EA12 obj && Slot.ContainedItem is Weapon weapon && weapon.SupportsInternalReload)
		{
			_EA6A currentMagazine = weapon.GetCurrentMagazine();
			int num2 = ((currentMagazine != null) ? (currentMagazine.MaxCount - currentMagazine.Count) : 0);
			int num3 = ((obj.StackObjectsCount > num2) ? num2 : obj.StackObjectsCount);
			if (num3 > 0)
			{
				InventoryController.LoadWeaponWithAmmo(weapon, obj, num3).HandleExceptions();
				return;
			}
		}
		if (item is _EA12 obj2 && Slot.ContainedItem is Weapon weapon2 && weapon2.IsMultiBarrel)
		{
			int freeChamberSlotsCount = weapon2.FreeChamberSlotsCount;
			int num4 = ((obj2.StackObjectsCount > freeChamberSlotsCount) ? freeChamberSlotsCount : obj2.StackObjectsCount);
			if (num4 > 0)
			{
				InventoryController.LoadMultiBarrelWeapon(weapon2, obj2, num4).HandleExceptions();
				return;
			}
		}
		_EB2D value = operation.Value;
		if (value == null)
		{
			goto IL_0361;
		}
		if (!(value is _EB47 obj3))
		{
			if (!(value is _EB49 obj4))
			{
				goto IL_0361;
			}
			_EB49 obj5 = obj4;
			itemContext.DragCancelled();
			if (obj5.Count > 1 && flag)
			{
				ItemUiContext.SplitDialog.Show(_ED3E._E000(197696).Localized(), obj5.Count, itemContext.CursorPosition, delegate(int count)
				{
					ItemUiContext.SplitDialog.Hide();
					InventoryController.TryRunNetworkTransaction(obj5.ExecuteWithNewCount(count, simulate: true));
				}, delegate
				{
					ItemUiContext.SplitDialog.Hide();
				});
			}
			else
			{
				InventoryController.RunNetworkTransaction(obj5, _E008);
			}
		}
		else
		{
			_EB47 obj6 = obj3;
			itemContext.DragCancelled();
			if (obj6.Count > 1 && flag)
			{
				ItemUiContext.SplitDialog.Show(_ED3E._E000(197670).Localized(), obj6.Count, itemContext.CursorPosition, delegate(int count)
				{
					ItemUiContext.SplitDialog.Hide();
					obj6.ExecuteWithNewCount(InventoryController, count);
				}, delegate
				{
					ItemUiContext.SplitDialog.Hide();
				});
			}
			else
			{
				InventoryController.RunNetworkTransaction(obj6, _E008);
			}
		}
		goto IL_0383;
		IL_0383:
		if (item is Mod)
		{
			if (item is _EA6A)
			{
				Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.MenuInstallMag);
				return;
			}
			EModClass modClass = ItemViewFactory.GetModClass(item.GetType());
			if (Singleton<GUISounds>.Instance.PlayItemSound(modClass))
			{
				return;
			}
		}
		if (operation.Value is _EB3B obj7 && obj7.To is _EB20 obj8 && obj8.Slot.ParentItem is _EB0B)
		{
			Singleton<GUISounds>.Instance.PlayItemSound(item.ItemSound, EInventorySoundType.use);
		}
		else
		{
			Singleton<GUISounds>.Instance.PlayItemSound(item.ItemSound, EInventorySoundType.drop);
		}
		return;
		IL_0361:
		InventoryController.RunNetworkTransaction(operation.Value, _E008);
		goto IL_0383;
	}

	public bool CanDrag(_EB68 itemContext)
	{
		if (itemContext.DragAvailable && ItemOwner != null && !ItemOwner.Locked)
		{
			return itemContext.Item.CheckAction(null);
		}
		return false;
	}

	public virtual void Hide()
	{
		if (_E013)
		{
			_E007();
			_E005();
			ItemOwner?.UnregisterView(this);
			if (InventoryController != ItemOwner)
			{
				InventoryController?.UnregisterView(this);
			}
			_E014?.Invoke();
			if (ItemUiContext.Tooltip.isActiveAndEnabled)
			{
				ItemUiContext.Tooltip.Close();
			}
			base.gameObject.SetActive(value: false);
			_E013 = false;
			InventoryController = null;
			_E014 = null;
			ParentItemContext = null;
			InsuranceCompany = null;
			ItemUiContext = null;
			ItemOwner = null;
			Skills = null;
			Slot = null;
		}
	}

	private static void _E008(IResult result)
	{
		if (result.Failed && result.ErrorCode == 1)
		{
			_E857.DisplayWarningNotification(_ED3E._E000(213804).Localized());
		}
	}

	[CompilerGenerated]
	private void _E009()
	{
		ParentItemContext.OnCheckAccept -= _E001;
		ParentItemContext.OnInventoryError -= _E002;
	}

	[CompilerGenerated]
	private void _E00A()
	{
		ItemUiContext.SplitDialog.Hide();
	}

	[CompilerGenerated]
	private void _E00B()
	{
		ItemUiContext.SplitDialog.Hide();
	}
}
