using System;
using EFT.UI.Ragfair;
using UnityEngine;

namespace EFT.UI;

public sealed class OpenBuildWindow : BaseUiWindow
{
	[SerializeField]
	private DefaultUIButton _openButton;

	[SerializeField]
	private BuildsCategoriesPanel _categoriesPanel;

	private _ECAD _E219;

	private NodeBaseView _E21A;

	private Action<_ECAC> _E21B;

	private string _E21C;

	protected override void Awake()
	{
		_openButton.OnClick.AddListener(_E000);
		base.Awake();
	}

	public void Show(_ECBD ragfair, _EBA8 handbook, _ECAD storage, Action<_ECAC> onBuildSelected)
	{
		_E219 = storage;
		_E21B = onBuildSelected;
		ShowGameObject();
		_EBAC weaponNodesWithParent = handbook.WeaponNodesWithParent;
		_EBAC filteredNodes = new _EBAC(weaponNodesWithParent);
		_openButton.Interactable = false;
		_categoriesPanel.Show(ragfair, handbook, weaponNodesWithParent, filteredNodes, null, EViewListType.WeaponBuild, EWindowType.OpenBuild, _E002, _E001).HandleExceptions();
	}

	public override void Close()
	{
		_categoriesPanel.Close();
		base.Close();
	}

	private void _E000()
	{
		_E21B?.Invoke(_E219[_E21C]);
	}

	private void _E001(NodeBaseView view, string buildId)
	{
		_E21C = buildId;
		_E21A = view;
		_E000();
	}

	private void _E002(NodeBaseView view, string buildId)
	{
		_E21C = buildId;
		if (_E21A != null)
		{
			_E21A.DeselectView();
		}
		_E21A = view;
		_openButton.Interactable = _E21A != null;
	}
}
