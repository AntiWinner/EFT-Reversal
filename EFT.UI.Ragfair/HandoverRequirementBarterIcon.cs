using EFT.InventoryLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI.Ragfair;

public sealed class HandoverRequirementBarterIcon : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[SerializeField]
	private Image _barterIcon;

	[SerializeField]
	private Color _unavailableColor;

	[SerializeField]
	private Color _defaultColor;

	[SerializeField]
	private Color _selectedColor;

	private string _E37E;

	private SimpleTooltip _E02A;

	private _E8B1 _E206;

	public void Show(_E8B1 requisite, SimpleTooltip tooltip)
	{
		_E206 = requisite;
		_E02A = tooltip;
		if (_E206.IsDogtagRequired)
		{
			_E37E = _ED3E._E000(200879).Localized() + _ED3E._E000(255020) + _E206.Level + _ED3E._E000(18502) + _ED3E._E000(255032).Localized() + ((_E206.Side != EDogtagExchangeSide.Any) ? (_ED3E._E000(10270) + _E206.Side) : "").ToUpper();
		}
		else
		{
			_E37E = _E206.RequiredItem.ShortName.Localized();
		}
		if (requisite.OnlyFunctional)
		{
			_E37E = _E37E + _ED3E._E000(216105) + _ED3E._E000(242163).Localized() + _ED3E._E000(27308);
		}
		ShowGameObject();
		UpdateView();
	}

	public void UpdateView()
	{
		_barterIcon.color = (_E206.Enough ? _selectedColor : _unavailableColor);
	}

	public void OnPointerEnter([CanBeNull] PointerEventData eventData)
	{
		_E02A.Show(_E37E + _ED3E._E000(242203) + Mathf.Clamp(_E206.PreparedItemsCount, 0, _E206.RequiredItemsCount) + _ED3E._E000(124720) + _E206.RequiredItemsCount + _ED3E._E000(242193));
	}

	public void OnPointerExit([CanBeNull] PointerEventData eventData)
	{
		_E02A.Close();
	}
}
