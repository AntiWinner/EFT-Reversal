using System;
using EFT.InventoryLogic;
using UnityEngine;

namespace EFT.UI.Ragfair;

public class AuxiliaryMoneyPanel : RequirementAuxiliaryPanel
{
	[SerializeField]
	private DropDownBox _moneyDropdown;

	private _EBAC _E081;

	public override _EBAB HandbookNode => _E081[_E000];

	public override HandoverRequirement Requirement => new HandoverRequirement(_E000, base.Quantity, onlyFunctional: false);

	public override bool ShowAddButton => base.Quantity > 0;

	protected override int MaxQuantityValue => 100000000;

	private string _E000 => _EA10.GetCurrencyId((ECurrencyType)_moneyDropdown.CurrentIndex);

	public override void Show(_ECBD ragfair, SimpleContextMenu contextMenu, ERequirementType type, _EBA8 handbook, Action<RequirementAuxiliaryPanel> onPanelSelected)
	{
		base.Show(ragfair, contextMenu, type, handbook, onPanelSelected);
		_E081 = handbook.StructuredItems;
		_moneyDropdown.Show(_EA10.CurrencyNames);
		_moneyDropdown.UpdateValue(0);
		base.Quantity = 1000;
		ItemQuantity.text = base.Quantity.ToString();
		UI.AddDisposable(_moneyDropdown.Hide);
	}
}
