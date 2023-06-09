using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using EFT.Console.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class ConsoleAutoCompletePanel : MonoBehaviour
{
	private const int m__E000 = 30;

	[SerializeField]
	private GameObject _container;

	[SerializeField]
	private ConsoleAutoCompleteItem _prefabItem;

	[SerializeField]
	private GameObject _documentationContainer;

	[SerializeField]
	private TMP_Text _documentation;

	[CompilerGenerated]
	private Action<AutoCompleteItem> _E001;

	private int _E002;

	private List<(AutoCompleteItem, ConsoleAutoCompleteItem)> _E003;

	public event Action<AutoCompleteItem> OnSelect
	{
		[CompilerGenerated]
		add
		{
			Action<AutoCompleteItem> action = _E001;
			Action<AutoCompleteItem> action2;
			do
			{
				action2 = action;
				Action<AutoCompleteItem> value2 = (Action<AutoCompleteItem>)Delegate.Combine(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
		[CompilerGenerated]
		remove
		{
			Action<AutoCompleteItem> action = _E001;
			Action<AutoCompleteItem> action2;
			do
			{
				action2 = action;
				Action<AutoCompleteItem> value2 = (Action<AutoCompleteItem>)Delegate.Remove(action2, value);
				action = Interlocked.CompareExchange(ref _E001, value2, action2);
			}
			while ((object)action != action2);
		}
	}

	private void Awake()
	{
		_prefabItem.gameObject.SetActive(value: false);
		_documentationContainer.gameObject.SetActive(value: false);
		_E003 = new List<(AutoCompleteItem, ConsoleAutoCompleteItem)>();
	}

	public void UpdateList(AutoCompleteItem[] items)
	{
		foreach (Transform item in _container.transform)
		{
			if (!(item.gameObject == _prefabItem.gameObject))
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
		_E003.Clear();
		_E002 = 0;
		for (int i = 0; i < items.Length && i < 30; i++)
		{
			ConsoleAutoCompleteItem consoleAutoCompleteItem = UnityEngine.Object.Instantiate(_prefabItem, _container.transform, worldPositionStays: false);
			consoleAutoCompleteItem.gameObject.SetActive(value: true);
			consoleAutoCompleteItem.IsSelected = false;
			consoleAutoCompleteItem.Setup(items[i]);
			_E003.Add((items[i], consoleAutoCompleteItem));
		}
		_E000();
	}

	private void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (_E002 > 0)
			{
				_E002--;
			}
			_E000();
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (_E002 < _E003.Count - 1)
			{
				_E002++;
			}
			_E000();
		}
		else if (Input.GetKeyDown(KeyCode.Tab) && _E002 >= 0 && _E002 < _E003.Count)
		{
			_E001?.Invoke(_E003[_E002].Item1);
		}
	}

	private void _E000()
	{
		if (_E003.Count == 0 && _documentationContainer.activeSelf)
		{
			_documentationContainer.SetActive(value: false);
		}
		else if (_E003.Count > 0 && !_documentationContainer.activeSelf)
		{
			_documentationContainer.SetActive(value: true);
		}
		for (int i = 0; i < _E003.Count; i++)
		{
			_E003[i].Item2.IsSelected = _E002 == i;
			if (_E003[i].Item2.IsSelected)
			{
				_documentation.text = _E003[i].Item1.Description;
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(_documentationContainer.RectTransform());
	}
}
