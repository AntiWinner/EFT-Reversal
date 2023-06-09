using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EFT.UI;

public class NoteWindow : MonoBehaviour
{
	[SerializeField]
	private RectTransform _mainWindow;

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private TMP_InputField _note;

	[SerializeField]
	private CustomTextMeshProUGUI _lettersCounter;

	[SerializeField]
	private DefaultUIButton _button;

	private Action<string> _acceptAction;

	private Action _cancelAction;

	private RectTransform RectTransform => (RectTransform)base.transform;

	public void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(_mainWindow, putOnTop: false);
		_closeButton.onClick.AddListener(delegate
		{
			_cancelAction();
		});
		_button.OnClick.AddListener(delegate
		{
			_acceptAction(_note.text.Trim());
		});
		_note.characterLimit = 1024;
		_note.onValueChanged.AddListener(TextChangedHandler);
	}

	public void Show(Action<string> acceptAction, Action cancelAction)
	{
		_note.text = string.Empty;
		_acceptAction = acceptAction;
		_cancelAction = cancelAction;
		RectTransform.SetInCenter();
		TextChangedHandler(_note.text);
		UpdateAcceptButton();
		base.gameObject.SetActive(value: true);
	}

	public void FillData(_E9CD note)
	{
		_note.text = note.Text;
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	public void TextChangedHandler(string value)
	{
		_lettersCounter.text = _note.text.Length + "/" + 1024;
		UpdateAcceptButton();
	}

	public void TitleChangedHandler(string value)
	{
		UpdateAcceptButton();
	}

	public void UpdateAcceptButton()
	{
		_button.Interactable = _note.text.Length > 0;
	}
}
