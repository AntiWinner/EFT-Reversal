using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Comfort.Common;
using Diz.Binding;
using EFT.AssetsManager;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public abstract class ItemView : AssetPoolObject, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public ItemAddress address;

		internal bool _E000(KeyValuePair<EBoundItem, Slot> pair)
		{
			return pair.Value == ((_EB20)address).Slot;
		}
	}

	[CompilerGenerated]
	private new sealed class _E001
	{
		public Item item;

		internal bool _E000(KeyValuePair<EBoundItem, Item> pair)
		{
			return pair.Value == item;
		}
	}

	protected const int HIGHLIGHT_ALPHA = 50;

	private const int _E038 = 77;

	private const float _E039 = 0.3f;

	[SerializeField]
	protected ItemViewAnimation Animator;

	[SerializeField]
	protected Image MainImage;

	[SerializeField]
	protected Image ColorPanel;

	[SerializeField]
	private Image _border;

	[SerializeField]
	private GameObject _drainLoader;

	[SerializeField]
	private GameObject _iconLoader;

	[SerializeField]
	protected CanvasGroup CanvasGroup;

	[SerializeField]
	private QuestItemViewPanel _questsItemViewPanel;

	[SerializeField]
	protected GameObject InsuredItemBorder;

	[SerializeField]
	protected GameObject InsuredIcon;

	[SerializeField]
	protected Image RepairBuffIcon;

	[SerializeField]
	protected HoverTrigger HoverTrigger;

	[SerializeField]
	private float _mainImageAlpha;

	public static readonly Color DefaultSelectedColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 77);

	protected Color SelectedColor = DefaultSelectedColor;

	[CompilerGenerated]
	private readonly _ECF5<bool> _E03A = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<bool> _E03B = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<bool> _E03C = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<bool> _E03D = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<bool> _E03E = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<bool> _E03F = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<bool> _E040 = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<bool> _E041 = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<bool> _E042 = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<bool> _E043 = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<bool> _E044 = new _ECF5<bool>();

	[CompilerGenerated]
	private readonly _ECF5<_ECD1> _E045 = new _ECF5<_ECD1>();

	public _EC9E Container;

	private bool _E046 = true;

	public bool IsConflicting;

	protected ItemUiContext ItemUiContext;

	private IBindable<bool> _E047;

	private IBindable<bool> _E048;

	protected IBindable<bool> IsInteractive;

	private bool _E049;

	protected bool HighlightedGlobally;

	protected bool IsKilled;

	protected ItemRotation _itemRotation;

	protected Color OriginalBackgroundColor;

	protected Color BackgroundColor;

	protected _EB1E ItemController;

	protected IItemOwner ItemOwner;

	protected bool IsTeammateDogtag;

	private float _E04A;

	private double _E04B;

	private RectTransform _E04C;

	protected readonly _E3A4 CompositeDisposable = new _E3A4();

	private _E3E2 _E04D;

	private Vector2? _E04E;

	private Action _E04F;

	private PointerEventData _E050;

	private Color? _E051;

	[CompilerGenerated]
	private _EB68 _E052;

	[CompilerGenerated]
	private Item _E053;

	[CompilerGenerated]
	private DraggedItemView _E054;

	protected _ECF5<bool> IsBeingDragged
	{
		[CompilerGenerated]
		get
		{
			return _E03A;
		}
	}

	public _ECF5<bool> IsBeingRemoved
	{
		[CompilerGenerated]
		get
		{
			return _E03B;
		}
	}

	public _ECF5<bool> IsBeingAdded
	{
		[CompilerGenerated]
		get
		{
			return _E03C;
		}
	}

	public _ECF5<bool> IsBeingDrained
	{
		[CompilerGenerated]
		get
		{
			return _E03D;
		}
	}

	public _ECF5<bool> IsFilteredOut
	{
		[CompilerGenerated]
		get
		{
			return _E03E;
		}
	}

	public _ECF5<bool> IsDisabledDrag
	{
		[CompilerGenerated]
		get
		{
			return _E03F;
		}
	}

	public _ECF5<bool> IsBeingSearched
	{
		[CompilerGenerated]
		get
		{
			return _E040;
		}
	}

	public _ECF5<bool> IsBeingExamined
	{
		[CompilerGenerated]
		get
		{
			return _E041;
		}
	}

	public _ECF5<bool> IsBeingLoadedMagazine
	{
		[CompilerGenerated]
		get
		{
			return _E042;
		}
	}

	public _ECF5<bool> IsBeingUnloadedMagazine
	{
		[CompilerGenerated]
		get
		{
			return _E043;
		}
	}

	public _ECF5<bool> IsBeingLoadedAmmo
	{
		[CompilerGenerated]
		get
		{
			return _E044;
		}
	}

	public _ECF5<_ECD1> RemoveError
	{
		[CompilerGenerated]
		get
		{
			return _E045;
		}
	}

	public _EB68 ItemContext
	{
		[CompilerGenerated]
		get
		{
			return _E052;
		}
		[CompilerGenerated]
		private set
		{
			_E052 = value;
		}
	}

	public bool BeingDragged => IsBeingDragged.Value;

	public Color BorderColor
	{
		set
		{
			if (!(_border == null))
			{
				if (!_E051.HasValue)
				{
					_E051 = _border.color;
				}
				_border.color = value;
			}
		}
	}

	public Vector2? IconScale
	{
		get
		{
			return _E04E;
		}
		set
		{
			_E04E = value;
			UpdateScale();
		}
	}

	public Item Item
	{
		[CompilerGenerated]
		get
		{
			return _E053;
		}
		[CompilerGenerated]
		private set
		{
			_E053 = value;
		}
	}

	public bool IsSearched
	{
		get
		{
			return _E046;
		}
		protected set
		{
			_E046 = value;
			ItemContext.Searched = _E046;
		}
	}

	protected abstract bool IsInteractable { get; }

	public virtual ItemRotation ItemRotation
	{
		get
		{
			return _itemRotation;
		}
		protected set
		{
			_itemRotation = value;
			MainImage.transform.rotation = ((value == ItemRotation.Horizontal) ? ItemViewFactory.HorizontalRotation : ItemViewFactory.VerticalRotation);
			if (RectTransform.anchorMin == RectTransform.anchorMax)
			{
				RectTransform.sizeDelta = ItemViewFactory.GetCellPixelSize(Item.CalculateRotatedSize(_itemRotation));
			}
		}
	}

	protected RectTransform RectTransform
	{
		get
		{
			if (_E04C == null)
			{
				_E04C = (RectTransform)base.transform;
			}
			return _E04C;
		}
	}

	protected virtual IBindable<float> Transparency => _ECF3.Combine(IsFilteredOut, IsDisabledDrag, RemoveError, (bool filtered, bool dragDisabled, _ECD1 removeError) => (!(filtered || dragDisabled) && (removeError == null || removeError is Slot._E005)) ? 1f : 0.25f);

	protected virtual bool Examined
	{
		get
		{
			if (ItemController != null)
			{
				return ItemController.Examined(Item);
			}
			return true;
		}
	}

	[CanBeNull]
	protected DraggedItemView DraggedItemView
	{
		[CompilerGenerated]
		get
		{
			return _E054;
		}
		[CompilerGenerated]
		private set
		{
			_E054 = value;
		}
	}

	protected _EC4E<EItemInfoButton> NewContextInteractions => ItemUiContext.GetItemContextInteractions(ItemContext, null);

	protected void NewItemView(Item item, _EB68 sourceItemContext, ItemRotation rotation, _EB1E itemController, _EC9E container, IItemOwner itemOwner, [CanBeNull] ItemUiContext itemUiContext)
	{
		ItemController = itemController;
		ItemOwner = itemOwner;
		ItemUiContext = itemUiContext;
		Item = item;
		Container = container;
		ItemContext = CreateNewItemContext(sourceItemContext);
		RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		IsKilled = false;
		ItemRotation = rotation;
		IsBeingExamined.Value = false;
		IsBeingLoadedMagazine.Value = false;
		IsBeingUnloadedMagazine.Value = false;
		IsBeingLoadedAmmo.Value = false;
		IsBeingSearched.Value = false;
		IsBeingDragged.Value = false;
		IsBeingDrained.Value = false;
		IsFilteredOut.Value = false;
		IsBeingRemoved.Value = false;
		IsBeingAdded.Value = false;
		HighlightedGlobally = false;
		_E049 = false;
		IconScale = null;
		ItemContext.OnUpdate += UpdateInfo;
		ItemContext.OnDragStateChange += _E000;
		ItemContext.OnCheckAccept += CheckAcceptHandler;
		CompositeDisposable.AddDisposable(delegate
		{
			ItemContext.OnUpdate -= UpdateInfo;
			ItemContext.OnDragStateChange -= _E000;
			ItemContext.OnCheckAccept -= CheckAcceptHandler;
		});
		_E048 = _ECF3.Combine(IsBeingDragged, IsBeingRemoved, (bool dragged, bool removed) => !dragged && !removed);
		_E047 = _ECF3.Combine(IsBeingAdded, IsBeingRemoved, IsBeingExamined, IsBeingLoadedAmmo, (bool added, bool removed, bool examined, bool loaded) => added || removed || examined || loaded);
		InitInteractiveBinding();
		CompositeDisposable.BindState(IsBeingDragged, Animator.SetDragState);
		CompositeDisposable.BindState(IsBeingDrained, delegate(bool drained)
		{
			_drainLoader.SetActive(drained);
		});
		CompositeDisposable.BindState(IsBeingSearched, Animator.SetSearchedState);
		CompositeDisposable.BindState(IsBeingExamined, OnBeingExaminedChanged);
		CompositeDisposable.BindState(_E048, UpdateInfoVisibility);
		CompositeDisposable.BindState(Transparency, delegate(float transparency)
		{
			CanvasGroup.alpha = transparency;
		});
		CompositeDisposable.BindState(IsInteractive, delegate(bool isInteractive)
		{
			CanvasGroup.blocksRaycasts = isInteractive;
		});
		CompositeDisposable.BindState(_E047, Animator.SetBlinkingState);
		_ = ItemUiContext == null;
	}

	protected bool CanInteract(_EB69 dragItemContext)
	{
		if (dragItemContext.Item is _EA8D obj && obj.CanRepair(Item))
		{
			_EB67 itemContext = new _EB67(ItemContext, obj);
			if (ItemUiContext.GetItemContextInteractions(itemContext, null).IsInteractionAvailable(EItemInfoButton.Repair))
			{
				return true;
			}
		}
		return false;
	}

	private void _E000(bool isBeingDragged)
	{
		IsBeingDragged.Value = isBeingDragged;
		if (!isBeingDragged)
		{
			Container.DragCancelled();
		}
	}

	protected virtual void CheckAcceptHandler(_EB69 dragItemContext)
	{
		if (Container == null || Container.Equals(null))
		{
			Container = null;
			CompositeDisposable.Dispose();
		}
	}

	protected virtual _EB68 CreateNewItemContext(_EB68 sourceContext)
	{
		return sourceContext.CreateChild(Item);
	}

	protected virtual void InitInteractiveBinding()
	{
		IsInteractive = _ECF3.Combine(IsFilteredOut, IsBeingDragged, IsBeingAdded, IsBeingDrained, IsBeingRemoved, IsBeingSearched, IsBeingLoadedAmmo, (bool filtered, bool dragged, bool added, bool drained, bool removed, bool searched, bool loadAmmo) => !filtered && !dragged && !added && !drained && !removed && !searched && !loadAmmo);
	}

	public void Rotate()
	{
		ItemRotation = (ItemRotation)((int)(_itemRotation + 1) % 2);
	}

	protected virtual void OnBeingExaminedChanged(bool isBeingExamined)
	{
	}

	public virtual void UpdateRemoveError(bool ignoreMalfunctions = true)
	{
		if (Item.CurrentAddress == null)
		{
			RemoveError.Value = null;
			return;
		}
		_ECD1 obj = _EB29.Remove(Item, ItemController, simulate: true).Error;
		if (obj is _EB29._E000 obj2 && ItemController is _EAED obj3 && !obj3.HasKnownMalfunction(obj2.Weapon))
		{
			if (ignoreMalfunctions)
			{
				obj = null;
			}
			else
			{
				obj3.ExamineMalfunction(obj2.Weapon);
			}
		}
		RemoveError.Value = obj;
	}

	protected virtual void UpdateInfoVisibility(bool isVisible)
	{
		if (_border != null)
		{
			_border.gameObject.SetActive(isVisible);
		}
		ColorPanel.gameObject.SetActive(isVisible);
	}

	protected internal void Init()
	{
		UpdateStaticInfo();
		UpdateInfo();
		_E002();
		_E003();
		SetQuestItemViewPanel();
		RegisterItemView();
		base.gameObject.SetActive(value: true);
	}

	protected virtual void RegisterItemView()
	{
		if (!(ItemUiContext == null))
		{
			ItemUiContext.RegisterView(ItemContext);
			CompositeDisposable.AddDisposable(delegate
			{
				ItemUiContext.UnregisterView(ItemContext);
			});
		}
	}

	protected void RefreshIcon()
	{
		if (_E04F != null)
		{
			_E04F();
			_E04F = null;
		}
		_E04D = ItemViewFactory.LoadItemIcon(Item);
		_E04F = _E04D.Changed.Bind(_E001);
	}

	protected virtual void UpdateStaticInfo()
	{
		RefreshIcon();
		BackgroundColor = Item.BackgroundColor.ToColor();
		BackgroundColor.a = 0.3019608f;
		UpdateColor();
		OriginalBackgroundColor = BackgroundColor;
		Animator.Init(OriginalBackgroundColor);
	}

	protected void SetQuestItemViewPanel()
	{
		if (!(_questsItemViewPanel == null) && ItemController != null && ItemController is _EAED obj)
		{
			_questsItemViewPanel.Show(obj.Profile, Item, (ItemUiContext != null) ? ItemUiContext.Tooltip : null);
		}
	}

	private void _E001()
	{
		if (!(base.gameObject == null) && !(_iconLoader == null))
		{
			_iconLoader.SetActive(_E04D.Sprite == null);
			MainImage.gameObject.SetActive(_E04D.Sprite != null && IsSearched);
			MainImage.sprite = _E04D.Sprite;
			MainImage.SetNativeSize();
			UpdateScale();
		}
	}

	public virtual void UpdateInfo()
	{
	}

	private void _E002()
	{
		if (ItemController != null)
		{
			IsTeammateDogtag = !ItemController.CanMoveDogtag(Item);
			IsDisabledDrag.Value = IsTeammateDogtag;
		}
	}

	private void _E003()
	{
		if (ItemController != null)
		{
			IsDisabledDrag.Value = !ItemController.CanMoveCompoundItem(Item);
		}
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (_E004(eventData))
		{
			UpdateRemoveError(ignoreMalfunctions: false);
		}
	}

	public virtual void OnBeginDrag([NotNull] PointerEventData eventData)
	{
		if (_E004(eventData))
		{
			Singleton<GUISounds>.Instance.PlayItemSound(Item.ItemSound, EInventorySoundType.pickup);
			IsBeingDragged.Value = true;
			DraggedItemView = DraggedItemView.Create(ItemContext, ItemRotation, Examined ? Color.white : new Color(0f, 0f, 0f, 0.85f), ItemUiContext);
			((RectTransform)DraggedItemView.transform).position = base.transform.position;
			Container.DragStarted();
		}
	}

	private bool _E004(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && Container.CanDrag(ItemContext) && IsSearched && !IsTeammateDogtag && RemoveError.Value == null)
		{
			return DraggedItemView == null;
		}
		return false;
	}

	public virtual void OnDrag([NotNull] PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && !(DraggedItemView == null))
		{
			_E050 = eventData;
			DraggedItemView.OnDrag(eventData);
			if (IsInteractable)
			{
				_EB68 itemUnderCursor = eventData.pointerEnter?.GetComponentInParent<ItemView>()?.ItemContext;
				_EC9E containerUnderCursor = eventData.pointerEnter?.GetComponentInParent<_EC9E>();
				DraggedItemView.UpdateTargetUnderCursor(containerUnderCursor, itemUnderCursor);
			}
		}
	}

	public virtual void OnEndDrag([NotNull] PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			_EB68 obj = eventData.pointerEnter?.GetComponentInParent<ItemView>()?.ItemContext;
			if (obj != null)
			{
				obj.InventoryError = null;
			}
			_E050 = null;
			if (!(DraggedItemView == null))
			{
				_EB69 itemContext = DraggedItemView.ItemContext;
				DraggedItemView.Kill();
				UnityEngine.Object.DestroyImmediate(DraggedItemView.gameObject);
				DraggedItemView = null;
				_E005(itemContext, eventData);
			}
		}
	}

	protected void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus && _E050 != null)
		{
			OnEndDrag(_E050);
		}
	}

	private void _E005(_EB69 dragItemContext, PointerEventData eventData)
	{
		_EB68 obj = ((eventData.pointerEnter == null) ? null : eventData.pointerEnter.GetComponentInParent<ItemView>()?.ItemContext);
		_EC9E obj2 = ((eventData.pointerEnter == null) ? null : eventData.pointerEnter.GetComponentInParent<_EC9E>());
		dragItemContext.DragCancelled();
		bool flag = obj2 == Container;
		if (Container is QuickSlotView quickSlotView && !flag)
		{
			quickSlotView.RemoveItemViewForced();
		}
		if (obj2 != null)
		{
			bool flag2 = true;
			if (Container is SlotView)
			{
				flag2 = !flag;
			}
			else if (Container is QuickSlotView)
			{
				flag2 = !flag && obj2 is QuickSlotView;
			}
			if (flag2 & obj2.CanAccept(dragItemContext, obj, out var _))
			{
				obj2.AcceptItem(dragItemContext, obj).HandleExceptions();
				return;
			}
		}
		if (obj != null && dragItemContext.Item is _EA8D repairKit)
		{
			_EB67 itemContext = new _EB67(obj, repairKit);
			ItemUiContext.GetItemContextInteractions(itemContext, null).ExecuteInteraction(EItemInfoButton.Repair);
		}
	}

	protected void Update()
	{
		if (_E050 != null && !Input.GetMouseButton(0))
		{
			OnEndDrag(_E050);
		}
		if (IsSearched)
		{
			if (IsBeingDrained.Value)
			{
				UpdateInfo();
			}
			if (Math.Abs(_E04A - _mainImageAlpha) > 0.01f)
			{
				_E04A = _mainImageAlpha;
				Color color = MainImage.color;
				color.a = _mainImageAlpha;
				MainImage.color = color;
			}
		}
	}

	public virtual void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		ItemUiContext.RegisterCurrentItemContext(ItemContext);
		Highlight(highlight: true);
	}

	public virtual void OnPointerExit([NotNull] PointerEventData eventData)
	{
		ItemUiContext.UnregisterCurrentItemContext(ItemContext);
		Highlight(highlight: false);
	}

	public void Highlight(bool highlight)
	{
		_E049 = highlight;
		UpdateColor();
	}

	protected void UpdateColor()
	{
		ColorPanel.color = ((_E049 || HighlightedGlobally) ? SelectedColor : BackgroundColor);
	}

	public void SetBeingExaminedState(_EAF6 activeEvent)
	{
		bool flag = activeEvent.Status == CommandStatus.Begin;
		IsBeingExamined.Value = flag;
		UpdateRemoveError();
		if (flag)
		{
			Animator.StartExamination(activeEvent.ExamineTime);
		}
		else
		{
			Animator.StopExamination();
		}
	}

	public void SetLoadMagazineStatus(_EAF7 activeEvent)
	{
		bool flag = activeEvent.Status == CommandStatus.Begin;
		IsBeingLoadedMagazine.Value = flag;
		if (flag)
		{
			Animator.StartLoading(activeEvent.LoadTime, activeEvent.LoadCount);
		}
		else
		{
			Animator.StopLoading();
		}
	}

	public void SetUnloadMagazineStatus(_EAF8 activeEvent)
	{
		bool flag = activeEvent.Status == CommandStatus.Begin;
		IsBeingUnloadedMagazine.Value = flag;
		if (flag)
		{
			Animator.StartLoading(activeEvent.UnloadTime, activeEvent.UnloadCount, activeEvent.StartCount);
		}
		else
		{
			Animator.StopLoading();
		}
	}

	protected void SetInventoryCheckMagazineStatus(float time, bool value)
	{
		IsBeingLoadedMagazine.Value = value;
		if (value)
		{
			Animator.StartLoading(time, 1);
		}
		else
		{
			Animator.StopLoading();
		}
	}

	public void SetLoadAmmoStatus(_EAF1 activeEvent)
	{
		IsBeingLoadedAmmo.Value = activeEvent.Status == CommandStatus.Begin;
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		bool doubleClick = false;
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			double utcNowUnix = _E5AD.UtcNowUnix;
			if (utcNowUnix - _E04B < 0.30000001192092896)
			{
				doubleClick = true;
				_E04B = 0.0;
			}
			else
			{
				_E04B = utcNowUnix;
			}
		}
		OnClick(eventData.button, eventData.pressPosition, doubleClick);
	}

	protected virtual void OnClick(PointerEventData.InputButton button, Vector2 position, bool doubleClick)
	{
		if (ItemUiContext == null || !IsSearched)
		{
			return;
		}
		bool flag = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
		bool flag2 = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
		bool flag3 = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		_EC4E<EItemInfoButton> newContextInteractions = NewContextInteractions;
		switch (button)
		{
		case PointerEventData.InputButton.Middle:
			if (!ExecuteMiddleClick())
			{
				newContextInteractions.ExecuteInteraction(EItemInfoButton.CheckMagazine);
			}
			break;
		case PointerEventData.InputButton.Right:
			ShowContextMenu(position);
			break;
		case PointerEventData.InputButton.Left:
		{
			if (!(flag || flag3) && doubleClick && newContextInteractions.ExecuteInteraction(Item.IsContainer ? EItemInfoButton.Open : EItemInfoButton.Inspect))
			{
				break;
			}
			SimpleTooltip tooltip = ItemUiContext.Tooltip;
			if (flag || flag3)
			{
				_ECD7 obj = (flag ? ItemUiContext.QuickFindAppropriatePlace(Item, ItemController) : ItemUiContext.QuickMoveToSortingTable(Item));
				if (obj.Failed || !ItemController.CanExecute(obj.Value))
				{
					break;
				}
				if (obj.Value is _EB30 obj2 && obj2.ItemsDestroyRequired)
				{
					_E857.DisplayWarningNotification(new _EA0B(Item, obj2.ItemsToDestroy).GetLocalizedDescription());
					break;
				}
				string itemSound = Item.ItemSound;
				ItemController.RunNetworkTransaction(obj.Value);
				if (tooltip != null)
				{
					tooltip.Close();
				}
				Singleton<GUISounds>.Instance.PlayItemSound(itemSound, EInventorySoundType.pickup);
			}
			else if (flag2)
			{
				if (newContextInteractions.IsInteractionAvailable(EItemInfoButton.Equip))
				{
					ItemUiContext.QuickEquip(Item).HandleExceptions();
				}
				if (tooltip != null)
				{
					tooltip.Close();
				}
			}
			else if (IsBeingLoadedMagazine.Value || IsBeingUnloadedMagazine.Value)
			{
				ItemController.StopProcesses();
			}
			break;
		}
		}
	}

	protected bool ExecuteMiddleClick()
	{
		if (Item.CurrentAddress == null)
		{
			return false;
		}
		_EC4E<EItemInfoButton> newContextInteractions = NewContextInteractions;
		if (!newContextInteractions.ExecuteInteraction(EItemInfoButton.Examine) && !newContextInteractions.ExecuteInteraction(EItemInfoButton.Fold))
		{
			return newContextInteractions.ExecuteInteraction(EItemInfoButton.Unfold);
		}
		return true;
	}

	protected void ShowContextMenu(Vector2 position)
	{
		ItemUiContext.ShowContextMenu(ItemContext, position);
	}

	protected virtual void UpdateScale()
	{
		if (MainImage.gameObject.activeSelf)
		{
			if (_E04E.HasValue)
			{
				Vector2 sizeDelta = MainImage.rectTransform.sizeDelta;
				float num = _E04E.Value.x / sizeDelta.x;
				float num2 = _E04E.Value.y / sizeDelta.y;
				float num3 = ((num > 1f && num2 > 1f) ? 1f : Mathf.Min(num, num2));
				MainImage.rectTransform.localScale = new Vector3(num3, num3, 1f);
			}
			else
			{
				MainImage.rectTransform.localScale = Vector3.one;
			}
		}
	}

	protected static EBoundItem? GetBindingForAddress([CanBeNull] _EAE5 inventoryOwner, ItemAddress address)
	{
		if (inventoryOwner == null)
		{
			return null;
		}
		if (!(address is _EB20))
		{
			return null;
		}
		KeyValuePair<EBoundItem, Slot> keyValuePair = inventoryOwner.FastAccess.BoundCells.FirstOrDefault((KeyValuePair<EBoundItem, Slot> pair) => pair.Value == ((_EB20)address).Slot);
		if (keyValuePair.Value != null)
		{
			return keyValuePair.Key;
		}
		if (inventoryOwner is _EB1E itemController && address.Container is Slot slot)
		{
			return GetBindingForItem(itemController, slot.ContainedItem);
		}
		return null;
	}

	protected static EBoundItem? GetBindingForItem([CanBeNull] _EB1E itemController, Item item)
	{
		if (!(itemController is _EAED obj))
		{
			return null;
		}
		var (value, item3) = obj.FastAccess.BoundItems.FirstOrDefault((KeyValuePair<EBoundItem, Item> pair) => pair.Value == item);
		if (item3 != null)
		{
			return value;
		}
		return null;
	}

	public virtual void Kill()
	{
		base.gameObject.SetActive(value: false);
		if (_E051.HasValue)
		{
			BorderColor = _E051.Value;
		}
		CompositeDisposable.Dispose();
		Animator.Stop();
		_E04F?.Invoke();
		_E04F = null;
		RemoveError.Value = null;
		if (DraggedItemView != null)
		{
			DraggedItemView.Kill();
			UnityEngine.Object.DestroyImmediate(DraggedItemView.gameObject);
			DraggedItemView = null;
		}
		if (ItemUiContext != null)
		{
			ItemUiContext.UnregisterCurrentItemContext(ItemContext);
			ItemUiContext = null;
		}
		ItemContext?.Dispose();
		ItemContext = null;
		ItemController = null;
		ItemOwner = null;
		Container = null;
		Item = null;
		ReturnToPool();
		IsKilled = true;
	}

	[CompilerGenerated]
	private void _E006()
	{
		ItemContext.OnUpdate -= UpdateInfo;
		ItemContext.OnDragStateChange -= _E000;
		ItemContext.OnCheckAccept -= CheckAcceptHandler;
	}

	[CompilerGenerated]
	private void _E007(bool drained)
	{
		_drainLoader.SetActive(drained);
	}

	[CompilerGenerated]
	private void _E008(float transparency)
	{
		CanvasGroup.alpha = transparency;
	}

	[CompilerGenerated]
	private void _E009(bool isInteractive)
	{
		CanvasGroup.blocksRaycasts = isInteractive;
	}

	[CompilerGenerated]
	private void _E00A()
	{
		ItemUiContext.UnregisterView(ItemContext);
	}
}
