using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EFT.UI;

public sealed class ClothingConfirmationWindow : UIElement, IPointerClickHandler, IEventSystemHandler
{
	[SerializeField]
	private RectTransform _windowTransform;

	[SerializeField]
	private GameObject _captionPanel;

	[SerializeField]
	private DefaultUIButton _unlockButton;

	[SerializeField]
	private TextMeshProUGUI _suiteName;

	[SerializeField]
	private Button _closeButton;

	[SerializeField]
	private ClothingRequirements _requirements;

	private Action _E0EA;

	private Action _E0EB;

	private void Awake()
	{
		_captionPanel.AddComponent<UIDragComponent>().Init(_windowTransform, putOnTop: true);
		_unlockButton.OnClick.AddListener(delegate
		{
			_E0EA?.Invoke();
			Close();
		});
		_closeButton.onClick.AddListener(delegate
		{
			_E0EB?.Invoke();
			Close();
		});
	}

	public void Show(string suiteName, Profile profile, Profile._E001 trader, _E934 quests, _EBE3 requirements, _E9EF stashGrid, Action unlockAction, [CanBeNull] Action cancelAction)
	{
		_E0EA = unlockAction;
		_E0EB = cancelAction;
		_suiteName.text = suiteName;
		ShowGameObject();
		_E000(availableStatus: true);
		_requirements.Show(profile, trader, quests, stashGrid, requirements, _E000);
	}

	private void _E000(bool availableStatus)
	{
		_unlockButton.Interactable = availableStatus;
	}

	protected override void CorrectPosition(_E3F3 margins = default(_E3F3))
	{
		_windowTransform.RectTransform().CorrectPositionResolution(margins);
	}

	private void _E001()
	{
		base.transform.SetAsLastSibling();
	}

	public void OnPointerClick([NotNull] PointerEventData eventData)
	{
		_E001();
	}

	public override void Close()
	{
		_requirements.Close();
		base.Close();
	}

	[CompilerGenerated]
	private void _E002()
	{
		_E0EA?.Invoke();
		Close();
	}

	[CompilerGenerated]
	private void _E003()
	{
		_E0EB?.Invoke();
		Close();
	}
}
