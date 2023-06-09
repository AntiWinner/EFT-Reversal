using System;
using System.Collections.Generic;
using EFT.InputSystem;
using UnityEngine;

namespace EFT.UI.Gestures;

public sealed class GesturesBindPanel : UIElement
{
	[SerializeField]
	private GesturesBindItem _leftTemplate;

	[SerializeField]
	private GesturesBindItem _rightTemplate;

	[SerializeField]
	private RectTransform _leftContainer;

	[SerializeField]
	private RectTransform _rightContainer;

	[NonSerialized]
	public readonly _ECED<int> OnItemSelected = new _ECED<int>();

	private readonly List<GesturesBindItem> _E303 = new List<GesturesBindItem>();

	public IReadOnlyList<GesturesBindItem> Items => _E303;

	private void Awake()
	{
		_leftTemplate.gameObject.SetActive(value: false);
		_rightTemplate.gameObject.SetActive(value: false);
	}

	public void Show(_ECAB binds, ColorMap colorMap, HashSet<EPhraseTrigger> availablePhrases)
	{
		ShowGameObject();
		if (_E303.Count > 0)
		{
			foreach (GesturesBindItem item in _E303)
			{
				binds.TryGetValue(item.Command, out var value);
				item.Show(value, _ECAB.GetCommandName(value));
			}
			return;
		}
		for (ECommand eCommand = ECommand.F1; eCommand <= ECommand.F12; eCommand++)
		{
			bool num = (int)eCommand % 2 == 0;
			GesturesBindItem original = (num ? _rightTemplate : _leftTemplate);
			RectTransform parent = (num ? _rightContainer : _leftContainer);
			GesturesBindItem gesturesBindItem = UnityEngine.Object.Instantiate(original, parent);
			binds.TryGetValue(eCommand, out var value2);
			gesturesBindItem.Init(colorMap, eCommand, availablePhrases);
			gesturesBindItem.Show(value2, _ECAB.GetCommandName(value2));
			gesturesBindItem.OnPointerClicked.Subscribe(_E000);
			_E303.Add(gesturesBindItem);
		}
	}

	public override void Close()
	{
		_E001();
		base.Close();
	}

	private void _E000(GestureBaseItem._E000 click)
	{
		OnItemSelected.Invoke(click.ItemIndex);
	}

	private void _E001()
	{
		foreach (GesturesBindItem item in _E303)
		{
			item.Close();
		}
	}
}
