using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI.Ragfair;

public abstract class RequirementAuxiliaryPanel : UIElement, IPointerClickHandler, IEventSystemHandler
{
	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	private CanvasGroup _childrenCanvasGroup;

	[SerializeField]
	protected TMP_InputField ItemQuantity;

	[CompilerGenerated]
	private ERequirementType _E384;

	[CompilerGenerated]
	private int _E385;

	protected Action<RequirementAuxiliaryPanel> OnPanelSelected;

	public ERequirementType Type
	{
		[CompilerGenerated]
		get
		{
			return _E384;
		}
		[CompilerGenerated]
		private set
		{
			_E384 = value;
		}
	}

	public abstract HandoverRequirement Requirement { get; }

	public abstract bool ShowAddButton { get; }

	[CanBeNull]
	public abstract _EBAB HandbookNode { get; }

	protected abstract int MaxQuantityValue { get; }

	protected int Quantity
	{
		[CompilerGenerated]
		get
		{
			return _E385;
		}
		[CompilerGenerated]
		set
		{
			_E385 = value;
		}
	}

	protected virtual void Awake()
	{
		ItemQuantity.onEndEdit.AddListener(delegate
		{
			if (int.TryParse(ItemQuantity.text, out var result) && result != Quantity)
			{
				Quantity = Mathf.Clamp(result, 1, MaxQuantityValue);
				ItemQuantity.text = Quantity.ToString();
			}
		});
	}

	public virtual void Show(_ECBD ragfair, SimpleContextMenu contextMenu, ERequirementType type, _EBA8 handbook, Action<RequirementAuxiliaryPanel> onPanelSelected)
	{
		Type = type;
		OnPanelSelected = onPanelSelected;
		ShowGameObject();
		Deselect();
	}

	public void OnPointerClick([CanBeNull] PointerEventData eventData)
	{
		OnPanelSelected(this);
	}

	public void Select()
	{
		_canvasGroup.SetUnlockStatus(value: true, setRaycast: false);
		_childrenCanvasGroup.blocksRaycasts = true;
	}

	public void Deselect()
	{
		_canvasGroup.SetUnlockStatus(value: false, setRaycast: false);
		_childrenCanvasGroup.blocksRaycasts = false;
	}

	public override void Close()
	{
		ItemQuantity.text = _ED3E._E000(27343);
		base.Close();
	}

	[CompilerGenerated]
	private void _E000(string arg)
	{
		if (int.TryParse(ItemQuantity.text, out var result) && result != Quantity)
		{
			Quantity = Mathf.Clamp(result, 1, MaxQuantityValue);
			ItemQuantity.text = Quantity.ToString();
		}
	}
}
