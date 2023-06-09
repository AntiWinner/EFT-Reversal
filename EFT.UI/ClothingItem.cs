using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Comfort.Common;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ClothingItem : UIElement, IPointerClickHandler, IEventSystemHandler
{
	public sealed class _E000
	{
		public _EBE2 Offer;

		public _EBE6 Suite;

		public _EBDF Clothing;
	}

	public sealed class _E001 : IComparer<_E000>
	{
		private readonly _E60E _E000;

		public int Compare(_E000 x, _E000 y)
		{
			if (x == null || y == null)
			{
				return -1;
			}
			bool flag = _E000.IsSuiteAvailable(x.Suite.Id);
			bool flag2 = _E000.IsSuiteAvailable(y.Suite.Id);
			if (flag && !flag2)
			{
				return -1;
			}
			if (!flag && flag2)
			{
				return 1;
			}
			int num = x.Offer.requirements.profileLevel.CompareTo(y.Offer.requirements.profileLevel);
			if (num == 0)
			{
				return string.Compare(x.Clothing.Name, y.Clothing.Name, StringComparison.Ordinal);
			}
			return num;
		}

		public _E001(_E60E solver)
		{
			_E000 = solver;
		}
	}

	[SerializeField]
	private Image _backgroud;

	[SerializeField]
	private TextMeshProUGUI _clothingName;

	[SerializeField]
	private Image _clothingIcon;

	[SerializeField]
	private Image _clothingIconBorder;

	[SerializeField]
	private GameObject _equippedPanel;

	[SerializeField]
	private GameObject _lockedPanel;

	[SerializeField]
	private ColorMap _backgroudColor;

	[SerializeField]
	private ColorMap _textColor;

	private Action<ClothingItem> _E1CF;

	private Action<ClothingItem> _E1D0;

	private bool _E1D1;

	private bool _E1D2;

	private _EBEA _E1D3;

	[CompilerGenerated]
	private _E000 _E1D4;

	[CompilerGenerated]
	private bool _E1D5;

	public _E000 Offer
	{
		[CompilerGenerated]
		get
		{
			return _E1D4;
		}
		[CompilerGenerated]
		private set
		{
			_E1D4 = value;
		}
	}

	public bool IsItemLocked
	{
		[CompilerGenerated]
		get
		{
			return _E1D5;
		}
		[CompilerGenerated]
		private set
		{
			_E1D5 = value;
		}
	}

	public void Init(Action<ClothingItem> onEquip, Action<ClothingItem> onTestFit, string clothingName, _E000 offer, bool isLocked = false)
	{
		_E1CF = onEquip;
		_E1D0 = onTestFit;
		Offer = offer;
		IsItemLocked = isLocked;
		_clothingName.text = clothingName;
		_lockedPanel.SetActive(IsItemLocked);
		Rect rect = _clothingIcon.rectTransform.rect;
		_E1D3 = Singleton<_E3DE>.Instance.GetIcon(Offer.Clothing, new _E313((int)rect.width, (int)rect.height));
		ShowGameObject();
		if (_E1D3 != null)
		{
			UI.AddDisposable(_E1D3.Changed.Bind(_E000));
			_clothingIcon.sprite = _E1D3.Sprite;
		}
	}

	private void _E000()
	{
		_clothingIcon.sprite = _E1D3.Sprite;
	}

	public void UpdateLock(bool isLocked)
	{
		IsItemLocked = isLocked;
		_lockedPanel.SetActive(isLocked);
		_E001();
	}

	public void UpdateView(bool? selected, bool? equipped)
	{
		_E1D2 = equipped ?? _E1D2;
		_E1D1 = selected ?? _E1D1;
		_E001();
	}

	private void _E001()
	{
		string key = (_E1D1 ? _ED3E._E000(258597) : (IsItemLocked ? _ED3E._E000(192960) : _ED3E._E000(30808)));
		Color color = _backgroudColor[key];
		Color color2 = _textColor[key];
		_equippedPanel.SetActive(_E1D2);
		_clothingIconBorder.color = color2;
		_clothingName.color = color2;
		_backgroud.color = color;
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		switch (eventData.button)
		{
		case PointerEventData.InputButton.Left:
			_E1CF(this);
			break;
		case PointerEventData.InputButton.Right:
			_E1D0(this);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case PointerEventData.InputButton.Middle:
			break;
		}
	}
}
