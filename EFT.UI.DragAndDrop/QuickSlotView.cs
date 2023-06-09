using System;
using System.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI.DragAndDrop;

public abstract class QuickSlotView : MonoBehaviour, _EC9E
{
	[SerializeField]
	protected Image InstallPlace;

	[SerializeField]
	protected CustomTextMeshProUGUI HotKey;

	[SerializeField]
	protected CustomTextMeshProUGUI Caption;

	[SerializeField]
	protected Image Border;

	[SerializeField]
	protected Image Background;

	[SerializeField]
	private Image _arrow;

	protected _EAED InventoryController;

	protected ItemUiContext ItemUiContext;

	protected ItemView ItemView;

	protected Item Item;

	private bool _E002;

	protected virtual void SetItem(Item item, _EAED controller, ItemUiContext itemUiContext)
	{
		Item = item;
		ItemView = QuickSlotItemView.Create(item, controller, itemUiContext);
		ItemView.Container = this;
		RectTransform obj = (RectTransform)ItemView.transform;
		obj.SetParent(InstallPlace.transform);
		obj.localPosition = Vector3.zero;
		obj.localScale = Vector3.one;
		obj.sizeDelta = Vector2.zero;
		obj.anchoredPosition = Vector2.zero;
		SetIconScale();
		if (Item is Weapon weapon)
		{
			weapon.MalfState.OnStateChanged += _E000;
			_E000();
		}
	}

	public void RefreshSelection(Item itemInHands)
	{
		SwitchVisualSelection(itemInHands == Item);
	}

	public virtual void Hide()
	{
		RemoveItemView();
		InventoryController = null;
		ItemUiContext = null;
		Item = null;
		if (this != null)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	protected virtual void SetIconScale()
	{
		ItemView.IconScale = ((RectTransform)base.transform).sizeDelta;
	}

	protected void RemoveItemView()
	{
		if (Item is Weapon weapon)
		{
			weapon.MalfState.OnStateChanged -= _E000;
			_E001();
		}
		if (ItemView != null)
		{
			ItemView.Kill();
			ItemView = null;
		}
		Caption.text = string.Empty;
	}

	protected void Awake()
	{
		_E002 = InstallPlace.enabled;
	}

	protected virtual void OnDestroy()
	{
		RemoveItemView();
	}

	protected void ShowInfoPanel([CanBeNull] Item item)
	{
		if (item == null)
		{
			RemoveItemView();
			return;
		}
		Caption.gameObject.SetActive(value: true);
		Caption.text = item.ShortName.Localized();
	}

	public void HighlightItemViewPosition(_EB69 itemContext, _EB68 targetItemContext, bool preview)
	{
		_ECD7 operation;
		bool flag = CanAccept(itemContext, targetItemContext, out operation);
		if (!preview || flag)
		{
			Color color = (flag ? new Color(0f, 0.4f, 0f) : Color.red);
			Border.color = color;
		}
	}

	public void DisableHighlight()
	{
		Border.color = Color.white;
	}

	public void SwitchVisualSelection(bool selected)
	{
		if (!(Background == null))
		{
			Background.color = (selected ? Color.white : new Color(0f, 0f, 0f, 0.6f));
			Caption.color = (selected ? Color.black : Color.white);
			Caption.fontStyle = FontStyles.Normal;
		}
	}

	public Vector2 ShowArrow(bool show)
	{
		if (_arrow == null)
		{
			return Vector2.zero;
		}
		_arrow.gameObject.SetActive(show);
		return ((RectTransform)base.transform).anchoredPosition;
	}

	public void ShowQuickdrawGlow(bool show, float duration)
	{
		if (!show)
		{
			InstallPlace.DOKill(complete: true);
		}
		else if (!(Item is Weapon weapon) || !InventoryController.HasKnownMalfunction(weapon))
		{
			int loops = Mathf.CeilToInt(duration / 0.175f);
			InstallPlace.color = new Color32(84, 193, byte.MaxValue, 0);
			TweenerCore<Color, Color, ColorOptions> tweenerCore = InstallPlace.DOFade(0.3f, 0.175f).SetLoops(loops, LoopType.Yoyo);
			tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, new TweenCallback(_E000));
		}
	}

	public void DragStarted()
	{
	}

	public void DragCancelled()
	{
	}

	public virtual Task AcceptItem(_EB69 itemContext, _EB68 targetItemContext)
	{
		return Task.CompletedTask;
	}

	public abstract bool CanDrag(_EB68 itemContext);

	public virtual void RemoveItemViewForced()
	{
	}

	public abstract bool CanAccept(_EB69 itemContext, _EB68 targetItemContext, out _ECD7 operation);

	private void _E000()
	{
		if (Item is Weapon weapon && InventoryController.HasKnownMalfunction(weapon))
		{
			Border.color = Color.red;
			InstallPlace.color = new Color32(181, 0, 0, 92);
			InstallPlace.enabled = true;
		}
		else
		{
			_E001();
		}
		if (ItemView != null)
		{
			ItemView.UpdateInfo();
		}
	}

	private void _E001()
	{
		Border.color = Color.white;
		InstallPlace.color = new Color32(0, 0, 0, 52);
		InstallPlace.enabled = _E002;
	}
}
