using System;
using EFT.InventoryLogic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EFT.UI;

public sealed class EditTagWindow : Window<_EC7C>, IPointerClickHandler, IEventSystemHandler, _E641, _E63F
{
	private static readonly Color[] m__E000 = new Color[7]
	{
		new Color(0.4f, 0f, 1f / 51f),
		new Color(0.4f, 0.22745098f, 3f / 85f),
		new Color(24f / 85f, 0.4f, 1f / 17f),
		new Color(1f / 85f, 0.4f, 28f / 85f),
		new Color(0.16862746f, 0f, 20f / 51f),
		new Color(0.4f, 0f, 0.22745098f),
		new Color(0.4f, 0.4f, 0.4f)
	};

	private const string m__E001 = "{0}/{1}";

	private const string m__E002 = "container tag";

	[SerializeField]
	private ValidationInputField _tagInput;

	[SerializeField]
	private TextMeshProUGUI _simbolsCounter;

	[SerializeField]
	private DefaultUIButton _saveButtonSpawner;

	[SerializeField]
	private TextMeshProUGUI _containerTagLabel;

	[SerializeField]
	private TagColorsPanel _colorsPanel;

	private TagComponent _E003;

	private IItemOwner _E004;

	private ColorView _E005;

	private int _E006 = -1;

	private Action _E007;

	private Action<string, int> _E008;

	public static Color GetColor(int colorIndex)
	{
		if (colorIndex >= EditTagWindow.m__E000.Length)
		{
			colorIndex = 0;
		}
		return EditTagWindow.m__E000[colorIndex];
	}

	public void Show(TagComponent tagComponent, Action onSelected, Action onClosed, Action<string, int> save)
	{
		Show(onClosed);
		_E003 = tagComponent;
		_E004 = _E003.Item.Parent.GetOwner();
		_E004.RegisterView(this);
		_E007 = onSelected;
		_tagInput.onValueChanged.AddListener(_E000);
		_saveButtonSpawner.OnClick.AddListener(_E002);
		_E008 = save;
		_tagInput.text = tagComponent.Name;
		_containerTagLabel.text = _ED3E._E000(247096).Localized();
		_E000(_tagInput.text);
		_colorsPanel.Show(EditTagWindow.m__E000, tagComponent.Color, _E001);
	}

	public void OnItemRemoved(_EAF3 obj)
	{
		Close();
	}

	private void _E000(string newText)
	{
		_simbolsCounter.text = string.Format(_ED3E._E000(182604), newText.Length, _tagInput.characterLimit);
	}

	private void _E001(ColorView cell, int colorIndex)
	{
		if (colorIndex != _E006)
		{
			if (_E005 != null)
			{
				_E005.SelectionChanged(selected: false);
			}
			_E005 = cell;
			_E005.SelectionChanged(selected: true);
			_E006 = colorIndex;
		}
	}

	private void _E002()
	{
		if (string.IsNullOrEmpty(_tagInput.text))
		{
			_E006 = 0;
		}
		if (_E008 != null && (!string.Equals(_tagInput.text, _E003.Name) || !object.Equals(_E006, _E003.Color)))
		{
			_E008(_tagInput.text, _E006);
		}
		Close();
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		if (_E007 != null)
		{
			_E007();
		}
	}

	public override void Close()
	{
		_E004.UnregisterView(this);
		_saveButtonSpawner.OnClick.RemoveAllListeners();
		_tagInput.onValueChanged.RemoveAllListeners();
		_E003 = null;
		base.Close();
	}
}
