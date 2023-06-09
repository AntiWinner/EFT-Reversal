using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.InputSystem;
using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class AmmoSelector : UIInputNode
{
	[SerializeField]
	private GameObject _cancelView;

	[SerializeField]
	private Image _cancelBackground;

	private Color _E06D;

	private readonly Vector2 _E06E = new Vector2(-37f, -54f);

	private readonly List<GridItemView> _E06F = new List<GridItemView>();

	private readonly List<Item> _E070 = new List<Item>();

	private int _E071;

	private Action _E072;

	[CompilerGenerated]
	private bool _E073;

	public bool IsShown
	{
		[CompilerGenerated]
		get
		{
			return _E073;
		}
		[CompilerGenerated]
		set
		{
			_E073 = value;
		}
	}

	private int _E000
	{
		get
		{
			return _E071;
		}
		set
		{
			if (value != _E071)
			{
				_E000(_E071, value);
				_E071 = value;
			}
		}
	}

	private void _E000(int prevSelectionIndex, int currentSelectionIndex)
	{
		_E001(prevSelectionIndex, isSelected: false);
		_E001(currentSelectionIndex, isSelected: true);
	}

	private void _E001(int index, bool isSelected)
	{
		if (index < _E06F.Count)
		{
			_E06F[index].Highlight(isSelected);
		}
		else if (index == _E06F.Count)
		{
			if (isSelected)
			{
				_E06D = _cancelBackground.color;
			}
			_cancelBackground.color = (isSelected ? ItemView.DefaultSelectedColor : _E06D);
		}
	}

	private void _E002()
	{
		int num = _E06F.Count + 1;
		this._E000 = (this._E000 + 1) % num;
	}

	private void _E003()
	{
		int num = _E06F.Count + 1;
		this._E000 = (this._E000 - 1 + num) % num;
	}

	public void ShowAcceptableMags(List<Item> foundMags, _EAED inventoryController, Vector2 screenPosition, _ECB1 insurance, Action exitAction)
	{
		ShowGameObject();
		_E072 = exitAction;
		((RectTransform)base.transform).anchoredPosition = screenPosition + _E06E;
		foreach (Item foundMag in foundMags)
		{
			GridItemView view = GridItemView.Create(foundMag, new _EB62(), ItemRotation.Horizontal, inventoryController, inventoryController, null, null, null, insurance, isSearched: true);
			_E004(view, foundMag);
		}
		_cancelView.gameObject.SetActive(value: true);
		_cancelView.transform.SetAsLastSibling();
		IsShown = true;
		_E071 = 0;
		_E000(_E071, 0);
	}

	private void _E004(GridItemView view, Item magazine)
	{
		LayoutElement layoutElement = view.gameObject.AddComponent<LayoutElement>();
		Vector2 sizeDelta = ((RectTransform)view.transform).sizeDelta;
		layoutElement.preferredWidth = sizeDelta.x;
		layoutElement.preferredHeight = sizeDelta.y;
		view.transform.SetParent(base.transform, worldPositionStays: false);
		view.transform.localScale = Vector3.one;
		_E06F.Add(view);
		_E070.Add(magazine);
	}

	private void _E005(int index)
	{
		GridItemView gridItemView = _E06F[index];
		gridItemView.transform.SetParent(null);
		UnityEngine.Object.Destroy(gridItemView.gameObject.GetComponent<LayoutElement>());
		gridItemView.Kill();
		_E06F.RemoveAt(index);
		_E070.RemoveAt(index);
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (command.IsCommand(ECommand.NextMagazine))
		{
			_E003();
			return ETranslateResult.Block;
		}
		if (command.IsCommand(ECommand.PreviousMagazine))
		{
			_E002();
			return ETranslateResult.Block;
		}
		if (command.IsCommand(ECommand.ReloadWeapon))
		{
			_E072();
			return ETranslateResult.Block;
		}
		return ETranslateResult.Ignore;
	}

	protected override void TranslateAxes(ref float[] axes)
	{
	}

	protected override ECursorResult ShouldLockCursor()
	{
		return ECursorResult.Ignore;
	}

	[CanBeNull]
	public Item GetSelectedMagazine()
	{
		if (_E071 >= _E070.Count)
		{
			return null;
		}
		return _E070[_E071];
	}

	public void Hide()
	{
		_E001(_E06F.Count, isSelected: false);
		for (int num = _E06F.Count - 1; num >= 0; num--)
		{
			_E001(num, isSelected: false);
			_E005(num);
		}
		_cancelView.gameObject.SetActive(value: false);
		IsShown = false;
		base.gameObject.SetActive(value: false);
	}
}
