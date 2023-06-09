using System;
using System.Linq;
using System.Runtime.CompilerServices;
using EFT.UI;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.HandBook;

public sealed class EntityListElement : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private EntityIcon _icon;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private GameObject _newNodeObject;

	[SerializeField]
	private TextMeshProUGUI _name;

	[SerializeField]
	private TextMeshProUGUI _itemCategory;

	[SerializeField]
	private Color32 _defaultTextColor = new Color32(197, 195, 178, byte.MaxValue);

	[SerializeField]
	private Color32 _defaultCategoryColor = new Color32(166, 176, 181, byte.MaxValue);

	[SerializeField]
	private Color32 _hoverTextColor = Color.black;

	[SerializeField]
	private Color _selectedBackgroundColor = new Color32(197, 195, 178, byte.MaxValue);

	private _EBAB _E086;

	private Action<_EBAB> _E087;

	private _E3E2 _E084;

	[CompilerGenerated]
	private bool _E088;

	private bool _E000
	{
		[CompilerGenerated]
		get
		{
			return _E088;
		}
		[CompilerGenerated]
		set
		{
			_E088 = value;
		}
	}

	private Color _E001 => new Color(0.275f, 0.275f, 0.275f, 0.314f);

	private Color _E002
	{
		get
		{
			Color color = this._E001;
			return new Color(color.r, color.g, color.b, 0.85f);
		}
	}

	public void Show(_EBAB node, Action<_EBAB> onChosen)
	{
		_E086 = node;
		_E087 = onChosen;
		_icon.Show(node.Data.Item);
		string text = (node.Data.FromBuild ? node.Data.Name : node.Data.Name.Localized());
		_name.text = text;
		_itemCategory.text = string.Join(_ED3E._E000(197193), (from x in node.Category.TakeLast(2)
			select x.Localized()).ToArray());
		_E086.OnNewStatusUpdated += _E000;
		UI.AddDisposable(delegate
		{
			_E086.OnNewStatusUpdated -= _E000;
		});
		_E000(_E086.New);
		ShowGameObject();
		DeselectView();
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		if (!this._E000)
		{
			_background.color = this._E002;
		}
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		if (!this._E000)
		{
			DeselectView();
		}
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (!this._E000)
		{
			_E001();
			_E087(_E086);
		}
	}

	private void _E000(bool isNew)
	{
		if (!(this == null) && !(_newNodeObject == null))
		{
			_newNodeObject.SetActive(isNew);
		}
	}

	private void _E001()
	{
		this._E000 = true;
		_background.color = _selectedBackgroundColor;
		_name.color = _hoverTextColor;
		_itemCategory.color = _hoverTextColor;
	}

	public void DeselectView()
	{
		this._E000 = false;
		_background.color = this._E001;
		_name.color = _defaultTextColor;
		_itemCategory.color = _defaultCategoryColor;
	}

	public override void Close()
	{
		_icon.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E002()
	{
		_E086.OnNewStatusUpdated -= _E000;
	}
}
