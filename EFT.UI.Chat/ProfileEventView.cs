using System;
using System.Runtime.CompilerServices;
using EFT.Communications;
using TMPro;
using UnityEngine;

namespace EFT.UI.Chat;

public class ProfileEventView : UIElement
{
	[SerializeField]
	private TextMeshProUGUI _description;

	[SerializeField]
	private DefaultUIButton _applyButton;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[NonSerialized]
	public _ECED<ProfileChangeEvent> OnRedeem = new _ECED<ProfileChangeEvent>();

	private ProfileChangeEvent _E2EF;

	public void Show(ProfileChangeEvent profileEvent)
	{
		UI.Dispose();
		ShowGameObject();
		_E2EF = profileEvent;
		_E2EF.OnUpdate += _E000;
		UI.AddDisposable(delegate
		{
			_E2EF.OnUpdate -= _E000;
		});
		UI.SubscribeEvent(_applyButton.OnClick, _E001);
		_E000();
	}

	private void _E000()
	{
		_applyButton.Interactable = !_E2EF.Redeemed;
		_canvasGroup.alpha = (_E2EF.Redeemed ? 0.5f : 1f);
		_description.text = _E2EF.Description;
	}

	private void _E001()
	{
		OnRedeem?.Invoke(_E2EF);
	}

	[CompilerGenerated]
	private void _E002()
	{
		_E2EF.OnUpdate -= _E000;
	}
}
