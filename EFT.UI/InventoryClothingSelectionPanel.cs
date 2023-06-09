using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace EFT.UI;

public class InventoryClothingSelectionPanel : UIElement
{
	[CompilerGenerated]
	private sealed class _E000
	{
		public InventoryClothingSelectionPanel _003C_003E4__this;

		public List<_EBE6> suites;

		internal void _E000(int arg)
		{
			_003C_003E4__this._E166?.Invoke(suites[arg]);
		}
	}

	[SerializeField]
	private DropDownBox _upperButtonDropDown;

	[SerializeField]
	private DropDownBox _lowerButtonDropDown;

	private Action<_EBE6> _E166;

	public void Show(List<_EBE6> upperSuites, _EBE6 selectedUpperSuite, List<_EBE6> lowerSuites, _EBE6 selectedLowerSuite, Action<_EBE6> onSuiteSelected)
	{
		ShowGameObject();
		_E166 = onSuiteSelected;
		_E000(_upperButtonDropDown, upperSuites, selectedUpperSuite);
		_E000(_lowerButtonDropDown, lowerSuites, selectedLowerSuite);
	}

	private void _E000(DropDownBox dropdownBox, List<_EBE6> suites, _EBE6 currentSuite)
	{
		dropdownBox.Show(suites.Select((_EBE6 x) => x.NameLocalizationKey.Localized()));
		dropdownBox.UpdateValue(Mathf.Max(suites.IndexOf(currentSuite), 0));
		dropdownBox.Bind(delegate(int arg)
		{
			_E166?.Invoke(suites[arg]);
		});
	}

	public override void Close()
	{
		_upperButtonDropDown.Hide();
		_lowerButtonDropDown.Hide();
		base.Close();
	}
}
