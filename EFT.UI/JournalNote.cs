using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public class JournalNote : UIElement, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const string TRUNCATE_STRING = "...";

	[SerializeField]
	private CustomTextMeshProUGUI _noteName;

	[SerializeField]
	private CustomTextMeshProUGUI _noteDescription;

	[SerializeField]
	private Button _deleteNoteButton;

	[SerializeField]
	private Button _editNoteButton;

	[SerializeField]
	private LayoutElement _layoutElement;

	[SerializeField]
	private RectTransform _mask;

	[SerializeField]
	private int _maxTitleLength;

	private float _minHeight;

	private float _minTextHeight;

	private static JournalNote _selectedNote;

	private Action<_E9CD> _editButtonPressed;

	private Action<_E9CD> _destroyButtonPressed;

	public _E9CD Note { get; private set; }

	private void Awake()
	{
		_minHeight = _layoutElement.preferredHeight;
		_minTextHeight = _noteDescription.rectTransform.rect.height;
		_deleteNoteButton.gameObject.SetActive(value: false);
		_editNoteButton.gameObject.SetActive(value: false);
		_editNoteButton.onClick.AddListener(delegate
		{
			_editButtonPressed?.Invoke(Note);
		});
		_deleteNoteButton.onClick.AddListener(delegate
		{
			_destroyButtonPressed?.Invoke(Note);
		});
	}

	private void OnDestroy()
	{
		if (_selectedNote == this)
		{
			_selectedNote = null;
		}
	}

	public void Init([CanBeNull] Action<_E9CD> editButtonPressed, [CanBeNull] Action<_E9CD> destroyButtonPressed)
	{
		_editButtonPressed = editButtonPressed;
		_destroyButtonPressed = destroyButtonPressed;
	}

	internal void Show(_E9CD note)
	{
		Note = note;
		ShowGameObject();
		string text = string.Empty;
		int num = note.Text.IndexOf("\n", StringComparison.Ordinal);
		if (num == -1)
		{
			num = note.Text.Length;
		}
		num = Math.Min(num, _maxTitleLength - 1);
		num = Math.Min(num, note.Text.Length - 1);
		bool flag = false;
		if (note.Text[num] != '\n' && note.Text[num] != ' ' && num < note.Text.Length - 1)
		{
			int num2 = note.Text.LastIndexOf(' ', num);
			if (num2 == -1)
			{
				num2 = num;
			}
			num = num2;
			flag = true;
		}
		string text2 = note.Text.Substring(0, num + 1);
		if (num + 1 < note.Text.Length)
		{
			text = note.Text.Substring(num + 1, note.Text.Length - (num + 1));
		}
		text2 = text2.Trim();
		text = text.Trim();
		if (flag)
		{
			text2 += "...";
			text = "..." + text;
		}
		string text3 = _E5AD.LocalDateTimeFromUnixTime(note.Time).ToString("dd.MM.yyyy");
		_noteName.text = text2;
		_noteDescription.text = "[" + text3 + "]<color=#00000000>_</color>" + text;
	}

	public void ShowFull()
	{
		_layoutElement.preferredHeight = _minHeight + _noteDescription.rectTransform.rect.height - _minTextHeight;
		_mask.sizeDelta = _noteDescription.rectTransform.sizeDelta;
	}

	public void HideFull()
	{
		_layoutElement.preferredHeight = _minHeight;
		_mask.sizeDelta = new Vector2(_noteDescription.rectTransform.sizeDelta.x, _minTextHeight);
	}

	public void ButtonClickHandler()
	{
		if (_selectedNote != null && _selectedNote != this)
		{
			_selectedNote.HideFull();
		}
		if (_selectedNote == this)
		{
			_selectedNote.HideFull();
			_selectedNote = null;
		}
		else
		{
			ShowFull();
			_selectedNote = this;
		}
	}

	public void OnPointerEnter([NotNull] PointerEventData eventData)
	{
		_deleteNoteButton.gameObject.SetActive(value: true);
		_editNoteButton.gameObject.SetActive(value: true);
	}

	public void OnPointerExit([NotNull] PointerEventData eventData)
	{
		_deleteNoteButton.gameObject.SetActive(value: false);
		_editNoteButton.gameObject.SetActive(value: false);
	}
}
