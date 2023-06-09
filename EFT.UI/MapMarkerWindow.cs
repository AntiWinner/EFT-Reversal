using System;
using System.Collections.Generic;
using System.Linq;
using EFT.InventoryLogic;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class MapMarkerWindow : MonoBehaviour
{
	[Serializable]
	public class ToggleData
	{
		public MapMarkerType Type;

		public Toggle Toggle;
	}

	[SerializeField]
	private RectTransform _mainWindow;

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private DefaultUIButton _acceptButton;

	[SerializeField]
	private ValidationInputField _note;

	[SerializeField]
	private CustomTextMeshProUGUI _lettersCounter;

	[SerializeField]
	private ToggleData[] _toggles;

	private Action<MapMarkerType, string> _acceptAction;

	private Action _cancelAction;

	private MapMarkerType _selectedType = MapMarkerType.Other;

	private void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(_mainWindow, putOnTop: false);
		_closeButton.onClick.AddListener(delegate
		{
			_cancelAction();
		});
		_acceptButton.OnClick.AddListener(delegate
		{
			_acceptAction(_selectedType, _note.text);
		});
		ToggleData[] toggles = _toggles;
		foreach (ToggleData toggleData in toggles)
		{
			ToggleData toggleDataSave = toggleData;
			toggleData.Toggle.onValueChanged.AddListener(delegate(bool value)
			{
				MarkerTypeChangedHandler(toggleDataSave.Type, value);
			});
		}
	}

	private void Start()
	{
		_note.onValueChanged.AddListener(TextChangedHandler);
	}

	private void OnDestroy()
	{
		_note.onValueChanged.RemoveListener(TextChangedHandler);
	}

	public void Show(Vector2 position, Action<MapMarkerType, string> acceptAction, Action cancelAction)
	{
		base.gameObject.SetActive(value: true);
		_acceptAction = acceptAction;
		_cancelAction = cancelAction;
		_note.text = string.Empty;
		_note.characterLimit = 75;
		_mainWindow.position = position;
		_mainWindow.CorrectPositionResolution();
		_note.ActivateInputField();
		_note.Select();
		UpdateLettersCounter();
	}

	public void FillWithData(MapMarker marker)
	{
		_note.text = marker.Note;
		SelectToggle(marker.Type);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void MarkerTypeChangedHandler(MapMarkerType type, bool value)
	{
		if (value)
		{
			_selectedType = type;
		}
	}

	private void TextChangedHandler(string value)
	{
		UpdateLettersCounter();
	}

	private void UpdateLettersCounter()
	{
		_lettersCounter.text = $"{_note.text.Length}/{75}";
	}

	private void SelectToggle(MapMarkerType type)
	{
		using IEnumerator<ToggleData> enumerator = _toggles.Where((ToggleData toggleData) => toggleData.Type == type).GetEnumerator();
		if (enumerator.MoveNext())
		{
			enumerator.Current.Toggle.isOn = true;
		}
	}
}
