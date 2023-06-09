using System.Collections.Generic;
using System.Linq;
using EFT.InputSystem;
using UnityEngine;

namespace EFT.UI.Gestures;

public class GesturesDropdownPanel : UIInputNode
{
	[SerializeField]
	private GesturesQuickPanelItem _quickTemplate;

	[SerializeField]
	private RectTransform _container;

	[SerializeField]
	private GameObject _dropdownObject;

	private readonly SortedDictionary<EPhraseTrigger, GesturesQuickPanelItem> _E0BC = new SortedDictionary<EPhraseTrigger, GesturesQuickPanelItem>();

	private readonly SortedDictionary<EPhraseTrigger, GesturesQuickPanelItem> _E0BD = new SortedDictionary<EPhraseTrigger, GesturesQuickPanelItem>();

	internal EPhraseTrigger _E0BE;

	private int _E0BF;

	public bool DropdownPanelActive => _dropdownObject.activeSelf;

	private void _E000()
	{
		EPhraseTrigger[] array = new EPhraseTrigger[8]
		{
			EPhraseTrigger.OnWeaponReload,
			EPhraseTrigger.OnWeaponJammed,
			EPhraseTrigger.Hit,
			EPhraseTrigger.OnGrenade,
			EPhraseTrigger.OnBeingHurt,
			EPhraseTrigger.OnFight,
			EPhraseTrigger.OnEnemyDown,
			EPhraseTrigger.OnEnemyShot
		};
		foreach (EPhraseTrigger trigger in array)
		{
			_E001(trigger);
		}
	}

	private void Start()
	{
		_E000();
	}

	public void AddCommand(EPhraseTrigger trigger)
	{
		if (!_E0BC.ContainsKey(EPhraseTrigger.PhraseNone))
		{
			GesturesQuickPanelItem gesturesQuickPanelItem = _E001(trigger);
			gesturesQuickPanelItem.Show(EPhraseTrigger.PhraseNone);
			_E0BC.Add(EPhraseTrigger.PhraseNone, gesturesQuickPanelItem);
		}
		GesturesQuickPanelItem gesturesQuickPanelItem2 = _E001(trigger);
		gesturesQuickPanelItem2.Show(trigger);
		gesturesQuickPanelItem2.Deselect();
		_E0BC.Add(trigger, gesturesQuickPanelItem2);
		_E002();
	}

	private GesturesQuickPanelItem _E001(EPhraseTrigger trigger)
	{
		if (!_E0BD.TryGetValue(trigger, out var value))
		{
			value = Object.Instantiate(_quickTemplate, _container);
			_E0BD[trigger] = value;
		}
		return value;
	}

	public void RemoveCommand(EPhraseTrigger trigger)
	{
		_E0BC[trigger].Close();
		_E0BC.Remove(trigger);
		if (_E0BC.Count == 1 && _E0BC.ContainsKey(EPhraseTrigger.PhraseNone))
		{
			RemoveCommand(EPhraseTrigger.PhraseNone);
		}
		_E002();
	}

	public void ShowDropdown()
	{
		_dropdownObject.SetActive(value: true);
		if (_E0BC.Count > 0)
		{
			foreach (KeyValuePair<EPhraseTrigger, GesturesQuickPanelItem> item in _E0BC)
			{
				item.Value.Deselect();
			}
			_E0BF = 0;
			_E0BC.ElementAt(_E0BF).Value.Select();
			_E0BE = _E0BC.ElementAt(_E0BF).Key;
		}
		else
		{
			_E0BE = EPhraseTrigger.PhraseNone;
		}
	}

	private void _E002()
	{
		foreach (GesturesQuickPanelItem value in _E0BC.Values)
		{
			value.transform.SetAsLastSibling();
		}
	}

	public void CloseDropdown()
	{
		_dropdownObject.SetActive(value: false);
	}

	protected override ETranslateResult TranslateCommand(ECommand command)
	{
		if (_dropdownObject.activeSelf && _E0BC.Count > 0)
		{
			if (command.IsCommand(ECommand.ScrollPrevious))
			{
				if (_E0BC.ContainsKey(_E0BE))
				{
					_E0BC[_E0BE].Deselect();
				}
				_E0BF++;
				if (_E0BF >= _E0BC.Count)
				{
					_E0BF = 0;
				}
				_E0BC.ElementAt(_E0BF).Value.Select();
				_E0BE = _E0BC.ElementAt(_E0BF).Key;
				return ETranslateResult.Block;
			}
			if (command.IsCommand(ECommand.ScrollNext))
			{
				if (_E0BC.ContainsKey(_E0BE))
				{
					_E0BC[_E0BE].Deselect();
				}
				_E0BF--;
				if (_E0BF < 0)
				{
					_E0BF = _E0BC.Count - 1;
				}
				_E0BC.ElementAt(_E0BF).Value.Select();
				_E0BE = _E0BC.ElementAt(_E0BF).Key;
				return ETranslateResult.Block;
			}
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
}
